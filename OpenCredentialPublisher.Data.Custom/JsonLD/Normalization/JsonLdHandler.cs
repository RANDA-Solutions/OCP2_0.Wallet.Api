// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.JsonLdHandler
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    public static class JsonLdHandler
    {
        public static async
#nullable disable
        Task<string> Canonize(string json, ExpandOptions options = null)
        {
            string str = await Normalize(json, options);
            return str;
        }

        public static async Task<string> Normalize(string json, ExpandOptions options = null)
        {
            List<Quad> dataset = await ToRDF(json, options);
            string str = URDNA2015.Normalize(dataset);
            dataset = null;
            return str;
        }

        private static async Task<List<Quad>> ToRDF(string json, ExpandOptions options = null)
        {
            JToken expanded = await Expand(json, options);
            List<Quad> rdf = expanded.ToRDF();
            expanded = null;
            return rdf;
        }

        private static async Task<JToken> Expand(string json, ExpandOptions options = null)
        {
            ExpandContext activeCtx = new ExpandContext();
            JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(json));
            jsonTextReader.DateParseHandling = 0;
            JToken token;
            using (JsonTextReader reader = jsonTextReader)
                token = JToken.Load(reader);
            List<JObject> objects = new List<JObject>();
            if (token.Type == JTokenType.Array)
                objects.AddRange(token.ToArray().Where(t => t.Type == JTokenType.Object).Cast<JObject>());
            if (token.Type == JTokenType.Object)
                objects.Add((JObject)token);
            JArray result = new JArray();
            foreach (JObject doc in objects)
            {
                if (options == null)
                    options = new ExpandOptions();
                JToken expanded = await Expansion.Expand(activeCtx, doc, options: options);
                JToken jtoken = expanded;
                if (jtoken != null && jtoken.Type == JTokenType.Object)
                {
                    JObject expandedObj = (JObject)expanded;
                    JToken graphProp = null;
                    if (expandedObj.TryGetValue("@graph", out graphProp) && expandedObj.Properties().Count() == 1)
                        expanded = graphProp;
                    expandedObj = null;
                    graphProp = null;
                }
                if (expanded != null)
                    result.Add(expanded);
                expanded = null;
            }
            JToken jtoken1 = result;
            activeCtx = null;
            token = null;
            objects = null;
            result = null;
            return jtoken1;
        }
    }
}
