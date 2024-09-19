// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.URDNA2015
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal static class URDNA2015
    {
        public static string Normalize(List<Quad> dataset)
        {
            Dictionary<string, BlankNodeInfo> blankNodeInfo = new Dictionary<string, BlankNodeInfo>();
            IdentifierIssuer canonicalIssuer = new IdentifierIssuer("_:c14n");
            List<Quad> quadList = dataset;
            foreach (Quad quad in dataset)
            {
                AddBlankNodeQuadInfo(quad, quad.Subject, blankNodeInfo);
                AddBlankNodeQuadInfo(quad, quad.Object, blankNodeInfo);
                AddBlankNodeQuadInfo(quad, quad.Graph, blankNodeInfo);
            }
            Dictionary<string, HashSet<string>> hashToBlankNodes = new Dictionary<string, HashSet<string>>();
            foreach (string id in blankNodeInfo.Keys.ToArray())
                HashAndTrackBlankNode(id, hashToBlankNodes, blankNodeInfo);
            string[] array = hashToBlankNodes.Keys.OrderBy(k => k).ToArray();
            List<HashSet<string>> stringSetList = new List<HashSet<string>>();
            foreach (string key in array)
            {
                HashSet<string> source = hashToBlankNodes[key];
                if (source.Count > 1)
                {
                    stringSetList.Add(source);
                }
                else
                {
                    string old = source.First();
                    canonicalIssuer.GetId(old);
                }
            }
            foreach (HashSet<string> stringSet in stringSetList)
            {
                List<(string, IdentifierIssuer)> valueTupleList = new List<(string, IdentifierIssuer)>();
                foreach (string str in stringSet)
                {
                    if (!canonicalIssuer.HasId(str))
                    {
                        IdentifierIssuer issuer = new IdentifierIssuer("_:b");
                        issuer.GetId(str);
                        (string, IdentifierIssuer) valueTuple = HashNDegreeQuads(str, issuer, blankNodeInfo, canonicalIssuer);
                        valueTupleList.Add(valueTuple);
                    }
                }
                valueTupleList.Sort((a, b) => string.Compare(a.Item1, b.Item1));
                foreach ((string, IdentifierIssuer) valueTuple in valueTupleList)
                {
                    foreach (string oldId in valueTuple.Item2.GetOldIds())
                        canonicalIssuer.GetId(oldId);
                }
            }
            List<string> values = new List<string>();
            foreach (Quad quad1 in quadList)
            {
                Quad quad2 = new Quad()
                {
                    Subject = UseCanonicalId(quad1.Subject, canonicalIssuer),
                    Object = UseCanonicalId(quad1.Object, canonicalIssuer),
                    Graph = UseCanonicalId(quad1.Graph, canonicalIssuer),
                    Predicate = quad1.Predicate
                };
                values.Add(SerializeQuad(quad2));
            }
            values.Sort(StringComparer.Ordinal);
            return string.Join("", (IEnumerable<string>)values);
        }

        private static void AddBlankNodeQuadInfo(
          Quad quad,
          QuadItem component,
          Dictionary<string, BlankNodeInfo> blankNodeInfo)
        {
            if (component.TermType != TermType.BlankNode)
                return;
            string key = component.Value;
            BlankNodeInfo blankNodeInfo1;
            if (blankNodeInfo.TryGetValue(key, out blankNodeInfo1))
                blankNodeInfo1.Quads.Add(quad);
            else
                blankNodeInfo[key] = new BlankNodeInfo()
                {
                    Quads = new HashSet<Quad>() { quad },
                    Hash = null
                };
        }

        private static void HashAndTrackBlankNode(
          string id,
          Dictionary<string, HashSet<string>> hashToBlankNodes,
          Dictionary<string, BlankNodeInfo> blankNodeInfo)
        {
            string key = HashFirstDegreeQuads(id, blankNodeInfo);
            HashSet<string> stringSet;
            if (!hashToBlankNodes.TryGetValue(key, out stringSet))
                hashToBlankNodes[key] = new HashSet<string>() { id };
            else
                stringSet.Add(id);
        }

        private static string HashFirstDegreeQuads(
          string id,
          Dictionary<string, BlankNodeInfo> blankNodeInfo)
        {
            List<string> stringList = new List<string>();
            BlankNodeInfo blankNodeInfo1 = blankNodeInfo[id];
            foreach (Quad quad1 in blankNodeInfo1.Quads)
            {
                Quad quad2 = new Quad()
                {
                    Subject = null,
                    Predicate = quad1.Predicate,
                    Object = null,
                    Graph = null
                };
                quad2.Subject = ModifyFirstDegreeComponent(id, quad1.Subject);
                quad2.Object = ModifyFirstDegreeComponent(id, quad1.Object);
                quad2.Graph = ModifyFirstDegreeComponent(id, quad1.Graph);
                stringList.Add(SerializeQuad(quad2));
            }
            stringList.Sort(StringComparer.Ordinal);
            MessageDigest messageDigest = new MessageDigest();
            foreach (string data in stringList)
                messageDigest.Update(data);
            blankNodeInfo1.Hash = messageDigest.Digest();
            return blankNodeInfo1.Hash;
        }

        private static T ModifyFirstDegreeComponent<T>(string id, T component) where T : QuadItem, new()
        {
            if (component.TermType != TermType.BlankNode)
                return component;
            T obj = new T();
            obj.TermType = TermType.BlankNode;
            obj.Value = component.Value == id ? "_:a" : "_:z";
            return obj;
        }

        private static string SerializeQuad(Quad quad)
        {
            QuadItem subject = quad.Subject;
            QuadItem predicate = quad.Predicate;
            ObjectQuadItem objectQuadItem = quad.Object;
            QuadItem graph = quad.Graph;
            StringBuilder stringBuilder = new StringBuilder();
            if (subject.TermType == TermType.NamedNode)
                stringBuilder.Append("<" + subject.Value + ">");
            else
                stringBuilder.Append(subject.Value ?? "");
            stringBuilder.Append(" <" + predicate.Value + "> ");
            if (objectQuadItem.TermType == TermType.NamedNode)
                stringBuilder.Append("<" + objectQuadItem.Value + ">");
            else if (objectQuadItem.TermType == TermType.BlankNode)
            {
                stringBuilder.Append(objectQuadItem.Value);
            }
            else
            {
                stringBuilder.Append("\"" + EscapeQuadValue(objectQuadItem.Value) + "\"");
                if (objectQuadItem.DataType.Value == "http://www.w3.org/1999/02/22-rdf-syntax-ns#langString")
                {
                    if (!string.IsNullOrEmpty(objectQuadItem.Language))
                        stringBuilder.Append("@" + objectQuadItem.Language);
                }
                else if (objectQuadItem.DataType.Value != "http://www.w3.org/2001/XMLSchema#string")
                    stringBuilder.Append("^^<" + objectQuadItem.DataType.Value + ">");
            }
            if (graph.TermType == TermType.NamedNode)
                stringBuilder.Append(" <" + graph.Value + ">");
            else if (graph.TermType == TermType.BlankNode)
                stringBuilder.Append(" " + graph.Value);
            stringBuilder.Append(" .\n");
            return stringBuilder.ToString();
        }

        private static string EscapeQuadValue(string value)
        {
            return Regex.Replace(value, "[\"\\\\\\n\\r]", m =>
            {
                string str1 = m.Value;
                string str2;
                switch (str1)
                {
                    case "\"":
                        str2 = "\\\"";
                        break;
                    case "\\":
                        str2 = "\\\\";
                        break;
                    case "\n":
                        str2 = "\\n";
                        break;
                    case "\r":
                        str2 = "\\r";
                        break;
                    default:
                        str2 = m.Value;
                        break;
                }
                return str2;
            });
        }

        private static (string, IdentifierIssuer) HashNDegreeQuads(
          string id,
          IdentifierIssuer issuer,
          Dictionary<string, BlankNodeInfo> blankNodeInfo,
          IdentifierIssuer canonicalIssuer)
        {
            MessageDigest messageDigest = new MessageDigest();
            Dictionary<string, List<string>> hashToRelated = CreateHashToRelated(id, issuer, blankNodeInfo, canonicalIssuer);
            List<string> list = hashToRelated.Keys.ToList();
            list.Sort();
            foreach (string str1 in list)
            {
                messageDigest.Update(str1);
                string str2 = "";
                IdentifierIssuer identifierIssuer1 = null;
                Permuter permuter = new Permuter(hashToRelated[str1]);
                while (permuter.HasNext())
                {
                    List<string> stringList1 = permuter.Next();
                    IdentifierIssuer issuer1 = issuer.Clone();
                    StringBuilder stringBuilder = new StringBuilder();
                    List<string> stringList2 = new List<string>();
                    bool flag = false;
                    foreach (string old in stringList1)
                    {
                        if (canonicalIssuer.HasId(old))
                        {
                            stringBuilder.Append(canonicalIssuer.GetId(old));
                        }
                        else
                        {
                            if (!issuer1.HasId(old))
                                stringList2.Add(old);
                            stringBuilder.Append(issuer1.GetId(old));
                        }
                        if (str2.Length != 0 && string.Compare(stringBuilder.ToString(), str2) > 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        foreach (string str3 in stringList2)
                        {
                            (string str4, IdentifierIssuer identifierIssuer2) = HashNDegreeQuads(str3, issuer1, blankNodeInfo, canonicalIssuer);
                            stringBuilder.Append(issuer1.GetId(str3));
                            stringBuilder.Append("<" + str4 + ">");
                            issuer1 = identifierIssuer2;
                            if (str2.Length != 0 && string.Compare(stringBuilder.ToString(), str2) > 0)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag && (str2.Length == 0 || string.Compare(stringBuilder.ToString(), str2) < 0))
                        {
                            str2 = stringBuilder.ToString();
                            identifierIssuer1 = issuer1;
                        }
                    }
                }
                messageDigest.Update(str2);
                issuer = identifierIssuer1;
            }
            return (messageDigest.Digest(), issuer);
        }

        private static Dictionary<string, List<string>> CreateHashToRelated(
          string id,
          IdentifierIssuer issuer,
          Dictionary<string, BlankNodeInfo> blankNodeInfo,
          IdentifierIssuer canonicalIssuer)
        {
            Dictionary<string, List<string>> hashToRelated = new Dictionary<string, List<string>>();
            foreach (Quad quad in blankNodeInfo[id].Quads)
            {
                AddRelatedBlankNodeHash(quad, quad.Subject, 's', id, issuer, hashToRelated, blankNodeInfo, canonicalIssuer);
                AddRelatedBlankNodeHash(quad, quad.Object, 'o', id, issuer, hashToRelated, blankNodeInfo, canonicalIssuer);
                AddRelatedBlankNodeHash(quad, quad.Graph, 'g', id, issuer, hashToRelated, blankNodeInfo, canonicalIssuer);
            }
            return hashToRelated;
        }

        private static void AddRelatedBlankNodeHash(
          Quad quad,
          QuadItem component,
          char position,
          string id,
          IdentifierIssuer issuer,
          Dictionary<string, List<string>> hashToRelated,
          Dictionary<string, BlankNodeInfo> blankNodeInfo,
          IdentifierIssuer canonicalIssuer)
        {
            if (component.TermType != TermType.BlankNode || !(component.Value != id))
                return;
            string related = component.Value;
            string key = HashRelatedBlankNode(related, quad, issuer, position, blankNodeInfo, canonicalIssuer);
            List<string> stringList;
            if (hashToRelated.TryGetValue(key, out stringList))
                stringList.Add(related);
            else
                hashToRelated[key] = new List<string>() { related };
        }

        private static string HashRelatedBlankNode(
          string related,
          Quad quad,
          IdentifierIssuer issuer,
          char position,
          Dictionary<string, BlankNodeInfo> blankNodeInfo,
          IdentifierIssuer canonicalIssuer)
        {
            string data = !canonicalIssuer.HasId(related) ? !issuer.HasId(related) ? blankNodeInfo[related].Hash : issuer.GetId(related) : canonicalIssuer.GetId(related);
            MessageDigest messageDigest = new MessageDigest();
            messageDigest.Update(new string(position, 1));
            if (position != 'g')
                messageDigest.Update("<" + quad.Predicate.Value + ">");
            messageDigest.Update(data);
            return messageDigest.Digest();
        }

        private static T UseCanonicalId<T>(T component, IdentifierIssuer canonicalIssuer) where T : QuadItem, new()
        {
            if (component.TermType != TermType.BlankNode || component.Value.StartsWith(canonicalIssuer.Prefix))
                return component;
            T obj = new T();
            obj.TermType = TermType.BlankNode;
            obj.Value = canonicalIssuer.GetId(component.Value);
            return obj;
        }
    }
}
