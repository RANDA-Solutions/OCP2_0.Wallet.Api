using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LevelData.Credentials.DIDForge.Abstractions;
using LevelData.Credentials.DIDForge.Extensions;
using LevelData.Credentials.DIDForge.Services;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Credentials.Cryptography;
using OpenCredentialPublisher.Credentials.VerifiableCredentials;
using OpenCredentialPublisher.Data.Custom.CredentialModels;
using OpenCredentialPublisher.JsonLD;
using PemUtils;
using JwtHeader = OpenCredentialPublisher.Proof.Models.JwtHeader;

namespace OpenCredentialPublisher.Proof
{
    public class ProofService : IProofService
    {

        private readonly Regex _keyDid;
        private readonly Regex _isDid;
        private readonly Regex _isUrl;
        private readonly Regex _jwsRegex;
        private readonly DidResolver _didResolver;


        public ProofService(DidResolver didResolver)
        {
            _jwsRegex = new Regex("(?<header>[a-zA-Z0-9+_]+)?\\.(?<body>[a-zA-Z0-9+_]+)?\\.(?<signature>[a-zA-Z0-9+-_]+)");
            _keyDid = new Regex("did:key:(?<key>[a-km-zA-HJ-NP-Z1-9]+)");
            _isDid = new Regex("^did:");
            _isUrl = new Regex("^http[s]?:");
            _didResolver = didResolver;
        }

        public async Task<bool> VerifyProof(string originalJson)
        {
            JObject document;

            await using (var reader = new JsonTextReader(new StringReader(originalJson)))
            {
                reader.DateParseHandling = DateParseHandling.None;
                document = JObject.Load(reader);
            }

            if (!document.ContainsKey("proof"))
            {
                return false;
            }

            var proofs = document["proof"];
            document.Remove("proof");
            var verified = false;
            if (proofs is { Type: JTokenType.Array })
            {
                foreach (var proofJson in proofs.Children())
                {
                    verified = await ProcessProof(document, proofJson);
                    if (!verified)
                        break;
                }
            }
            else if (proofs is { Type: JTokenType.Object })
            {
                verified = await ProcessProof(document, proofs);
            }

            return verified;
        }

        private async Task<bool> ProcessProof(JObject document, JToken proofJson)
        {
            var jsonldSignature = new JsonLinkedDataSignature();
            var proof = System.Text.Json.JsonSerializer.Deserialize<ProofModel>(proofJson.ToString());

            var algorithm = proof.Type switch
            {
                "Ed25519Signature2018" => KeyAlgorithmEnum.Ed25519,
                "Ed25519Signature2020" => KeyAlgorithmEnum.Ed25519,
                "Ed25519VerificationKey2020" => KeyAlgorithmEnum.Ed25519,
                _ => KeyAlgorithmEnum.RSA
            };

            var verifydata = jsonldSignature.CreateVerifyData(document, proofJson);
            var publicKeyBytes = await GetPublicKeyAsync(proof);
            if (publicKeyBytes == null)
                return false;

            var signature = GetSignature(proof);
            byte[] signatureBytes;
            if (signature.StartsWith("z"))
            {
                signatureBytes = CryptoMethods.Base58DecodeString(signature);

            }
            else
            {
                signatureBytes = WebEncoders.Base64UrlDecode(signature);
            }

            if (!String.IsNullOrEmpty(proof.JWS) && _jwsRegex.IsMatch(proof.JWS))
            {
                var match = _jwsRegex.Match(proof.JWS);
                var header = match.Groups["header"].Value;
                var headerJson = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(header));
                var jwtHeader = System.Text.Json.JsonSerializer.Deserialize<JwtHeader>(headerJson);
                var headerBytes = Encoding.UTF8.GetBytes(header + ".");
                byte[] jws;
                if (jwtHeader.B64)
                {
                    var encodedVerifyDataBytes = Encoding.UTF8.GetBytes(WebEncoders.Base64UrlEncode(verifydata));
                    jws = new byte[headerBytes.Length + encodedVerifyDataBytes.Length];
                    headerBytes.CopyTo(jws, 0);
                    encodedVerifyDataBytes.CopyTo(jws, headerBytes.Length);
                }
                else
                {
                    jws = new byte[headerBytes.Length + verifydata.Length];
                    headerBytes.CopyTo(jws, 0);
                    verifydata.CopyTo(jws, headerBytes.Length);
                }

                return CryptoMethods.VerifySignature(algorithm, publicKeyBytes, signatureBytes, jws);
            }
            else
            {
                return CryptoMethods.VerifySignature(algorithm, publicKeyBytes, signatureBytes, verifydata);
            }
        }

        private async Task<byte[]> GetPublicKeyAsync(ProofModel proof)
        {
            string verificationMethod;

            if (proof.VerificationMethod is BasicPropertiesModel val)
            {
                verificationMethod = val.Id;
            }
            else
            {
                verificationMethod = proof.VerificationMethod.ToString();
            }
            if (verificationMethod != null && _isDid.IsMatch(verificationMethod))
            {
                var resolver = _didResolver.GetResolver(verificationMethod);
                var didDocument = await resolver.ResolveDidDocumentAsync(verificationMethod);
                var publicKeyMultibase = didDocument.GetPublicKeyMultibaseForUse(verificationMethod, proof.ProofPurpose);
                if (publicKeyMultibase != null) {
                    var keyBytes = CryptoMethods.Base58DecodeString(publicKeyMultibase);
                    var length = keyBytes.Length - 32;
                    return length > 0 ? keyBytes.Skip(length).ToArray() : keyBytes;
                }
            }
            if (verificationMethod != null && _isUrl.IsMatch(verificationMethod))
            {
                var verificationUrl = SanitizePath(verificationMethod);
                var publicKey = await GetKeyAsync(verificationUrl);

                if (proof.Type == ProofTypeEnum.RsaSignature2018.ToString())
                {
                    RSAParameters rsaParameters;
                    await using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(publicKey)))
                    {
                        using var reader = new PemReader(stream);
                        rsaParameters = reader.ReadRsaKey();
                    }

                    using var crypto = new RSACryptoServiceProvider();
                    crypto.ImportParameters(rsaParameters);
                    return crypto.ExportCspBlob(false);
                }
                if (proof.Type == ProofTypeEnum.Ed25519Signature2018.ToString())
                {
                    return CryptoMethods.Base58DecodeString(publicKey);
                }
            }
            return null;
        }

        private async Task<string> GetKeyAsync(string url)
        {
            using var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // ReSharper disable once ConvertTypeCheckToNullCheck
            if (response.Content is object)
            {
                return await response.Content.ReadAsStringAsync();
            }
            throw new Exception("There was an issue processing your connect request.");
        }

        private string SanitizePath(string url)
        {
            var builder = new UriBuilder(url);
            if (builder.Path.StartsWith("//"))
            {
                builder.Path = builder.Path.Trim('/');
            }
            return builder.ToString();
        }

        private string GetSignature(ProofModel proof)
        {
            if (proof.JWS != null && _jwsRegex.IsMatch(proof.JWS))
            {
                var jwsMatch = _jwsRegex.Match(proof.JWS);
                return jwsMatch.Groups["signature"].Value;
            }
            else if (proof.Signature != null)
            {
                return proof.Signature;
            }
            else
            {
                return proof.ProofValue;
            }
        }
    }
}


