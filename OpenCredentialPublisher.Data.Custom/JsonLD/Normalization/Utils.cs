// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.Utils
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Data.Custom.JsonLD.Normalization.Exceptions;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal static class Utils
    {
        private static string[] UriKeys = new string[14]
        {
      "href",
      "protocol",
      "scheme",
      "authority",
      "auth",
      "user",
      "password",
      "hostname",
      "port",
      "path",
      "directory",
      "file",
      "query",
      "fragment"
        };
        private static Regex UriRegex = new Regex("^(([^:\\/?#]+):)?(?:\\/\\/((?:(([^:@]*)(?::([^:@]*))?)?@)?([^:\\/?#]*)(?::(\\d*))?))?(?:(((?:[^?#\\/]*\\/)*)([^?#]*))(?:\\?([^#]*))?(?:#(.*))?)");
        private static Regex IriKeywordRegex = new Regex("^@[a-zA-Z]+$");
        private static Regex AbsoluteIriRegex = new Regex("^([A-Za-z][A-Za-z0-9+-.]*|_):[^\\s]*$");

        public static JToken[] NormalizeContext(JToken context)
        {
            JToken jtoken;
            if (context.Type == JTokenType.Object && ((JObject)context).TryGetValue("@context", out jtoken) && jtoken.Type == JTokenType.Array)
                context = jtoken;
            JToken[] jtokenArray;
            if (context.Type != JTokenType.Array)
                jtokenArray = new JToken[1] { context };
            else
                jtokenArray = context.ToArray();
            return jtokenArray;
        }

        public static string PrependBase(string baseVal, string iri)
        {
            return string.IsNullOrEmpty(baseVal) ? iri : PrependBase(ParseUri(baseVal), iri);
        }

        public static string PrependBase(Dictionary<string, string> baseVal, string iri)
        {
            if (baseVal == null || AbsoluteIriRegex.IsMatch(iri))
                return iri;
            Dictionary<string, string> uri = ParseUri(iri);
            Dictionary<string, string> dict = new Dictionary<string, string>()
      {
        {
          "protocol",
          GetFromDict(baseVal, "protocol", "")
        }
      };
            string fromDict1 = GetFromDict(uri, "authority");
            string fromDict2 = GetFromDict(uri, "path");
            string fromDict3 = GetFromDict(uri, "query");
            if (fromDict1 != null)
            {
                dict["authority"] = fromDict1;
                dict["path"] = fromDict2;
                dict["query"] = fromDict3;
            }
            else
            {
                dict["authority"] = GetFromDict(baseVal, "authority");
                if ((fromDict2 ?? "") == "")
                {
                    dict["path"] = GetFromDict(baseVal, "path");
                    dict["query"] = fromDict3 == null ? GetFromDict(baseVal, "query") : fromDict3;
                }
                else
                {
                    if (fromDict2.IndexOf('/') == 0)
                    {
                        dict["path"] = fromDict2;
                    }
                    else
                    {
                        string fromDict4 = GetFromDict(baseVal, "path");
                        string str1 = fromDict4.Substring(0, fromDict4.LastIndexOf('/') + 1);
                        int num;
                        if (str1.Length <= 0)
                        {
                            string fromDict5 = GetFromDict(baseVal, "authority");
                            if ((fromDict5 != null ? fromDict5.Any() ? 1 : 0 : 0) == 0)
                            {
                                num = 0;
                                goto label_12;
                            }
                        }
                        num = !str1.EndsWith("/") ? 1 : 0;
                    label_12:
                        if (num != 0)
                            str1 += "/";
                        string str2 = str1 + fromDict2;
                        dict["path"] = str2;
                    }
                    dict["query"] = fromDict3;
                }
            }
            if ((fromDict2 ?? "") != "")
                dict["path"] = RemoveDotSegments(dict["path"]);
            string str3 = dict["protocol"];
            if (GetFromDict(dict, "authority") != null)
                str3 = str3 + "//" + dict["authority"];
            string str4 = str3 + dict["path"];
            if (GetFromDict(dict, "query") != null)
                str4 = str4 + "?" + dict["query"];
            if (GetFromDict(uri, "fragment") != null)
                str4 = str4 + "#" + uri["fragment"];
            if (str4 == "")
                str4 = "./";
            return str4;
        }

        public static JArray AsArray(JToken token)
        {
            if (token.Type == JTokenType.Array)
                return (JArray)token;
            return new JArray() { token };
        }

        public static bool IsKeyword(string v)
        {
            if (string.IsNullOrEmpty(v) || v[0] != '@')
                return false;
            switch (v)
            {
                case "@base":
                case "@container":
                case "@context":
                case "@default":
                case "@direction":
                case "@embed":
                case "@explicit":
                case "@graph":
                case "@id":
                case "@included":
                case "@index":
                case "@json":
                case "@language":
                case "@list":
                case "@nest":
                case "@none":
                case "@omitDefault":
                case "@prefix":
                case "@preserve":
                case "@protected":
                case "@requireAll":
                case "@reverse":
                case "@set":
                case "@type":
                case "@value":
                case "@version":
                case "@vocab":
                    return true;
                default:
                    return false;
            }
        }

        public static Dictionary<string, string> ParseUri(string str)
        {
            Dictionary<string, string> uri = new Dictionary<string, string>();
            Match match = UriRegex.Match(str);
            int length = UriKeys.Length;
            while (length-- > 0)
                uri[UriKeys[length]] = match.Groups[length].Success ? match.Groups[length].Value : null;
            if (uri["scheme"] == "https" && uri["port"] == "443" || uri["scheme"] == "http" && uri["port"] == "80")
            {
                uri["href"] = uri["href"].Replace(":" + uri["port"], "");
                uri["authority"] = uri["authority"].Replace(":" + uri["port"], "");
                uri["port"] = null;
            }
            uri["normalizedPath"] = RemoveDotSegments(uri["path"]);
            return uri;
        }

        public static string RemoveDotSegments(string path)
        {
            if (!path.Any())
                return "";
            string[] strArray = path.Split('/');
            List<string> stringList = new List<string>();
            for (int index = 0; index < strArray.Length; ++index)
            {
                string str = strArray[index];
                bool flag = strArray.Length == index + 1;
                switch (str)
                {
                    case ".":
                        if (flag)
                        {
                            stringList.Add("");
                            break;
                        }
                        break;
                    case "..":
                        if (stringList.Count > 0)
                            stringList.RemoveAt(stringList.Count - 1);
                        if (flag)
                        {
                            stringList.Add("");
                            break;
                        }
                        break;
                    default:
                        stringList.Add(str);
                        break;
                }
            }
            if (path[0] == '/' && stringList.Any() && stringList[0] != "")
                stringList = stringList.Prepend("").ToList();
            return stringList.Count == 1 && stringList[0] == "" ? "/" : string.Join("/", (IEnumerable<string>)stringList);
        }

        public static bool IsEmptyObject(JToken element)
        {
            return element == null || element.Type == JTokenType.Null || element.Type == JTokenType.Undefined;
        }

        public static bool IsBlankNode(JToken v)
        {
            if (v.Type != JTokenType.Object)
                return false;
            JObject jobject = (JObject)v;
            JToken jtoken;
            return jobject.TryGetValue("@id", out jtoken) ? jtoken.Type == JTokenType.String && jtoken.Value<string>().IndexOf("_:") == 0 : !jobject.Properties().Any() || !jobject.ContainsKey("@value") && !jobject.ContainsKey("@set") && !jobject.ContainsKey("@list");
        }

        public static bool IsGraphSubject(JToken v)
        {
            if (v.Type == JTokenType.Object)
            {
                JObject jobject = (JObject)v;
                if (!jobject.ContainsKey("@value") && !jobject.ContainsKey("@set") && !jobject.ContainsKey("@list"))
                    return jobject.Properties().Count() > 1 || !jobject.ContainsKey("@id");
            }
            return false;
        }

        public static bool IsGraphSubjectReference(JToken v)
        {
            if (v.Type != JTokenType.Object)
                return false;
            JObject jobject = (JObject)v;
            return jobject.Properties().Count() == 1 && jobject.ContainsKey("@id");
        }

        public static bool IsGraphValue(JToken v)
        {
            return v.Type == JTokenType.Object && ((JObject)v).ContainsKey("@value");
        }

        public static bool IsGraphList(JToken v)
        {
            return v.Type == JTokenType.Object && ((JObject)v).ContainsKey("@list");
        }

        public static bool IsGraph(JToken v)
        {
            return v.Type == JTokenType.Object && ((JObject)v).ContainsKey("@graph") && ((JObject)v).Properties().Where(prop => prop.Name != "@id" && prop.Name != "@index").Count() == 1;
        }

        public static bool HasGraphValue(JObject subject, string property, JToken value)
        {
            if (HasProperty(subject, property))
            {
                JToken jtoken = subject[property];
                bool flag = IsGraphList(jtoken);
                if (jtoken.Type == JTokenType.Array | flag)
                {
                    if (flag)
                    {
                        jtoken = jtoken["@list"];
                        if (jtoken.Type != JTokenType.Array)
                            throw new JsonLdParseException("\"@list\" field in \"" + property + "\" has to be an array");
                    }
                    JArray jarray = (JArray)jtoken;
                    for (int index = 0; index < jarray.Count; ++index)
                    {
                        if (CompareValues(value, jarray[index]))
                            return true;
                    }
                }
                else if (value.Type != JTokenType.Array)
                    return CompareValues(value, jtoken);
            }
            return false;
        }

        public static bool HasProperty(JObject subject, string property)
        {
            JToken jtoken;
            return subject.TryGetValue(property, out jtoken) && (jtoken.Type != JTokenType.Array || ((JContainer)jtoken).Count > 0);
        }

        public static bool CompareValues(JToken v1, JToken v2)
        {
            if (v1 == v2 || JToken.EqualityComparer.Equals(v1, v2) || IsGraphValue(v1) && IsGraphValue(v2) && v1["@value"] == v2["@value"] && v1["@type"] == v2["@type"] && v1["@language"] == v2["@language"] && v1["@index"] == v2["@index"])
                return true;
            JToken jtoken1;
            JToken jtoken2;
            return v1.Type == JTokenType.Object && ((JObject)v1).TryGetValue("@id", out jtoken1) && v2.Type == JTokenType.Object && ((JObject)v2).TryGetValue("@id", out jtoken2) && jtoken1 == jtoken2;
        }

        public static void AddValue(
          JObject subject,
          string property,
          JToken value,
          bool propertyIsArray = false,
          bool allowDuplicate = false,
          bool valueIsArray = false,
          bool prependValue = false)
        {
            if (valueIsArray)
                subject[property] = value;
            else if (value.Type == JTokenType.Array)
            {
                JArray jarray = (JArray)value;
                if (jarray.Count == 0 & propertyIsArray && !subject.ContainsKey(property))
                    subject[property] = new JArray();
                if (prependValue)
                {
                    JToken source;
                    if (subject.TryGetValue(property, out source) && source.Type == JTokenType.Array)
                    {
                        foreach (JToken jtoken in source.ToArray())
                            jarray.Add(jtoken);
                    }
                    subject[property] = new JArray();
                }
                for (int index = 0; index < jarray.Count; ++index)
                    AddValue(subject, property, jarray[index], propertyIsArray, allowDuplicate, valueIsArray, prependValue);
            }
            else
            {
                JToken jtoken;
                if (subject.TryGetValue(property, out jtoken))
                {
                    bool flag = !allowDuplicate && HasGraphValue(subject, property, value);
                    if (jtoken.Type != JTokenType.Array && !flag | propertyIsArray)
                        jtoken = subject[property] = new JArray()
            {
              jtoken
            };
                    if (flag)
                        return;
                    if (prependValue)
                        ((JContainer)jtoken).AddFirst(value);
                    else
                        ((JArray)jtoken).Add(value);
                }
                else if (propertyIsArray)
                    subject[property] = new JArray()
          {
            value
          };
                else
                    subject[property] = value;
            }
        }

        public static V GetFromDict<T, V>(Dictionary<T, V> dict, T key, V defVal = null) where V : class
        {
            V v;
            return dict.TryGetValue(key, out v) && v != null ? v : defVal;
        }

        public static bool IsIriKeyword(string value) => IriKeywordRegex.IsMatch(value);

        public static bool IsIriAbsolute(string value)
        {
            return !string.IsNullOrEmpty(value) && AbsoluteIriRegex.IsMatch(value);
        }
    }
}
