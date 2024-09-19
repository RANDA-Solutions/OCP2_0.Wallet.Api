// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLD.JsonLinkedDataSignature
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable enable
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ExpandOptions = OpenCredentialPublisher.Data.Custom.JsonLD.Normalization.ExpandOptions;
using JsonLdHandler = OpenCredentialPublisher.Data.Custom.JsonLD.Normalization.JsonLdHandler;

namespace OpenCredentialPublisher.Data.Custom.JsonLD
{
    public class JsonLinkedDataSignature
    {
        public
#nullable disable
        byte[] CreateVerifyData(JObject document, JToken proof)
        {
            return CreateVerifyDataAsync(document, proof).Result;
        }

        public async Task<byte[]> CreateVerifyDataAsync(JObject document, JToken proof)
        {
            byte[] documentHash = await CanonizeDocumentAsync(document);
            byte[] proofOptionsHash = await CanonizeProofAsync(document, proof);
            byte[] array = new byte[proofOptionsHash.Length + documentHash.Length];
            proofOptionsHash.CopyTo(array, 0);
            documentHash.CopyTo(array, proofOptionsHash.Length);
            byte[] verifyDataAsync = array;
            documentHash = null;
            proofOptionsHash = null;
            array = null;
            return verifyDataAsync;
        }

        public async Task<byte[]> CanonizeDocumentAsync(JObject document)
        {
            string normalizedJson = await JsonLdHandler.Normalize(document.ToString(), new ExpandOptions()
            {
                Base = "c14n",
                KeepFreeFloatingNodes = true
            });
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(normalizedJson);
                hash = sha256.ComputeHash(textBytes);
            }
            normalizedJson = null;
            return hash;
        }

        public async Task<byte[]> CanonizeProofAsync(JObject document, JToken proof)
        {
            var serializedProof = JsonSerializer.Serialize(proof, (JsonSerializerOptions)null);
            var proofDocument = JObject.Parse("{}");
            proofDocument.Add("@context", new JArray());
            var contextArray = proofDocument["@context"] as JArray;
            contextArray!.Add((JToken)"https://w3id.org/security/suites/ed25519-2020/v1");
            proofDocument.Merge(proof);
            proofDocument.Remove("jws");
            proofDocument.Remove("signatureValue");
            proofDocument.Remove("proofValue");
            proofDocument.Remove("signature");
            byte[] numArray = await CanonizeDocumentAsync(proofDocument);
            serializedProof = null;
            proofDocument = null;
            contextArray = null;
            return numArray;
        }
    }
}
