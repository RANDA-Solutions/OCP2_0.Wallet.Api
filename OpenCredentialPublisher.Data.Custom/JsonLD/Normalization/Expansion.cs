// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Expansion
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Data.Custom.JsonLD.Normalization.Exceptions;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal class Expansion
    {
        public static async
#nullable disable
        Task<JToken> Expand(
          ExpandContext activeCtx,
          JToken element,
          string activeProperty = null,
          ExpandOptions options = null,
          bool insideList = false,
          bool insideIndex = false,
          ExpandContext typeScopedContext = null,
          Func<object, JToken> expansionMap = null)
        {
            if (Utils.IsEmptyObject(element))
                return null;
            if (element.Type != JTokenType.Array && element.Type != JTokenType.Object)
            {
                if (insideList || activeProperty != null && !(ExpandIri(activeCtx, activeProperty, IriRelativeTo.VocabSet, options: options) == "@graph"))
                    return ExpandValue(activeCtx, element, activeProperty, options);
                Func<object, JToken> func = expansionMap;
                return func != null ? func(new
                {
                    unmappedValue = element,
                    activeCtx,
                    activeProperty,
                    options,
                    insideList
                }) : null;
            }
            if (element.Type == JTokenType.Array)
            {
                JArray rvalArr = new JArray();
                JToken containerToken = GetContextValue(activeCtx, activeProperty, "@container");
                JToken jtoken1 = containerToken;
                JToken[] container = (jtoken1 != null ? jtoken1.Type == JTokenType.Array ? 1 : 0 : 0) != 0 ? containerToken.ToArray() : new JToken[0];
                insideList = insideList || container.Any(c => c.Type == JTokenType.String && c.Value<string>() == "@list");
                JToken[] elementArray = element.ToArray();
                for (int i = 0; i < elementArray.Length; ++i)
                {
                    JToken e = await Expand(activeCtx, elementArray[i], activeProperty, options, insideIndex: insideIndex, typeScopedContext: typeScopedContext, expansionMap: expansionMap);
                    int num;
                    if (insideList)
                    {
                        JToken jtoken2 = e;
                        num = jtoken2 != null ? jtoken2.Type == JTokenType.Array ? 1 : 0 : 0;
                    }
                    else
                        num = 0;
                    if (num != 0)
                    {
                        JObject eObj = new JObject();
                        eObj["@list"] = e;
                        e = eObj;
                        eObj = null;
                    }
                    if (e == null)
                    {
                        Func<object, JToken> func = expansionMap;
                        e = func != null ? func(new
                        {
                            unmappedValue = elementArray[i],
                            activeCtx,
                            activeProperty,
                            parent = element,
                            index = i,
                            options,
                            expandedParent = rvalArr,
                            insideList
                        }) : null;
                        if (e == null)
                            continue;
                    }
                    if (e.Type == JTokenType.Array)
                    {
                        JToken[] jtokenArray = e.ToArray();
                        for (int index = 0; index < jtokenArray.Length; ++index)
                        {
                            JToken eElem = jtokenArray[index];
                            rvalArr.Add(eElem);
                            eElem = null;
                        }
                        jtokenArray = null;
                    }
                    else
                        rvalArr.Add(e);
                    e = null;
                }
                return rvalArr;
            }
            JObject elemObj = (JObject)element;
            string expandedActiveProperty = ExpandIri(activeCtx, activeProperty, IriRelativeTo.VocabSet, options: options);
            JToken propertyScopedCtx = GetContextValue(activeCtx, activeProperty, "@context");
            if (typeScopedContext == null)
                typeScopedContext = activeCtx.PreviousContext != null ? activeCtx : null;
            List<JProperty> properties = elemObj.Properties().OrderBy(p => p.Name).ToList();
            bool mustRevert = !insideIndex;
            if (mustRevert && typeScopedContext != null && properties.Count <= 2 && !properties.Any(p => p.Name == "@context"))
            {
                foreach (JProperty jproperty in properties)
                {
                    JProperty prop = jproperty;
                    string expandedProperty = ExpandIri(typeScopedContext, prop.Name, IriRelativeTo.VocabSet, options: options);
                    if (expandedProperty == "@value")
                    {
                        mustRevert = false;
                        activeCtx = typeScopedContext;
                        break;
                    }
                    if (expandedProperty == "@id" && properties.Count == 1)
                    {
                        mustRevert = false;
                        break;
                    }
                    expandedProperty = null;
                    prop = null;
                }
            }
            if (mustRevert)
                activeCtx = activeCtx.RevertToPreviousContext();
            if (propertyScopedCtx != null)
            {
                ExpandContext expandContext = await ProcessContext(activeCtx, propertyScopedCtx, true, true, options);
                activeCtx = expandContext;
                expandContext = null;
            }
            JToken elemContext;
            if (elemObj.TryGetValue("@context", out elemContext))
            {
                ExpandContext expandContext = await ProcessContext(activeCtx, elemContext, true, false, options);
                activeCtx = expandContext;
                expandContext = null;
            }
            typeScopedContext = activeCtx;
            string typeKey = null;
            foreach (JProperty prop in properties)
            {
                string expandedProperty = ExpandIri(activeCtx, prop.Name, IriRelativeTo.VocabSet, options: options);
                if (expandedProperty == "@type")
                {
                    typeKey = typeKey ?? prop.Name;
                    JToken value = elemObj[prop.Name];
                    JArray types;
                    if (value.Type == JTokenType.Array)
                    {
                        JToken[] valueArr = value.ToArray();
                        types = valueArr.Length <= 1 ? (JArray)value : new JArray((object[])valueArr.OrderBy(e => e.ToString()).ToArray());
                        valueArr = null;
                    }
                    else
                        types = new JArray(value);
                    foreach (JToken type in types)
                    {
                        JToken ctx = GetContextValue(typeScopedContext, type.ToString(), "@context");
                        if (ctx != null)
                        {
                            ExpandContext expandContext = await ProcessContext(activeCtx, ctx, false, false, options);
                            activeCtx = expandContext;
                            expandContext = null;
                        }
                        ctx = null;
                    }
                    value = null;
                    types = null;
                }
                expandedProperty = null;
            }
            JObject rval = new JObject();
            await ExpandObject(activeCtx, activeProperty, expandedActiveProperty, elemObj, rval, options, insideList, typeKey, typeScopedContext, expansionMap);
            int count = rval.Properties().Count();
            JToken valueProp;
            if (rval.TryGetValue("@value", out valueProp))
            {
                if (rval.ContainsKey("@type") && (rval.ContainsKey("@language") || rval.ContainsKey("@direction")))
                    throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; an element containing \"@value\" may not contain both \"@type\" and either \"@language\" or \"@direction\".");
                int validCount = count - 1;
                if (rval.ContainsKey("@type"))
                    --validCount;
                if (rval.ContainsKey("@index"))
                    --validCount;
                if (rval.ContainsKey("@language"))
                    --validCount;
                if (rval.ContainsKey("@direction"))
                    --validCount;
                if (validCount != 0)
                    throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; an element containing \"@value\" may only have an \"@index\" property and either \"@type\" or either or both \"@language\" or \"@direction\".");
                List<JToken> values = new List<JToken>();
                JToken jtoken3 = valueProp;
                if (jtoken3 != null && jtoken3.Type == JTokenType.Array)
                    values.AddRange(valueProp.ToArray());
                else if (!Utils.IsEmptyObject(valueProp))
                    values.Add(valueProp);
                JToken typeProp;
                rval.TryGetValue("@type", out typeProp);
                List<JToken> types = new List<JToken>();
                JToken jtoken4 = typeProp;
                if (jtoken4 != null && jtoken4.Type == JTokenType.Array)
                    types.AddRange(typeProp.ToArray());
                else if (!Utils.IsEmptyObject(typeProp))
                    types.Add(typeProp);
                if (!types.Contains((JToken)"@json") || types.Count != 1)
                {
                    if (values.Count == 0)
                    {
                        Func<object, JToken> func = expansionMap;
                        JToken mapped = func != null ? func(new
                        {
                            unmappedValue = rval,
                            activeCtx,
                            activeProperty,
                            element,
                            options,
                            insideList
                        }) : null;
                        rval = mapped as JObject;
                        mapped = null;
                    }
                    else
                    {
                        if (!values.All((Func<JToken, bool>)(v => v.Type == JTokenType.String || Utils.IsEmptyObject(v))) && rval.ContainsKey("@language"))
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; only strings may be language-tagged.");
                        if (!types.All((Func<JToken, bool>)(t =>
                        {
                            if (Utils.IsEmptyObject(t))
                                return true;
                            if (t.Type != JTokenType.String)
                                return false;
                            return Utils.IsIriAbsolute(t.Value<string>()) || t.Value<string>().IndexOf("_:") != 0;
                        })))
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; an element containing \"@value\" and \"@type\" must have an absolute IRI for the value of \"@type\".");
                    }
                }
                values = null;
                typeProp = null;
                types = null;
            }
            else
            {
                JToken typeProp;
                if (rval.TryGetValue("@type", out typeProp) && typeProp.Type != JTokenType.Array)
                    rval["@type"] = new JArray(typeProp);
                else if (rval.ContainsKey("@set") || rval.ContainsKey("@list"))
                {
                    if (count > 1 && (count != 2 || !rval.ContainsKey("@index")))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; if an element has the property \"@set\" or \"@list\", then it can have at most one other property that is \"@index\".");
                    JToken setProp;
                    if (rval.TryGetValue("@set", out setProp))
                    {
                        if (setProp.Type != JTokenType.Object)
                            return setProp;
                        rval = (JObject)setProp;
                        count = rval.Properties().Count();
                    }
                    setProp = null;
                }
                else if (count == 1 && rval.ContainsKey("@language"))
                {
                    Func<object, JToken> func = expansionMap;
                    JToken mapped = func != null ? func(new
                    {
                        unmappedValue = rval,
                        activeCtx,
                        activeProperty,
                        element,
                        options,
                        insideList
                    }) : null;
                    rval = mapped as JObject;
                    mapped = null;
                }
                typeProp = null;
            }
            int num1;
            if (rval != null)
            {
                ExpandOptions expandOptions = options;
                if ((expandOptions != null ? expandOptions.KeepFreeFloatingNodes ? 1 : 0 : 0) == 0 && !insideList)
                {
                    num1 = activeProperty == null ? 1 : expandedActiveProperty == "@graph" ? 1 : 0;
                    goto label_110;
                }
            }
            num1 = 0;
        label_110:
            if (num1 != 0 && (count == 0 || rval.ContainsKey("@value") || rval.ContainsKey("@list") || count == 1 && rval.ContainsKey("@id")))
            {
                Func<object, JToken> func = expansionMap;
                JToken mapped = func != null ? func(new
                {
                    unmappedValue = rval,
                    activeCtx,
                    activeProperty,
                    element,
                    options,
                    insideList
                }) : null;
                rval = mapped as JObject;
                mapped = null;
            }
            return rval;
        }

        private static async Task<ExpandContext> ProcessContext(
          ExpandContext activeCtx,
          JToken localCtx,
          bool propagate,
          bool overrideProtected,
          ExpandOptions options,
          HashSet<object> cycles = null)
        {
            if (cycles == null)
                cycles = new HashSet<object>();
            JToken[] ctxs = Utils.NormalizeContext(localCtx);
            if (!ctxs.Any())
                return activeCtx;
            string baseUrl = options.Base;
            IContextResolver contextResolver = options.ContextResolver;
            List<ResolvedContext> resolved = await contextResolver.Resolve(activeCtx, localCtx, baseUrl);
            ResolvedContext resolvedContext1 = resolved.FirstOrDefault();
            int num;
            if (resolvedContext1 == null)
            {
                num = 0;
            }
            else
            {
                JTokenType? type = resolvedContext1.Document?.Type;
                JTokenType jtokenType = JTokenType.Object;
                num = type.GetValueOrDefault() == jtokenType & type.HasValue ? 1 : 0;
            }
            JToken propagateToken;
            if (num != 0 && ((JObject)resolved[0].Document).TryGetValue("@propagate", out propagateToken) && propagateToken.Type == JTokenType.Boolean)
                propagate = propagateToken.Value<bool>();
            ExpandContext rval = activeCtx;
            if (!propagate && rval.PreviousContext == null)
            {
                rval = rval.CloneActiveContext();
                rval.PreviousContext = activeCtx;
            }
            foreach (ResolvedContext resolvedContext2 in resolved)
            {
                ResolvedContext resolvedContext = resolvedContext2;
                JToken ctx = resolvedContext.Document;
                activeCtx = rval;
                if (ctx == null)
                {
                    if (!overrideProtected && activeCtx.Protected.Any())
                    {
                        string protectedMode = options.ProtectedMode ?? "error";
                        switch (protectedMode)
                        {
                            case "error":
                                throw new Exceptions.JsonLdParseException("Tried to nullify a context with protected terms outside of a term definition.");
                            case "warn":
                                ExpandContext processedInWarnProtectedMode = resolvedContext.GetProcessed(activeCtx);
                                if (processedInWarnProtectedMode != null)
                                {
                                    rval = activeCtx = processedInWarnProtectedMode;
                                    continue;
                                }
                                ExpandContext oldActiveCtx = activeCtx;
                                rval = activeCtx = new ExpandContext();
                                foreach ((string key, bool flag) in oldActiveCtx.Protected)
                                {
                                    if (flag)
                                    {
                                        JObject protectedVal;
                                        if (oldActiveCtx.Mappings.TryGetValue(key, out protectedVal))
                                            activeCtx.Mappings[key] = (JObject)protectedVal.DeepClone();
                                        activeCtx.Protected[key] = flag;
                                        protectedVal = null;
                                    }
                                }
                                resolvedContext.SetProcessed(oldActiveCtx, rval);
                                continue;
                            default:
                                throw new Exceptions.JsonLdParseException("Invalid protectedMode.");
                        }
                    }
                    else
                        rval = activeCtx = new ExpandContext();
                }
                else
                {
                    ExpandContext processed = resolvedContext.GetProcessed(activeCtx);
                    if (processed != null)
                    {
                        rval = activeCtx = processed;
                    }
                    else
                    {
                        JToken ctxContext;
                        if (ctx.Type == JTokenType.Object && ((JObject)ctx).TryGetValue("@context", out ctxContext))
                            ctx = ctxContext;
                        if (ctx.Type != JTokenType.Object)
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context must be an object.");
                        rval = rval.CloneActiveContext();
                        Dictionary<string, bool> defined = new Dictionary<string, bool>();
                        JObject ctxObj = (JObject)ctx;
                        JToken versionProp;
                        if (ctxObj.TryGetValue("@version", out versionProp))
                        {
                            rval.Fields["@version"] = !(versionProp.ToString() != "1.1") ? versionProp : throw new Exceptions.JsonLdParseException(string.Format("Unsupported JSON-LD version: {0}", versionProp));
                            defined["@version"] = true;
                        }
                        JToken baseProp;
                        if (ctxObj.TryGetValue("@base", out baseProp))
                        {
                            if (!Utils.IsEmptyObject(baseProp))
                            {
                                string baseVal = baseProp.Type == JTokenType.String ? baseProp.Value<string>() : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; the value of \"@base\" in a @context must be an absolute IRI, a relative IRI, or null.");
                                if (!Utils.IsIriAbsolute(baseVal))
                                {
                                    string rvalBaseVal = "";
                                    JToken rvalBase;
                                    if (rval.Fields.TryGetValue("@base", out rvalBase) && rvalBase.Type == JTokenType.String)
                                        rvalBaseVal = rvalBase.Value<string>();
                                    baseProp = (JToken)Utils.PrependBase(rvalBaseVal, baseVal);
                                    rvalBaseVal = null;
                                    rvalBase = null;
                                }
                                baseVal = null;
                            }
                            rval.Fields["@base"] = baseProp;
                            defined["@base"] = true;
                        }
                        JToken vocabProp;
                        if (ctxObj.TryGetValue("@vocab", out vocabProp))
                        {
                            if (Utils.IsEmptyObject(vocabProp))
                            {
                                rval.Fields.Remove("@vocab");
                            }
                            else
                            {
                                string vocabVal = vocabProp.Type == JTokenType.String ? vocabProp.Value<string>() : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; the value of \"@vocab\" in a @context must be a string or null.");
                                rval.Fields["@vocab"] = (JToken)ExpandIri(rval, vocabVal, IriRelativeTo.BothSet, options: options);
                                vocabVal = null;
                            }
                            defined["@vocab"] = true;
                        }
                        JToken languageProp;
                        if (ctxObj.TryGetValue("@language", out languageProp))
                        {
                            if (Utils.IsEmptyObject(languageProp))
                                rval.Fields.Remove("@language");
                            else
                                rval.Fields["@language"] = languageProp.Type == JTokenType.String ? (JToken)languageProp.Value<string>().ToLower() : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; the value of \"@language\" in a @context must be a string or null.");
                            defined["@language"] = true;
                        }
                        JToken directionProp;
                        if (ctxObj.TryGetValue("@direction", out directionProp))
                        {
                            if (Utils.IsEmptyObject(directionProp))
                            {
                                rval.Fields.Remove("@direction");
                            }
                            else
                            {
                                string dirVal = directionProp.ToString();
                                rval.Fields["@direction"] = !(dirVal != "ltr") || !(dirVal != "rtl") ? (JToken)dirVal : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; the value of \"@direction\" in a @context must be null, \"ltr\", or \"rtl\".");
                                dirVal = null;
                            }
                            defined["@direction"] = true;
                        }
                        JToken propagateProp;
                        if (ctxObj.TryGetValue("@propagate", out propagateProp))
                        {
                            if (propagateProp.Type != JTokenType.Boolean)
                                throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @propagate value must be a boolean.");
                            defined["@propagate"] = true;
                        }
                        JToken importProp;
                        if (ctxObj.TryGetValue("@import", out importProp))
                        {
                            if (importProp.Type != JTokenType.String)
                                throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @import must be a string.");
                            List<ResolvedContext> resolvedImport = await contextResolver.Resolve(activeCtx, (JToken)importProp.Value<string>(), baseUrl);
                            ExpandContext processedImport = resolvedImport.Count == 1 ? resolvedImport[0].GetProcessed(activeCtx) : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @import must reference a single context.");
                            if (processedImport != null)
                            {
                                JObject newCtx = new JObject();
                                foreach (KeyValuePair<string, JToken> field1 in processedImport.Fields)
                                {
                                    KeyValuePair<string, JToken> field = field1;
                                    newCtx[field.Key] = field.Value;
                                    field = new KeyValuePair<string, JToken>();
                                }
                                ctxObj = newCtx;
                                newCtx = null;
                            }
                            else
                            {
                                JToken importCtx = resolvedImport[0].Document;
                                if (importCtx.Type == JTokenType.Object)
                                {
                                    JObject importCtxObj = (JObject)importCtx;
                                    if (importCtxObj.ContainsKey("@import"))
                                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax: imported context must not include @import.");
                                    processedImport = activeCtx.CloneActiveContext();
                                    foreach (JProperty prop in importCtxObj.Properties())
                                    {
                                        if (!ctxObj.ContainsKey(prop.Name))
                                        {
                                            ctxObj[prop.Name] = prop.Value;
                                            processedImport.Fields[prop.Name] = prop.Value;
                                        }
                                    }
                                    importCtxObj = null;
                                }
                                resolvedImport[0].SetProcessed(activeCtx, processedImport);
                                importCtx = null;
                            }
                            defined["@import"] = true;
                            resolvedImport = null;
                            processedImport = null;
                        }
                        JToken protectedProp;
                        defined["@protected"] = ctxObj.TryGetValue("@protected", out protectedProp) && protectedProp.Type == JTokenType.Boolean && protectedProp.Value<bool>();
                        foreach (JProperty prop in ctxObj.Properties())
                        {
                            CreateTermDefinition(rval, ctxObj, prop.Name, defined, options, overrideProtected);
                            JToken propCtx;
                            if (prop.Value.Type == JTokenType.Object && ((JObject)prop.Value).TryGetValue("@context", out propCtx))
                            {
                                bool process = true;
                                if (propCtx.Type == JTokenType.String)
                                {
                                    string url = Utils.PrependBase(baseUrl, propCtx.Value<string>());
                                    if (cycles.Contains(url))
                                        process = false;
                                    else
                                        cycles.Add(url);
                                    url = null;
                                }
                                if (process)
                                {
                                    try
                                    {
                                        ExpandContext expandContext = await ProcessContext(rval.CloneActiveContext(), propCtx, true, true, options, cycles);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; invalid scoped context.", ex);
                                    }
                                }
                            }
                            propCtx = null;
                        }
                        resolvedContext.SetProcessed(activeCtx, rval);
                        ctx = null;
                        processed = null;
                        ctxContext = null;
                        defined = null;
                        ctxObj = null;
                        versionProp = null;
                        baseProp = null;
                        vocabProp = null;
                        languageProp = null;
                        directionProp = null;
                        propagateProp = null;
                        importProp = null;
                        protectedProp = null;
                        resolvedContext = null;
                    }
                }
            }
            return rval;
        }

        private static void CreateTermDefinition(
          ExpandContext activeCtx,
          JObject localCtx,
          string term,
          Dictionary<string, bool> defined,
          ExpandOptions options,
          bool overrideProtected = false)
        {
            if (string.IsNullOrEmpty(term))
                throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; a term cannot be an empty string.");
            bool flag1;
            if (defined.TryGetValue(term, out flag1))
            {
                if (!flag1)
                    throw new Exceptions.JsonLdParseException("Cyclical context definition detected.");
            }
            else
            {
                defined[term] = false;
                JToken jtoken1;
                localCtx.TryGetValue(term, out jtoken1);
                JToken jtoken2;
                if (term == "@type" && jtoken1 != null && jtoken1.Type == JTokenType.Object && (!((JObject)jtoken1).TryGetValue("@container", out jtoken2) || jtoken2.ToString() == "@set"))
                {
                    string[] validKeywords = new string[3]
                    {
            "@container",
            "@id",
            "@protected"
                    };
                    IEnumerable<JProperty> source = ((JObject)jtoken1).Properties();
                    if (!source.Any() || source.Any(p => !validKeywords.Contains(p.Name)))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; keywords cannot be overridden.");
                }
                else
                {
                    if (Utils.IsKeyword(term))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; keywords cannot be overridden.");
                    if (Utils.IsIriKeyword(term))
                        return;
                }
                JObject t1;
                if (activeCtx.Mappings.TryGetValue(term, out t1))
                    activeCtx.Mappings.Remove(term);
                bool flag2 = false;
                if (jtoken1 == null || Utils.IsEmptyObject(jtoken1) || jtoken1.Type == JTokenType.String)
                {
                    flag2 = true;
                    jtoken1 = new JObject(new JProperty("@id", jtoken1));
                }
                JObject jobject1 = jtoken1.Type == JTokenType.Object ? (JObject)jtoken1 : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context term values must be strings or objects.");
                JObject t2 = new JObject();
                activeCtx.Mappings[term] = t2;
                t2["reverse"] = (JToken)false;
                List<string> stringList = new List<string>()
        {
          "@container",
          "@id",
          "@language",
          "@reverse",
          "@type"
        };
                stringList.AddRange(new string[6]
                {
          "@context",
          "@direction",
          "@index",
          "@nest",
          "@prefix",
          "@protected"
                });
                foreach (JProperty property in jobject1.Properties())
                {
                    if (!stringList.Contains(property.Name))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; a term definition must not contain " + property.Name);
                }
                int length = term.IndexOf(':');
                t2["_termHasColon"] = (JToken)(length > 0);
                JToken jtoken3;
                if (jobject1.TryGetValue("@reverse", out jtoken3))
                {
                    if (jobject1.ContainsKey("@id"))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; a @reverse term definition must not contain @id.");
                    if (jobject1.ContainsKey("@nest"))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; a @reverse term definition must not contain @nest.");
                    string v = jtoken3.Type == JTokenType.String ? jtoken3.Value<string>() : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; a @context @reverse value must be a string.");
                    if (!Utils.IsKeyword(v) && Utils.IsIriKeyword(v))
                    {
                        if (t1 != null)
                        {
                            activeCtx.Mappings[term] = t1;
                            return;
                        }
                        activeCtx.Mappings.Remove(term);
                        return;
                    }
                    string str = ExpandIri(activeCtx, v, IriRelativeTo.VocabSet, localCtx, defined, options);
                    t2["@id"] = Utils.IsIriAbsolute(str) ? (JToken)str : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; a @context @reverse value must be an absolute IRI or a blank node identifier.");
                    t2["reverse"] = (JToken)true;
                }
                else
                {
                    JToken element;
                    if (jobject1.TryGetValue("@id", out element))
                    {
                        if (Utils.IsEmptyObject(element))
                        {
                            t2["@id"] = null;
                        }
                        else
                        {
                            string v = element.Type == JTokenType.String ? element.Value<string>() : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; a @context @id value must be an array of strings or a string.");
                            if (!Utils.IsKeyword(v) && Utils.IsIriKeyword(v))
                            {
                                if (t1 != null)
                                {
                                    activeCtx.Mappings[term] = t1;
                                    return;
                                }
                                activeCtx.Mappings.Remove(term);
                                return;
                            }
                            if (v != term)
                            {
                                string str = ExpandIri(activeCtx, v, IriRelativeTo.VocabSet, localCtx, defined, options);
                                if (!Utils.IsIriAbsolute(str) && !Utils.IsKeyword(str))
                                    throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; a @context @id value must be an absolute IRI, a blank node identifier, or a keyword.");
                                if (Regex.IsMatch(term, "(?::[^:])|\\/"))
                                {
                                    Dictionary<string, bool> dictionary = defined.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                                    dictionary[term] = true;
                                    if (ExpandIri(activeCtx, term, IriRelativeTo.VocabSet, localCtx, dictionary, options) != str)
                                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; term in form of IRI must expand to definition.");
                                }
                                t2["@id"] = (JToken)str;
                                JToken jtoken4;
                                t2["_prefix"] = (JToken)(flag2 && (!t2.TryGetValue("_termHasColon", out jtoken4) || jtoken4.Type != JTokenType.Boolean || !jtoken4.Value<bool>()) && Regex.IsMatch(str, "[:\\/\\?#\\[\\]@]$"));
                            }
                        }
                    }
                }
                if (!t2.ContainsKey("@id"))
                {
                    JToken jtoken5;
                    if (t2.TryGetValue("_termHasColon", out jtoken5) && jtoken5.Value<bool>())
                    {
                        string str1 = term.Substring(0, length);
                        if (localCtx.ContainsKey(str1))
                            CreateTermDefinition(activeCtx, localCtx, str1, defined, options);
                        JObject jobject2;
                        if (activeCtx.Mappings.TryGetValue(str1, out jobject2) && jobject2.Type == JTokenType.Object)
                        {
                            string str2 = term.Substring(length + 1);
                            string str3 = "";
                            JToken jtoken6;
                            if (jobject2.TryGetValue("@id", out jtoken6) && jtoken6.Type == JTokenType.String)
                                str3 = jtoken6.Value<string>();
                            t2["@id"] = (JToken)(str3 + str2);
                        }
                        else
                            t2["@id"] = (JToken)term;
                    }
                    else if (term == "@type")
                    {
                        t2["@id"] = (JToken)term;
                    }
                    else
                    {
                        JToken jtoken7;
                        if (!activeCtx.Fields.TryGetValue("@vocab", out jtoken7) || jtoken7.Type != JTokenType.String)
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context terms must define an @id.");
                        t2["@id"] = (JToken)(jtoken7.Value<string>() + term);
                    }
                }
                JToken jtoken8;
                t2.TryGetValue("@id", out jtoken8);
                JToken jtoken9;
                bool flag3 = jobject1.TryGetValue("@protected", out jtoken9);
                bool flag4 = false;
                if (flag3 && jtoken9.Type == JTokenType.Boolean && jtoken9.Value<bool>() || ((flag3 ? 0 : defined.TryGetValue("@protected", out flag4) ? 1 : 0) & (flag4 ? 1 : 0)) != 0)
                {
                    activeCtx.Protected[term] = true;
                    t2["protected"] = (JToken)true;
                }
                defined[term] = true;
                JToken jtoken10;
                if (jobject1.TryGetValue("@type", out jtoken10))
                {
                    string str = jtoken10.Type == JTokenType.String ? jtoken10.Value<string>() : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; an @context @type value must be a string.");
                    if (!(str == "@json") && !(str == "@none") && str != "@id" && str != "@vocab")
                    {
                        str = ExpandIri(activeCtx, str, IriRelativeTo.VocabSet, localCtx, defined, options);
                        if (!Utils.IsIriAbsolute(str))
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; an @context @type value must be an absolute IRI.");
                        if (str.IndexOf("_:") == 0)
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; an @context @type value must be an IRI, not a blank node identifier.");
                    }
                    t2["@type"] = (JToken)str;
                }
                JToken source1;
                if (jobject1.TryGetValue("@container", out source1))
                {
                    List<string> source2 = new List<string>();
                    if (source1.Type == JTokenType.String)
                        source2.Add(source1.Value<string>());
                    else if (source1.Type == JTokenType.Array)
                        source2.AddRange(source1.ToArray().Where(c => c.Type == JTokenType.String).Select(c => c.Value<string>()));
                    List<string> validContainers = new List<string>()
          {
            "@list",
            "@set",
            "@index",
            "@language"
          };
                    bool flag5 = true;
                    bool flag6 = source2.Contains("@set");
                    validContainers.AddRange(new string[3]
                    {
            "@graph",
            "@id",
            "@type"
                    });
                    if (source2.Contains("@list"))
                    {
                        if (source2.Count != 1)
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context @container with @list must have no other values");
                    }
                    else if (source2.Contains("@graph"))
                    {
                        if (source2.Any(key => key != "@graph" && key != "@id" && key != "@index" && key != "@set"))
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context @container with @graph must have no other values other than @id, @index, and @set");
                    }
                    else
                        flag5 &= source2.Count <= (flag6 ? 2 : 1);
                    if (source2.Contains("@type"))
                    {
                        JToken jtoken11;
                        if (!t2.TryGetValue("@type", out jtoken11) || string.IsNullOrEmpty(jtoken11?.ToString()))
                            t2["@type"] = (JToken)"@id";
                        if (!(new string[2]
                        {
              "@id",
              "@vocab"
                        }).Contains(t2["@type"].ToString()))
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; container: @type requires @type to be @id or @vocab.");
                    }
                    if (((flag5 & source2.All(c => validContainers.Contains(c)) ? 1 : 0) & (!flag6 ? 1 : !source2.Contains("@list") ? 1 : 0)) == 0)
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context @container value must be one of the following: " + string.Join(", ", (IEnumerable<string>)validContainers));
                    JToken jtoken12;
                    if (t2.TryGetValue("reverse", out jtoken12) && jtoken12.Type == JTokenType.Boolean && jtoken12.Value<bool>() && !source2.All(c => (new string[2]
                    {
            "@index",
            "@set"
                    }).Contains(c)))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context @container value for a @reverse type definition must be @index or @set.");
                    t2["@container"] = new JArray((object[])source2.ToArray());
                }
                JToken jtoken13;
                if (jobject1.TryGetValue("@index", out jtoken13))
                {
                    if (jtoken13.Type != JTokenType.String || jtoken13.Value<string>().IndexOf('@') == 0)
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @index must expand to an IRI: \"" + jtoken13.Value<string>() + "\" on term \"" + term + "\".");
                    JToken source3;
                    if (!jobject1.ContainsKey("@container") || !t2.TryGetValue("@container", out source3) || source3.Type != JTokenType.Array || !source3.ToArray().Any(t => JToken.EqualityComparer.Equals(t, (JToken)"@index")))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @index without @index in @container: \"" + jtoken13.Value<string>() + "\" on term \"" + term + "\".");
                    t2["@index"] = (JToken)jtoken13.Value<string>();
                }
                JToken jtoken14;
                if (jobject1.TryGetValue("@context", out jtoken14))
                    t2["@context"] = jtoken14;
                JToken lower;
                if (jobject1.TryGetValue("@language", out lower) && !jobject1.ContainsKey("@type"))
                {
                    if (!Utils.IsEmptyObject(lower) && lower.Type != JTokenType.String)
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context @language value must be a string or null.");
                    if (lower.Type == JTokenType.String)
                        lower = (JToken)lower.Value<string>().ToLower();
                    t2["@language"] = lower;
                }
                JToken jtoken15;
                if (jobject1.TryGetValue("@prefix", out jtoken15))
                {
                    if (Regex.IsMatch(term, ":|\\/"))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context @prefix used on a compact IRI term");
                    if (jtoken8 != null && jtoken8.Type == JTokenType.String && Utils.IsKeyword(jtoken8.Value<string>()))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; keywords may not be used as prefixes");
                    t2["_prefix"] = jtoken15.Type == JTokenType.Boolean ? (JToken)jtoken15.Value<bool>() : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context value for @prefix must be boolean");
                }
                JToken element1;
                if (jobject1.TryGetValue("@direction", out element1))
                    t2["@direction"] = Utils.IsEmptyObject(element1) || element1.Type == JTokenType.String && (element1.Value<string>() == "ltr" || element1.Value<string>() == "rtl") ? element1 : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @direction value must be null, \"ltr\", or \"rtl\".");
                JToken jtoken16;
                if (jobject1.TryGetValue("@nest", out jtoken16))
                    t2["@nest"] = jtoken16.Type == JTokenType.String && (!(jtoken16.Value<string>() != "@nest") || jtoken16.Value<string>().IndexOf('@') != 0) ? jtoken16 : throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context @nest value must be a string which is not a keyword other than @nest.");
                if (jtoken8 != null && jtoken8.Type == JTokenType.String && (jtoken8.Value<string>() == "@context" || jtoken8.Value<string>() == "@preserve"))
                    throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; @context and @preserve cannot be aliased.");
                JToken jtoken17;
                if (t1 == null || !t1.TryGetValue("protected", out jtoken17) || jtoken17.Type != JTokenType.Boolean || !jtoken17.Value<bool>() || overrideProtected)
                    return;
                activeCtx.Protected[term] = true;
                t2["protected"] = (JToken)true;
                if (JToken.DeepEquals(t1, t2))
                    return;
                switch (options.ProtectedMode ?? "error")
                {
                    case "error":
                        throw new Exceptions.JsonLdParseException("Invalid JSON - LD syntax; tried to redefine \"" + term + "\" which is a protected term.");
                    case "warn":
                        break;
                    default:
                        throw new Exceptions.JsonLdParseException("Invalid protectedMode.");
                }
            }
        }

        private static async Task ExpandObject(
          ExpandContext activeCtx,
          string activeProperty,
          string expandedActiveProperty,
          JObject element,
          JObject expandedParent,
          ExpandOptions options,
          bool insideList,
          string typeKey,
          ExpandContext typeScopedContext,
          Func<object, JToken> expansionMap)
        {
            IOrderedEnumerable<JProperty> props = element.Properties().OrderBy(p => p.Name);
            List<string> nests = new List<string>();
            JToken unexpandedValue = null;
            bool isFrame = options.IsFrame;
            JToken elementTypeKeyProp = typeKey != null ? element[typeKey] : null;
            JToken jtoken = elementTypeKeyProp;
            JToken elementTypeKey = (jtoken != null ? jtoken.Type == JTokenType.Array ? 1 : 0 : 0) != 0 ? ((JArray)elementTypeKeyProp)[0] : elementTypeKeyProp;
            bool isJsonType = elementTypeKeyProp != null && ExpandIri(activeCtx, elementTypeKey.ToString(), IriRelativeTo.VocabSet, options: options) == "@json";
            foreach (JProperty jproperty in (IEnumerable<JProperty>)props)
            {
                JProperty prop = jproperty;
                JToken value = prop.Value;
                JToken expandedValue = null;
                if (!(prop.Name == "@context"))
                {
                    string expandedProperty = ExpandIri(activeCtx, prop.Name, IriRelativeTo.VocabSet, options: options);
                    if (expandedProperty == null || !Utils.IsIriAbsolute(expandedProperty) && !Utils.IsKeyword(expandedProperty))
                    {
                        Func<object, JToken> func = expansionMap;
                        expandedProperty = func != null ? func(new
                        {
                            unmappedProperty = prop.Name,
                            activeCtx,
                            activeProperty,
                            parent = element,
                            options,
                            insideList,
                            value,
                            expandedParent
                        })?.ToString() : null;
                        if (expandedProperty == null)
                            continue;
                    }
                    if (Utils.IsKeyword(expandedProperty))
                    {
                        if (expandedActiveProperty == "@reverse")
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; a keyword cannot be used as a @reverse property.");
                        if (expandedParent.ContainsKey(expandedProperty) && expandedProperty != "@included" && expandedProperty != "@type")
                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; colliding keywords detected.");
                    }
                    switch (expandedProperty)
                    {
                        case "@id":
                            if (value.Type != JTokenType.String)
                            {
                                if (!isFrame)
                                    throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@id\" value must a string.");
                                if (value.Type == JTokenType.Object)
                                {
                                    if (!Utils.IsEmptyObject(value))
                                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@id\" value an empty object or array of strings, if framing");
                                }
                                else
                                {
                                    if (value.Type != JTokenType.Array)
                                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@id\" value an empty object or array of strings, if framing");
                                    if (!value.ToArray().All(v => v.Type == JTokenType.String))
                                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@id\" value an empty object or array of strings, if framing");
                                }
                            }
                            JArray valueArray = Utils.AsArray(value);
                            for (int i = 0; i < valueArray.Count; ++i)
                            {
                                if (valueArray[i].Type == JTokenType.String)
                                    valueArray[i] = (JToken)ExpandIri(activeCtx, valueArray[i].Value<string>(), IriRelativeTo.BaseSet, options: options);
                            }
                            Utils.AddValue(expandedParent, "@id", (JToken)valueArray, isFrame);
                            continue;
                        case "@type":
                            if (value.Type == JTokenType.Object)
                            {
                                JObject newObj = new JObject();
                                foreach (JProperty valueProp in ((JObject)value).Properties())
                                {
                                    string newName = ExpandIri(typeScopedContext, valueProp.Name, IriRelativeTo.VocabSet, options: options);
                                    JArray newVal = new JArray((object)Utils.AsArray(valueProp.Value).Select<JToken, string>((Func<JToken, string>)(item => ExpandIri(typeScopedContext, item.ToString(), IriRelativeTo.BothSet, options: options))));
                                    newObj[newName] = newVal;
                                    newName = null;
                                    newVal = null;
                                }
                                value = newObj;
                                newObj = null;
                            }
                            ValidateTypeValue(value, isFrame);
                            Utils.AddValue(expandedParent, "@type", (JToken)new JArray((object)Utils.AsArray(value).Select<JToken, JToken>((Func<JToken, JToken>)(v => v.Type == JTokenType.String ? (JToken)ExpandIri(typeScopedContext, v.Value<string>(), IriRelativeTo.BothSet, options: options) : v))), isFrame);
                            continue;
                        case "@included":
                            JToken token = await Expand(activeCtx, value, activeProperty, options, expansionMap: expansionMap);
                            JArray includedResult = Utils.AsArray(token);
                            token = null;
                            if (!includedResult.All((Func<JToken, bool>)(v => Utils.IsGraphSubject(v))))
                                throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; values of @included must expand to node objects.");
                            Utils.AddValue(expandedParent, "@included", (JToken)includedResult, true);
                            continue;
                        default:
                            if (expandedProperty == "@graph" && value.Type != JTokenType.Object && value.Type != JTokenType.Array)
                                throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@graph\" value must not be an object or an array.");
                            switch (expandedProperty)
                            {
                                case "@value":
                                    unexpandedValue = value;
                                    if (isJsonType)
                                    {
                                        expandedParent["@value"] = value;
                                        continue;
                                    }
                                    Utils.AddValue(expandedParent, "@value", value, isFrame);
                                    continue;
                                case "@language":
                                    if (value != null)
                                    {
                                        if (value.Type != JTokenType.String && !isFrame)
                                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@language\" value must be a string.");
                                        value = new JArray((object)Utils.AsArray(value).Select<JToken, JToken>((Func<JToken, JToken>)(v => v.Type != JTokenType.String ? v : (JToken)v.Value<string>().ToLower())));
                                        Utils.AddValue(expandedParent, "@language", value, isFrame);
                                        continue;
                                    }
                                    continue;
                                case "@direction":
                                    if (value.Type != JTokenType.String && !isFrame)
                                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@direction\" value must be a string.");
                                    value = (JToken)Utils.AsArray(value);
                                    foreach (JToken dir in (IEnumerable<JToken>)value)
                                    {
                                        if (dir.Type == JTokenType.String && dir.Value<string>() != "ltr" && dir.Value<string>() != "rtl")
                                            throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@direction\" must be \"ltr\" or \"rtl\".");
                                    }
                                    Utils.AddValue(expandedParent, "@direction", value, isFrame);
                                    continue;
                                case "@index":
                                    if (value.Type != JTokenType.String)
                                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@index\" value must be a string.");
                                    Utils.AddValue(expandedParent, "@index", value);
                                    continue;
                                case "@reverse":
                                    if (value.Type != JTokenType.Object)
                                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@reverse\" value must be an object.");
                                    expandedValue = await Expand(activeCtx, value, "@reverse", options, expansionMap: expansionMap);
                                    JToken reverseProp;
                                    if (expandedValue.Type == JTokenType.Object && ((JObject)expandedValue).TryGetValue("@reverse", out reverseProp) && reverseProp.Type == JTokenType.Object)
                                    {
                                        foreach (JProperty revProp in ((JObject)reverseProp).Properties())
                                            Utils.AddValue(expandedParent, revProp.Name, revProp.Value, true);
                                    }
                                    JObject reverseMap1 = expandedParent["@reverse"] as JObject;
                                    if (expandedValue.Type == JTokenType.Object)
                                    {
                                        foreach (JProperty property in ((JObject)expandedValue).Properties())
                                        {
                                            JProperty expProp = property;
                                            if (!(expProp.Name == "@reverse"))
                                            {
                                                if (reverseMap1 == null)
                                                {
                                                    reverseMap1 = new JObject();
                                                    expandedParent["@reverse"] = reverseMap1;
                                                }
                                                Utils.AddValue(reverseMap1, expProp.Name, (JToken)new JArray(), true);
                                                JArray items = expProp.Value as JArray;
                                                int ii = 0;
                                                while (true)
                                                {
                                                    int num = ii;
                                                    JArray jarray = items;
                                                    // ISSUE: explicit non-virtual call
                                                    int count = jarray != null ? jarray.Count : 0;
                                                    if (num < count)
                                                    {
                                                        JToken item = items[ii];
                                                        if (!Utils.IsGraphValue(item) && !Utils.IsGraphList(item))
                                                        {
                                                            Utils.AddValue(reverseMap1, expProp.Name, item, true);
                                                            item = null;
                                                            ++ii;
                                                        }
                                                        else
                                                            break;
                                                    }
                                                    else
                                                        goto label_88;
                                                }
                                                throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@reverse\" value must not be a @value or an @list.");
                                            label_88:
                                                items = null;
                                                expProp = null;
                                            }
                                        }
                                        continue;
                                    }
                                    continue;
                                case "@nest":
                                    nests.Add(prop.Name);
                                    continue;
                                default:
                                    ExpandContext termCtx = activeCtx;
                                    JToken ctx = GetContextValue(activeCtx, prop.Name, "@context");
                                    if (ctx != null)
                                        termCtx = await ProcessContext(activeCtx, ctx, true, true, options);
                                    if (!(GetContextValue(termCtx, prop.Name, "@container") is JArray source))
                                        source = new JArray();
                                    List<string> container = source.Select(t => t.ToString()).ToList();
                                    if (container.Contains("@language") && value.Type == JTokenType.Object)
                                    {
                                        JToken direction = GetContextValue(termCtx, prop.Name, "@direction");
                                        expandedValue = ExpandLanguageMap(termCtx, value, direction, options);
                                        direction = null;
                                    }
                                    else if (container.Contains("@index") && value.Type == JTokenType.Object)
                                    {
                                        bool asGraph = container.Contains("@graph");
                                        string indexKey = GetContextValue(termCtx, prop.Name, "@index")?.ToString() ?? "@index";
                                        string propertyIndex = indexKey != "@index" ? ExpandIri(activeCtx, indexKey, IriRelativeTo.VocabSet, options: options) : null;
                                        JArray jarray = await ExpandIndexMap(termCtx, prop.Name, value, expansionMap, asGraph, indexKey, propertyIndex, options);
                                        expandedValue = jarray;
                                        jarray = null;
                                        indexKey = null;
                                        propertyIndex = null;
                                    }
                                    else if (container.Contains("@id") && value.Type == JTokenType.Object)
                                    {
                                        bool asGraph = container.Contains("@graph");
                                        JArray jarray = await ExpandIndexMap(termCtx, prop.Name, value, expansionMap, asGraph, "@id", null, options);
                                        expandedValue = jarray;
                                        jarray = null;
                                    }
                                    else if (container.Contains("@type") && value.Type == JTokenType.Object)
                                    {
                                        JArray jarray = await ExpandIndexMap(termCtx.RevertToPreviousContext(), prop.Name, value, expansionMap, false, "@type", null, options);
                                        expandedValue = jarray;
                                        jarray = null;
                                    }
                                    else
                                    {
                                        bool isList = expandedProperty == "@list";
                                        if (isList || expandedProperty == "@set")
                                        {
                                            string nextActiveProperty = activeProperty;
                                            if (isList && expandedActiveProperty == "@graph")
                                                nextActiveProperty = null;
                                            expandedValue = await Expand(termCtx, value, nextActiveProperty, options, isList, expansionMap: expansionMap);
                                            nextActiveProperty = null;
                                        }
                                        else if (JToken.EqualityComparer.Equals(GetContextValue(activeCtx, prop.Name, "@type"), (JToken)"@json"))
                                        {
                                            expandedValue = new JObject();
                                            expandedValue["@type"] = (JToken)"@json";
                                            expandedValue["@value"] = value;
                                        }
                                        else
                                            expandedValue = await Expand(termCtx, value, prop.Name, options, expansionMap: expansionMap);
                                    }
                                    if (expandedValue == null && expandedProperty != "@value")
                                    {
                                        Func<object, JToken> func = expansionMap;
                                        expandedValue = func != null ? func(new
                                        {
                                            unmappedValue = value,
                                            expandedProperty,
                                            activeCtx = termCtx,
                                            activeProperty,
                                            parent = element,
                                            options,
                                            insideList,
                                            key = prop.Name,
                                            expandedParent
                                        }) : null;
                                        if (expandedValue == null)
                                            continue;
                                    }
                                    if (expandedProperty != "@list" && !Utils.IsGraphList(expandedValue) && container.Contains("@list"))
                                    {
                                        JObject newObj = new JObject();
                                        newObj["@list"] = (JToken)Utils.AsArray(expandedValue);
                                        expandedValue = newObj;
                                        newObj = null;
                                    }
                                    if (container.Contains("@graph") && !container.Any(key => key == "@id" || key == "@index"))
                                        expandedValue = new JArray((object)Utils.AsArray(expandedValue).Select<JToken, JObject>((Func<JToken, JObject>)(v => new JObject()
                                        {
                                            ["@graph"] = (JToken)Utils.AsArray(v)
                                        })));
                                    JObject mappedProp;
                                    JToken mappedReverseProp;
                                    if (termCtx.Mappings.TryGetValue(prop.Name, out mappedProp) && mappedProp.TryGetValue("reverse", out mappedReverseProp) && mappedReverseProp.Type == JTokenType.Boolean && mappedReverseProp.Value<bool>())
                                    {
                                        JToken reverseMap2;
                                        if (!expandedParent.TryGetValue("@reverse", out reverseMap2) || reverseMap2.Type != JTokenType.Object)
                                        {
                                            reverseMap2 = new JObject();
                                            expandedParent["@reverse"] = reverseMap2;
                                        }
                                        expandedValue = (JToken)Utils.AsArray(expandedValue);
                                        for (int ii = 0; ii < ((JContainer)expandedValue).Count; ++ii)
                                        {
                                            JToken item = expandedValue[ii];
                                            if (Utils.IsGraphValue(item) || Utils.IsGraphList(item))
                                                throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@reverse\" value must not be a @value or an @list.");
                                            Utils.AddValue((JObject)reverseMap2, expandedProperty, item, true);
                                            item = null;
                                        }
                                        continue;
                                    }
                                    Utils.AddValue(expandedParent, expandedProperty, expandedValue, true);
                                    value = null;
                                    expandedValue = null;
                                    expandedProperty = null;
                                    termCtx = null;
                                    ctx = null;
                                    container = null;
                                    mappedProp = null;
                                    mappedReverseProp = null;
                                    prop = null;
                                    continue;
                            }
                    }
                }
            }
            if (expandedParent.ContainsKey("@value") && !(expandedParent["@type"]?.ToString() == "@json") && (unexpandedValue.Type == JTokenType.Object || unexpandedValue.Type == JTokenType.Array) && !isFrame)
                throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@value\" value must not be an object or an array.");
            foreach (string key in nests)
            {
                JArray nestedValues = Utils.AsArray(element[key]);
                foreach (JToken nv in nestedValues)
                {
                    if (nv.Type != JTokenType.Object || ((JObject)nv).Properties().Any(prop => ExpandIri(activeCtx, prop.Name, IriRelativeTo.VocabSet, options: options) == "@value"))
                        throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; nested value must be a node object.");
                    await ExpandObject(activeCtx, activeProperty, expandedActiveProperty, (JObject)nv, expandedParent, options, insideList, typeKey, typeScopedContext, expansionMap);
                }
                nestedValues = null;
            }
            props = null;
            nests = null;
            unexpandedValue = null;
            elementTypeKeyProp = null;
            elementTypeKey = null;
        }

        private static void ValidateTypeValue(JToken v, bool isFrame)
        {
            if (v.Type != JTokenType.String && (v.Type != JTokenType.Array || !v.All(vv => vv.Type == JTokenType.String)))
            {
                if (isFrame && v.Type == JTokenType.Object)
                {
                    JObject jobject = (JObject)v;
                    switch (jobject.Properties().Count())
                    {
                        case 0:
                            return;
                        case 1:
                            JToken token;
                            if (jobject.TryGetValue("@default", out token) && Utils.AsArray(token).All<JToken>((Func<JToken, bool>)(vv => vv.Type == JTokenType.String)))
                                return;
                            break;
                    }
                }
                throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; \"@type\" value must a string, an array of strings, an empty object, or a default object.");
            }
        }

        private static JArray ExpandLanguageMap(
          ExpandContext activeCtx,
          JToken languageMap,
          JToken direction,
          ExpandOptions options)
        {
            JArray jarray = new JArray();
            if (languageMap.Type == JTokenType.Object)
            {
                foreach (JProperty jproperty in (IEnumerable<JProperty>)((JObject)languageMap).Properties().OrderBy(p => p.Name))
                {
                    string str = ExpandIri(activeCtx, jproperty.Name, IriRelativeTo.VocabSet, options: options);
                    foreach (JToken jtoken in Utils.AsArray(jproperty.Value))
                    {
                        if (jtoken != null)
                        {
                            if (jtoken.Type != JTokenType.String)
                                throw new Exceptions.JsonLdParseException("Invalid JSON-LD syntax; language map values must be strings.");
                            JObject jobject = new JObject();
                            jobject["@value"] = jtoken;
                            if (str != "@none")
                                jobject["@language"] = (JToken)jproperty.Name.ToLower();
                            if (!Utils.IsEmptyObject(direction))
                                jobject["@direction"] = direction;
                            jarray.Add(jobject);
                        }
                    }
                }
            }
            return jarray;
        }

        private static async Task<JArray> ExpandIndexMap(
          ExpandContext activeCtx,
          string activeProperty,
          JToken value,
          Func<object, JToken> expansionMap,
          bool asGraph,
          string indexKey,
          string propertyIndex,
          ExpandOptions options)
        {
            JArray rval = new JArray();
            if (value.Type == JTokenType.Object)
            {
                bool isTypeIndex = indexKey == "@type";
                foreach (JProperty prop in (IEnumerable<JProperty>)((JObject)value).Properties().OrderBy(p => p.Name))
                {
                    string key = prop.Name;
                    if (isTypeIndex)
                    {
                        JToken ctx = GetContextValue(activeCtx, key, "@context");
                        if (ctx != null)
                        {
                            ExpandContext expandContext = await ProcessContext(activeCtx, ctx, false, false, options);
                            activeCtx = expandContext;
                            expandContext = null;
                        }
                        ctx = null;
                    }
                    JToken val = await Expand(activeCtx, (JToken)Utils.AsArray(prop.Value), activeProperty, options, insideIndex: true, expansionMap: expansionMap);
                    JToken expandedKey = null;
                    expandedKey = propertyIndex == null ? (JToken)ExpandIri(activeCtx, key, IriRelativeTo.VocabSet, options: options) : !(key == "@none") ? ExpandValue(activeCtx, (JToken)key, indexKey, options) : (JToken)"@none";
                    if (indexKey == "@id")
                        key = ExpandIri(activeCtx, key, IriRelativeTo.BaseSet, options: options);
                    else if (isTypeIndex)
                        key = expandedKey.ToString();
                    JToken[] items = new JToken[0];
                    if (val.Type == JTokenType.Object)
                        items = ((JObject)val).Properties().Select(p => p.Value).ToArray();
                    if (val.Type == JTokenType.Array)
                        items = val.ToArray();
                    JToken[] jtokenArray = items;
                    for (int index = 0; index < jtokenArray.Length; ++index)
                    {
                        JToken itemElem = jtokenArray[index];
                        JToken item = itemElem;
                        if (asGraph && !Utils.IsGraph(item))
                        {
                            item = new JObject();
                            item["@graph"] = new JArray(itemElem);
                        }
                        if (indexKey == "@type")
                        {
                            if ((expandedKey.Type != JTokenType.String || !(expandedKey.Value<string>() == "@none")) && item.Type == JTokenType.Object)
                            {
                                JObject itemObj = (JObject)item;
                                JToken typeProp;
                                if (itemObj.TryGetValue("@type", out typeProp))
                                {
                                    JArray arr = new JArray(key);
                                    arr.Add(typeProp);
                                    item["@type"] = arr;
                                    arr = null;
                                }
                                else
                                    item["@type"] = new JArray(key);
                                itemObj = null;
                                typeProp = null;
                            }
                        }
                        else
                        {
                            int num;
                            if (Utils.IsGraphValue(item))
                                num = !(new string[3]
                                {
                  "@language",
                  "@type",
                  "@index"
                                }).Contains(indexKey) ? 1 : 0;
                            else
                                num = 0;
                            if (num != 0)
                                throw new JsonLdParseException("Invalid JSON-LD syntax; Attempt to add illegal key to value object: \"" + indexKey + "\".");
                            if (expandedKey.Type != JTokenType.String || expandedKey.Value<string>() != "@none")
                            {
                                if (!string.IsNullOrEmpty(propertyIndex))
                                {
                                    if (item.Type == JTokenType.Object)
                                        Utils.AddValue((JObject)item, propertyIndex, expandedKey, true, prependValue: true);
                                }
                                else if (item.Type == JTokenType.Object && !((JObject)item).ContainsKey(indexKey))
                                    item[indexKey] = (JToken)key;
                            }
                        }
                        rval.Add(item);
                        item = null;
                        itemElem = null;
                    }
                    jtokenArray = null;
                    key = null;
                    val = null;
                    expandedKey = null;
                    items = null;
                }
            }
            JArray jarray = rval;
            rval = null;
            return jarray;
        }

        private static string ExpandIri(
          ExpandContext activeCtx,
          string value,
          IriRelativeTo relativeTo = null,
          JObject localCtx = null,
          Dictionary<string, bool> defined = null,
          ExpandOptions options = null)
        {
            if (value == null || Utils.IsKeyword(value))
                return value;
            if (Utils.IsIriKeyword(value))
                return null;
            bool flag;
            if (localCtx != null && localCtx.ContainsKey(value) && !(defined.TryGetValue(value, out flag) & flag))
                CreateTermDefinition(activeCtx, localCtx, value, defined, options);
            JObject jobject1;
            if ((object)relativeTo != null && relativeTo.Vocab && activeCtx.Mappings.TryGetValue(value, out jobject1))
            {
                if (jobject1 == null)
                    return null;
                JToken jtoken;
                if (jobject1.TryGetValue("@id", out jtoken))
                    return jtoken.ToString();
            }
            int length = value.IndexOf(':');
            if (length > 0)
            {
                string str1 = value.Substring(0, length);
                string str2 = value.Substring(length + 1);
                if (str1 == "_" || str2.IndexOf("//") == 0)
                    return value;
                // ISSUE: explicit non-virtual call
                if (localCtx != null && localCtx.ContainsKey(str1))
                    CreateTermDefinition(activeCtx, localCtx, str1, defined, options);
                JObject jobject2;
                JToken jtoken1;
                if (activeCtx.Mappings.TryGetValue(str1, out jobject2) && jobject2.TryGetValue("_prefix", out jtoken1) && jtoken1.Value<bool>())
                {
                    JToken jtoken2;
                    return (jobject2.TryGetValue("@id", out jtoken2) ? jtoken2?.ToString() ?? "" : "") + str2;
                }
                if (Utils.IsIriAbsolute(value))
                    return value;
            }
            JToken jtoken3;
            if (relativeTo.Vocab && activeCtx.Fields.TryGetValue("@vocab", out jtoken3))
                return (jtoken3.Type == JTokenType.String ? jtoken3.Value<string>() : "") + value;
            JToken element;
            if (relativeTo.Base && activeCtx.Fields.TryGetValue("@base", out element) && (Utils.IsEmptyObject(element) || element != null && element.Type == JTokenType.String))
            {
                string iri = element?.ToString();
                if (!string.IsNullOrEmpty(iri))
                    return Utils.PrependBase(Utils.PrependBase(options.Base, iri), value);
            }
            else if (relativeTo.Base)
                return Utils.PrependBase(options.Base, value);
            return value;
        }

        private static JToken ExpandValue(
          ExpandContext activeCtx,
          JToken value,
          string activeProperty = null,
          ExpandOptions options = null)
        {
            if (Utils.IsEmptyObject(value))
                return null;
            string v = ExpandIri(activeCtx, activeProperty, IriRelativeTo.VocabSet, options: options);
            if (v == "@id" || v == "@type")
            {
                if (value.Type != JTokenType.String)
                    return value;
                IriRelativeTo relativeTo = new IriRelativeTo()
                {
                    Base = true,
                    Vocab = v == "@type"
                };
                return (JToken)ExpandIri(activeCtx, value.Value<string>(), relativeTo, options: options);
            }
            JToken contextValue1 = GetContextValue(activeCtx, activeProperty, "@type");
            string str1 = contextValue1 == null || contextValue1.Type != JTokenType.String ? null : contextValue1.Value<string>();
            if ((str1 == "@id" || v == "@graph") && value.Type == JTokenType.String)
                return new JObject()
                {
                    ["@id"] = (JToken)ExpandIri(activeCtx, value.Value<string>(), IriRelativeTo.BaseSet, options: options)
                };
            if (str1 == "@vocab" && value.Type == JTokenType.String)
                return new JObject()
                {
                    ["@id"] = (JToken)ExpandIri(activeCtx, value.Value<string>(), IriRelativeTo.BothSet, options: options)
                };
            if (Utils.IsKeyword(v))
                return value;
            JObject jobject = new JObject();
            int num;
            if (!string.IsNullOrEmpty(str1))
                num = !(new string[3]
                {
          "@id",
          "@vocab",
          "@none"
                }).Contains(str1) ? 1 : 0;
            else
                num = 0;
            if (num != 0)
                jobject["@type"] = (JToken)str1;
            else if (value.Type == JTokenType.String)
            {
                JToken contextValue2 = GetContextValue(activeCtx, activeProperty, "@language");
                if (contextValue2 != null)
                    jobject["@language"] = contextValue2;
                JToken contextValue3 = GetContextValue(activeCtx, activeProperty, "@direction");
                if (contextValue3 != null)
                    jobject["@direction"] = contextValue3;
            }
            if (value.Type == JTokenType.Boolean || value.Type == JTokenType.Float || value.Type == JTokenType.Integer || value.Type == JTokenType.String)
            {
                jobject["@value"] = value;
            }
            else
            {
                string str2 = value.ToString(Formatting.None);
                if (str2.StartsWith("\"") && str2.EndsWith("\""))
                {
                    string str3 = str2;
                    str2 = str3.Substring(1, str3.Length - 1 - 1);
                }
                jobject["@value"] = (JToken)str2;
            }
            return jobject;
        }

        private static JToken GetContextValue(ExpandContext ctx, string key, string type)
        {
            if (key == null || type == null)
                return null;
            JObject jobject;
            JToken contextValue;
            if (ctx.Mappings.TryGetValue(key, out jobject) && jobject.TryGetValue(type, out contextValue))
                return contextValue;
            JToken jtoken;
            return (type == "@language" || type == "@direction") && ctx.Fields.TryGetValue(type, out jtoken) ? jtoken : null;
        }
    }
}
