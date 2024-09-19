// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.RDF
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Data.Custom.JsonLD.Normalization.Exceptions;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal static class RDF
    {
        public static List<Quad> ToRDF(this JToken input)
        {
            List<Quad> dataset = new List<Quad>();
            IdentifierIssuer issuer = new IdentifierIssuer("_:b");
            JObject graphs = new JObject();
            graphs["@default"] = new JObject();
            CreateNodeMap(input, graphs, "@default", issuer);
            foreach (JProperty property in graphs.Properties())
            {
                QuadItem graphTerm = new QuadItem();
                if (property.Name == "@default")
                {
                    graphTerm.TermType = TermType.DefaultGraph;
                    graphTerm.Value = "";
                }
                else if (Utils.IsIriAbsolute(property.Name))
                {
                    graphTerm.TermType = !property.Name.StartsWith("_:") ? TermType.NamedNode : TermType.BlankNode;
                    graphTerm.Value = property.Name;
                }
                else
                    continue;
                if (property.Value.Type == JTokenType.Object)
                    GraphToRDF(dataset, (JObject)property.Value, graphTerm, issuer);
            }
            return dataset;
        }

        private static void CreateNodeMap(
          JToken input,
          JObject graphs,
          string graph,
          IdentifierIssuer issuer,
          string name = null,
          JArray list = null)
        {
            if (input.Type == JTokenType.Array)
            {
                foreach (JToken input1 in input.ToArray())
                    CreateNodeMap(input1, graphs, graph, issuer, list: list);
            }
            else if (input.Type != JTokenType.Object)
            {
                list?.Add(input);
            }
            else
            {
                JObject jobject1 = (JObject)input;
                JToken id1;
                jobject1.TryGetValue("@type", out id1);
                if (jobject1.ContainsKey("@value"))
                {
                    if (id1 != null && id1.Type == JTokenType.String && id1.Value<string>().IndexOf("_:") == 0)
                        jobject1["@type"] = id1 = (JToken)issuer.GetId(id1.Value<string>());
                    list?.Add(input);
                }
                else
                {
                    JToken input2;
                    if (list != null && jobject1.TryGetValue("@list", out input2))
                    {
                        JArray list1 = new JArray();
                        CreateNodeMap(input2, graphs, graph, issuer, name, list1);
                        list.Add(new JObject()
                        {
                            ["@list"] = list1
                        });
                    }
                    else
                    {
                        if (id1 != null && id1.Type == JTokenType.Array)
                        {
                            foreach (JToken jtoken in id1.ToArray())
                            {
                                if (jtoken.Type == JTokenType.String && jtoken.Value<string>().IndexOf("_:") == 0)
                                    issuer.GetId(jtoken.Value<string>());
                            }
                        }
                        if (name == null)
                        {
                            string old = null;
                            JToken jtoken;
                            if (jobject1.TryGetValue("@id", out jtoken) && jtoken.Type == JTokenType.String)
                                old = jtoken.Value<string>();
                            name = Utils.IsBlankNode(input) ? issuer.GetId(old) : old;
                        }
                        if (list != null)
                            list.Add(new JObject()
                            {
                                ["@id"] = (JToken)name
                            });
                        JToken jtoken1;
                        if (!graphs.TryGetValue(graph, out jtoken1) || jtoken1.Type != JTokenType.Object)
                        {
                            jtoken1 = new JObject();
                            graphs[graph] = jtoken1;
                        }
                        JObject jobject2 = (JObject)jtoken1;
                        JToken jtoken2;
                        if (!jobject2.TryGetValue(name, out jtoken2) || jtoken2.Type != JTokenType.Object)
                        {
                            jtoken2 = new JObject();
                            if (name != null)
                                jobject2[name] = jtoken2;
                        }
                        JObject subject1 = (JObject)jtoken2;
                        subject1["@id"] = (JToken)name;
                        foreach (JProperty jproperty in (IEnumerable<JProperty>)jobject1.Properties().OrderBy(p => p.Name, StringComparer.Ordinal))
                        {
                            string str = jproperty.Name;
                            switch (str)
                            {
                                case "@id":
                                    continue;
                                case "@reverse":
                                    JObject jobject3 = new JObject();
                                    jobject3["@id"] = (JToken)name;
                                    JToken jtoken3;
                                    if (jobject1.TryGetValue("@reverse", out jtoken3) && jtoken3.Type == JTokenType.Object)
                                    {
                                        using (IEnumerator<JProperty> enumerator = ((JObject)jtoken3).Properties().GetEnumerator())
                                        {
                                            while (enumerator.MoveNext())
                                            {
                                                JProperty current = enumerator.Current;
                                                if (current.Value.Type == JTokenType.Array)
                                                {
                                                    foreach (JObject jobject4 in current.Value.ToArray().OfType<JObject>())
                                                    {
                                                        string id2 = jobject4["@id"]?.ToString();
                                                        if (Utils.IsBlankNode((JToken)jobject4))
                                                            id2 = issuer.GetId(id2);
                                                        CreateNodeMap(jobject4, graphs, graph, issuer, id2);
                                                        JToken subject2;
                                                        if (!jobject2.TryGetValue(id2, out subject2) || subject2.Type != JTokenType.Object)
                                                            throw new Exceptions.JsonLdParseException("Item \"" + id2 + "\" has to be an object during \"" + graph + "\" graph reversal");
                                                        Utils.AddValue((JObject)subject2, current.Name, (JToken)jobject3, true);
                                                    }
                                                }
                                            }
                                            continue;
                                        }
                                    }
                                    else
                                        continue;
                                case "@graph":
                                    if (name != null && !graphs.ContainsKey(name))
                                        graphs[name] = new JObject();
                                    CreateNodeMap(jproperty.Value, graphs, name, issuer);
                                    continue;
                                case "@included":
                                    CreateNodeMap(jproperty.Value, graphs, graph, issuer);
                                    continue;
                                default:
                                    if (str != "@type" && Utils.IsKeyword(str))
                                    {
                                        JToken jtoken4;
                                        // ISSUE: explicit non-virtual call
                                        // ISSUE: explicit non-virtual call
                                        if (str == "@index" && subject1.TryGetValue(str, out jtoken4) && (jproperty.Value != jtoken4 || (jproperty.Value is JObject jobject5 ? jobject5["@id"] : null) != (jtoken4 is JObject jobject6 ? jobject6["@id"] : null)))
                                            throw new JsonLdParseException("Invalid JSON-LD syntax; conflicting @index property detected.");
                                        subject1[str] = jproperty.Value;
                                        continue;
                                    }
                                    if (jproperty.Value.Type == JTokenType.Array)
                                    {
                                        JToken[] array = jproperty.Value.ToArray();
                                        if (str.IndexOf("_:") == 0)
                                            str = issuer.GetId(str);
                                        if (array.Length == 0)
                                        {
                                            Utils.AddValue(subject1, str, (JToken)new JArray(), true);
                                        }
                                        else
                                        {
                                            foreach (JToken id3 in array)
                                            {
                                                JToken id3Copy = id3; // Copy to allow modifications.

                                                if (str == "@type" && id3Copy.Type == JTokenType.String && id3Copy.Value<string>().StartsWith("_:"))
                                                {
                                                    id3Copy = (JToken)issuer.GetId(id3Copy.Value<string>());
                                                }

                                                if (Utils.IsGraphSubject(id3Copy) || Utils.IsGraphSubjectReference(id3Copy))
                                                {
                                                    if (!((JObject)id3Copy).TryGetValue("@id", out JToken element) ||
                                                        !Utils.IsEmptyObject(element) &&
                                                         (element.Type != JTokenType.String || !string.IsNullOrEmpty(element.Value<string>())))
                                                    {
                                                        string id4 = element?.ToString();
                                                        if (id4 == null || Utils.IsBlankNode(id3Copy))
                                                            id4 = issuer.GetId(id4);

                                                        Utils.AddValue(subject1, str, new JObject
                                                        {
                                                            ["@id"] = id4
                                                        }, true);

                                                        CreateNodeMap(id3Copy, graphs, graph, issuer, id4);
                                                    }
                                                }
                                                else if (Utils.IsGraphValue(id3Copy))
                                                {
                                                    Utils.AddValue(subject1, str, id3Copy, true);
                                                }
                                                else if (Utils.IsGraphList(id3Copy))
                                                {
                                                    JArray list2 = new JArray();
                                                    CreateNodeMap(id3Copy["@list"], graphs, graph, issuer, name, list2);

                                                    Utils.AddValue(subject1, str, new JObject
                                                    {
                                                        ["@list"] = list2
                                                    }, true);
                                                }
                                                else
                                                {
                                                    CreateNodeMap(id3Copy, graphs, graph, issuer, name);
                                                    Utils.AddValue(subject1, str, id3Copy, true);
                                                }
                                            }
                                        }
                                    }
                                    continue;
                            }
                        }
                    }
                }
            }
        }

        private static void GraphToRDF(
          List<Quad> dataset,
          JObject graph,
          QuadItem graphTerm,
          IdentifierIssuer issuer)
        {
            foreach (JProperty jproperty1 in (IEnumerable<JProperty>)graph.Properties().OrderBy(p => p.Name, StringComparer.Ordinal))
            {
                string name = jproperty1.Name;
                JToken jtoken1 = jproperty1.Value;
                if (jtoken1.Type == JTokenType.Object)
                {
                    foreach (JProperty jproperty2 in (IEnumerable<JProperty>)((JObject)jtoken1).Properties().OrderBy(p => p.Name, StringComparer.Ordinal))
                    {
                        string v = jproperty2.Name;
                        JToken source = jproperty2.Value;
                        if (v == "@type")
                            v = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type";
                        else if (Utils.IsKeyword(v))
                            continue;
                        if (source.Type == JTokenType.Array)
                        {
                            foreach (JToken jtoken2 in source.ToArray())
                            {
                                QuadItem quadItem1 = new QuadItem()
                                {
                                    TermType = name.StartsWith("_:") ? TermType.BlankNode : TermType.NamedNode,
                                    Value = name
                                };
                                if (Utils.IsIriAbsolute(name))
                                {
                                    QuadItem quadItem2 = new QuadItem()
                                    {
                                        TermType = v.StartsWith("_:") ? TermType.BlankNode : TermType.NamedNode,
                                        Value = v
                                    };
                                    if (Utils.IsIriAbsolute(v) && quadItem2.TermType != TermType.BlankNode)
                                    {
                                        ObjectQuadItem rdf = ObjectToRDF(jtoken2, issuer, dataset, graphTerm);
                                        if (rdf != null)
                                            dataset.Add(new Quad()
                                            {
                                                Subject = quadItem1,
                                                Predicate = quadItem2,
                                                Object = rdf,
                                                Graph = graphTerm
                                            });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static ObjectQuadItem ObjectToRDF(
          JToken item,
          IdentifierIssuer issuer,
          List<Quad> dataset,
          QuadItem graphTerm,
          string rdfDirection = null)
        {
            ObjectQuadItem objectQuadItem = new ObjectQuadItem();
            if (Utils.IsGraphValue(item))
            {
                objectQuadItem.TermType = TermType.Literal;
                objectQuadItem.Value = null;
                objectQuadItem.DataType = new QuadItem()
                {
                    TermType = TermType.NamedNode
                };
                JToken jtoken1 = item["@value"];
                string str1 = null;
                JToken source;
                if (item.Type == JTokenType.Object && ((JObject)item).TryGetValue("@type", out source))
                    str1 = source.Type != JTokenType.Array ? source.ToString() : string.Join(",", source.ToArray().Select(t => t.ToString()));
                if (str1 == "@json")
                {
                    objectQuadItem.Value = JsonCanonicalize(jtoken1);
                    objectQuadItem.DataType.Value = "http://www.w3.org/1999/02/22-rdf-syntax-ns#JSON";
                }
                else if (jtoken1.Type == JTokenType.Boolean)
                {
                    objectQuadItem.Value = jtoken1.ToString(Formatting.None);
                    objectQuadItem.DataType.Value = str1 ?? "http://www.w3.org/2001/XMLSchema#boolean";
                }
                else if (jtoken1.Type == JTokenType.Float || str1 == "http://www.w3.org/2001/XMLSchema#double")
                {
                    int result;
                    if (str1 != "http://www.w3.org/2001/XMLSchema#double" && int.TryParse(jtoken1.ToString(), out result))
                    {
                        objectQuadItem.Value = result.ToString();
                        objectQuadItem.DataType.Value = str1 ?? "http://www.w3.org/2001/XMLSchema#integer";
                    }
                    else
                    {
                        if (jtoken1.Type != JTokenType.Float)
                            jtoken1 = (JToken)double.Parse(jtoken1.ToString(), CultureInfo.InvariantCulture);
                        objectQuadItem.Value = jtoken1.Value<double>().ToString("0.0##############E-0");
                        objectQuadItem.DataType.Value = str1 ?? "http://www.w3.org/2001/XMLSchema#double";
                    }
                }
                else if (jtoken1.Type == JTokenType.Integer)
                {
                    objectQuadItem.Value = jtoken1.Value<long>().ToString();
                    objectQuadItem.DataType.Value = str1 ?? "http://www.w3.org/2001/XMLSchema#integer";
                }
                else
                {
                    JToken jtoken2;
                    if (rdfDirection == "i18n-datatype" && ((JObject)item).TryGetValue("@direction", out jtoken2))
                    {
                        string str2 = "https://www.w3.org/ns/i18n#" + (item["@language"] ?? (JToken)"")?.ToString() + "_" + jtoken2.ToString();
                        objectQuadItem.DataType.Value = str2;
                        objectQuadItem.Value = jtoken1.ToString();
                    }
                    else
                    {
                        JToken jtoken3;
                        if (((JObject)item).TryGetValue("@language", out jtoken3))
                        {
                            objectQuadItem.Value = jtoken1.ToString();
                            objectQuadItem.DataType.Value = str1 ?? "http://www.w3.org/1999/02/22-rdf-syntax-ns#langString";
                            objectQuadItem.Language = jtoken3.ToString();
                        }
                        else
                        {
                            objectQuadItem.Value = jtoken1.ToString();
                            objectQuadItem.DataType.Value = str1 ?? "http://www.w3.org/2001/XMLSchema#string";
                        }
                    }
                }
            }
            else if (Utils.IsGraphList(item))
            {
                QuadItem rdf = ListToRDF(item["@list"].ToArray().ToList(), issuer, dataset, graphTerm, rdfDirection);
                objectQuadItem.TermType = rdf.TermType;
                objectQuadItem.Value = rdf.Value;
            }
            else
            {
                string str = (item.Type == JTokenType.Object ? item["@id"] : (object)item)?.ToString();
                objectQuadItem.TermType = str == null || !str.StartsWith("_:") ? TermType.NamedNode : TermType.BlankNode;
                objectQuadItem.Value = str;
            }
            return objectQuadItem.TermType == TermType.NamedNode && !Utils.IsIriAbsolute(objectQuadItem.Value) ? null : objectQuadItem;
        }

        private static string JsonCanonicalize(JToken value)
        {
            if (value != null && value.Type == JTokenType.Array)
                return "[" + value.ToArray().Aggregate("", (acc, val) =>
                {
                    string str = acc.Length == 0 ? "" : ",";
                    if (val != null && val.Type == JTokenType.Undefined)
                        val = null;
                    return acc + str + JsonCanonicalize(val);
                }) + "]";
            if (value != null && value.Type == JTokenType.Object)
                return "{" + ((JObject)value).Properties().OrderBy(p => p.Name, StringComparer.Ordinal).Aggregate("", (acc, prop) =>
                {
                    JToken jtoken = prop.Value;
                    if (jtoken != null && jtoken.Type == JTokenType.Undefined)
                        return acc;
                    string str = acc.Length == 0 ? "" : ",";
                    return acc + str + JsonCanonicalize((JToken)prop.Name) + ":" + JsonCanonicalize(prop.Value);
                }) + "}";
            if ((new JTokenType?[3]
            {
        new JTokenType?(JTokenType.Boolean),
        new JTokenType?(JTokenType.String),
        new JTokenType?(JTokenType.Null)
            }).Contains(value?.Type))
                return value.ToString(Formatting.None);
            return value != null && value.Type == JTokenType.Float ? value.ToString().ToLower() : value?.ToString() ?? "";
        }

        private static QuadItem ListToRDF(
          List<JToken> list,
          IdentifierIssuer issuer,
          List<Quad> dataset,
          QuadItem graphTerm,
          string rdfDirection)
        {
            QuadItem quadItem1 = new QuadItem()
            {
                TermType = TermType.NamedNode,
                Value = "http://www.w3.org/1999/02/22-rdf-syntax-ns#first"
            };
            QuadItem quadItem2 = new QuadItem()
            {
                TermType = TermType.NamedNode,
                Value = "http://www.w3.org/1999/02/22-rdf-syntax-ns#rest"
            };
            QuadItem quadItem3 = new QuadItem()
            {
                TermType = TermType.NamedNode,
                Value = "http://www.w3.org/1999/02/22-rdf-syntax-ns#nil"
            };
            JToken jtoken1 = list.LastOrDefault();
            if (jtoken1 != null)
                list.RemoveAt(list.Count - 1);
            QuadItem quadItem4;
            if (jtoken1 == null)
            {
                quadItem4 = quadItem3;
            }
            else
            {
                quadItem4 = new QuadItem();
                quadItem4.TermType = TermType.BlankNode;
                quadItem4.Value = issuer.GetId();
            }
            QuadItem rdf1 = quadItem4;
            QuadItem quadItem5 = rdf1;
            foreach (JToken jtoken2 in list)
            {
                ObjectQuadItem rdf2 = ObjectToRDF(jtoken2, issuer, dataset, graphTerm, rdfDirection);
                QuadItem quadItem6 = new QuadItem()
                {
                    TermType = TermType.BlankNode,
                    Value = issuer.GetId()
                };
                dataset.Add(new Quad()
                {
                    Subject = quadItem5,
                    Predicate = quadItem1,
                    Object = rdf2,
                    Graph = graphTerm
                });
                List<Quad> quadList = dataset;
                Quad quad = new Quad();
                quad.Subject = quadItem5;
                quad.Predicate = quadItem2;
                ObjectQuadItem objectQuadItem = new ObjectQuadItem();
                objectQuadItem.TermType = quadItem6.TermType;
                objectQuadItem.Value = quadItem6.Value;
                quad.Object = objectQuadItem;
                quad.Graph = graphTerm;
                quadList.Add(quad);
                quadItem5 = quadItem6;
            }
            if (jtoken1 != null)
            {
                ObjectQuadItem rdf3 = ObjectToRDF(jtoken1, issuer, dataset, graphTerm, rdfDirection);
                dataset.Add(new Quad()
                {
                    Subject = quadItem5,
                    Predicate = quadItem1,
                    Object = rdf3,
                    Graph = graphTerm
                });
                List<Quad> quadList = dataset;
                Quad quad = new Quad();
                quad.Subject = quadItem5;
                quad.Predicate = quadItem2;
                ObjectQuadItem objectQuadItem = new ObjectQuadItem();
                objectQuadItem.TermType = quadItem3.TermType;
                objectQuadItem.Value = quadItem3.Value;
                quad.Object = objectQuadItem;
                quad.Graph = graphTerm;
                quadList.Add(quad);
            }
            return rdf1;
        }
    }
}
