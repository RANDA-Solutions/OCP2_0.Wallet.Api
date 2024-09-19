// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.ContextResolver
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.Data.Custom.JsonLD.Normalization.Exceptions;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    public class ContextResolver : IContextResolver
    {
        protected static readonly
#nullable disable
        ConcurrentDictionary<string, (string, string, DateTime)> documentCache = new ConcurrentDictionary<string, (string, string, DateTime)>();
        protected static readonly TimeSpan CACHE_TIMEOUT = TimeSpan.FromMinutes(5.0);
        protected const int MAX_CONTEXT_URLS = 10;
        protected const int MAX_REDIRECTS = 10;
        protected readonly Dictionary<string, ResolvedContext> OperationCache = new Dictionary<string, ResolvedContext>();
        protected readonly Dictionary<string, Dictionary<string, ResolvedContext>> SharedCache = new Dictionary<string, Dictionary<string, ResolvedContext>>();
        protected static readonly Regex JsonContentTypeRegex = new Regex("^application\\/(\\w*\\+)?json$");
        protected static readonly Regex LinkHeadersRegex = new Regex("(?:<[^>] *?>| \"[^\"]*?\"|[^,])+");
        protected static readonly Regex LinkHeaderRegex = new Regex("\\s*<([^>]*?)>\\s* (?:;\\s* (.*))?");
        protected static readonly Regex LinkHeaderParamsRegex = new Regex("(.*?)=(?:(?:\"([^\"]*?)\")| ([^\"]*?))\\s*(?:(?:;\\s*)|$)");

        public virtual async Task<List<ResolvedContext>> Resolve(
          ExpandContext activeCtx,
          JToken context,
          string baseUrl)
        {
            List<ResolvedContext> resolvedContextList = await Resolve(activeCtx, context, baseUrl, null);
            return resolvedContextList;
        }

        public virtual async Task<List<ResolvedContext>> Resolve(
          ExpandContext activeCtx,
          JToken context,
          string baseUrl,
          HashSet<object> cycles)
        {
            if (cycles == null)
                cycles = new HashSet<object>();
            JToken[] ctxs = Utils.NormalizeContext(context);
            List<ResolvedContext> allResolved = new List<ResolvedContext>();
            JToken[] jtokenArray = ctxs;
            for (int index = 0; index < jtokenArray.Length; ++index)
            {
                JToken ctx = jtokenArray[index];
                if (ctx.Type == JTokenType.String)
                {
                    string ctxVal = ctx.Value<string>();
                    ResolvedContext resolvedCtx = Get(ctxVal);
                    if (resolvedCtx != null)
                    {
                        allResolved.Add(resolvedCtx);
                    }
                    else
                    {
                        List<ResolvedContext> resolvedContextList = allResolved;
                        List<ResolvedContext> collection = await ResolveRemoteContext(activeCtx, ctxVal, baseUrl, cycles);
                        resolvedContextList.AddRange(collection);
                        resolvedContextList = null;
                        collection = null;
                    }
                }
                else if (ctx == null || Utils.IsEmptyObject(ctx))
                {
                    allResolved.Add(new ResolvedContext(null));
                }
                else
                {
                    string key = ctx.Type == JTokenType.Object ? ctx.ToString() : throw new JsonLdParseException("Invalid JSON-LD syntax; @context must be an object.");
                    ResolvedContext resolved = Get(key);
                    if (resolved == null)
                    {
                        resolved = new ResolvedContext(ctx);
                        CacheResolvedContext(key, resolved, "static");
                    }
                    allResolved.Add(resolved);
                    key = null;
                    resolved = null;
                    ctx = null;
                }
            }
            jtokenArray = null;
            List<ResolvedContext> resolvedContextList1 = allResolved;
            ctxs = null;
            allResolved = null;
            return resolvedContextList1;
        }

        protected ResolvedContext Get(string key)
        {
            ResolvedContext resolvedContext;
            Dictionary<string, ResolvedContext> dictionary;
            if (!OperationCache.TryGetValue(key, out resolvedContext) && SharedCache.TryGetValue(key, out dictionary) && dictionary.TryGetValue("static", out resolvedContext))
                OperationCache[key] = resolvedContext;
            return resolvedContext;
        }

        protected virtual async Task<List<ResolvedContext>> ResolveRemoteContext(
          ExpandContext activeCtx,
          string url,
          string baseUrl,
          HashSet<object> cycles)
        {
            url = Utils.PrependBase(baseUrl, url);
            (JObject, string) valueTuple1 = await FetchContext(activeCtx, url, cycles);
            (JObject, string) valueTuple2 = valueTuple1;
            JObject context = valueTuple2.Item1;
            string docUrl = valueTuple2.Item2;
            baseUrl = !string.IsNullOrEmpty(docUrl) ? docUrl : url;
            ResolveContextUrls(context, baseUrl);
            List<ResolvedContext> resolved = await Resolve(activeCtx, context, baseUrl, cycles);
            foreach (ResolvedContext ctx in resolved)
                CacheResolvedContext(url, ctx);
            List<ResolvedContext> resolvedContextList = resolved;
            return resolvedContextList;
        }

        protected virtual async Task<(JObject, string)> FetchContext(
          ExpandContext activeCtx,
          string url,
          HashSet<object> cycles)
        {
            if (cycles.Count > 10)
                throw new JsonLdParseException("Maximum number of @context URLs exceeded.");
            if (cycles.Contains(url))
                throw new JsonLdParseException("Cyclical @context URLs detected.");
            cycles.Add(url);
            string docBody;
            string docUrl;
            JToken context;
            try
            {
                (string, string, DateTime) cached;
                bool fromCache = documentCache.TryGetValue(url, out cached) && DateTime.UtcNow - cached.Item3 < CACHE_TIMEOUT;
                if (fromCache)
                {
                    string str1 = cached.Item1;
                    string str2 = cached.Item2;
                    docBody = str1;
                    docUrl = str2;
                }
                else
                {
                    (string, string) valueTuple1 = await LoadDocument(url);
                    (string, string) valueTuple2 = valueTuple1;
                    docBody = valueTuple2.Item1;
                    docUrl = valueTuple2.Item2;
                }
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(docBody));
                jsonTextReader.DateParseHandling = DateParseHandling.None;
                await using (JsonTextReader reader = jsonTextReader)
                    context = await JToken.LoadAsync(reader);
                if (!fromCache)
                    documentCache[url] = (docBody, docUrl, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                throw new JsonLdParseException("Dereferencing a URL did not result in a valid JSON-LD object. Possible causes are an inaccessible URL perhaps due to a same-origin policy (ensure the server uses CORS if you are using client-side JavaScript), too many redirects, a non-JSON response, or more than one HTTP Link Header was provided for a remote context.", ex);
            }
            if (context.Type != JTokenType.Object)
                throw new JsonLdParseException("Dereferencing a URL did not result in a JSON object. The response was valid JSON, but it was not a JSON object.");
            JObject result = new JObject();
            JToken contextProp;
            result["@context"] = !((JObject)context).TryGetValue("@context", out contextProp) ? new JObject() : contextProp;
            (JObject, string) valueTuple = (result, docUrl);
            docBody = null;
            docUrl = null;
            context = null;
            result = null;
            contextProp = null;
            return valueTuple;
        }

        protected virtual async Task<(string, string)> LoadDocument(string url, List<string> redirects = null)
        {
            if (redirects == null)
                redirects = new List<string>();
            JToken alternate = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.GetAsync(url);
            if (res.StatusCode >= HttpStatusCode.BadRequest)
                throw new JsonLdParseException(string.Format("URL '{0}' could not be dereferenced: {1}", url, res.StatusCode));
            IEnumerable<string> links;
            string link = res.Headers.TryGetValues("link", out links) ? links.FirstOrDefault() : null;
            IEnumerable<string> locations;
            string location = res.Headers.TryGetValues("location", out locations) ? locations.FirstOrDefault() : null;
            IEnumerable<string> contentTypes;
            string contentType = res.Headers.TryGetValues("content-type", out contentTypes) ? contentTypes.FirstOrDefault() : null;
            if (link != null && contentType != "application/ld+json")
            {
                JObject linkHeaders = ParseLinkHeader(link);
                if (linkHeaders.TryGetValue("alternate", out alternate) && alternate.Type == JTokenType.Object)
                {
                    JObject alternateObj = (JObject)alternate;
                    JToken typeToken;
                    JToken targetToken;
                    if (alternateObj.TryGetValue("type", out typeToken) && typeToken.Value<string>() == "application/ld+json" && !JsonContentTypeRegex.IsMatch(contentType ?? "") && alternateObj.TryGetValue("target", out targetToken))
                        location = Utils.PrependBase(url, targetToken.Value<string>());
                    alternateObj = null;
                    typeToken = null;
                    targetToken = null;
                }
                linkHeaders = null;
            }
            if ((alternate != null || res.StatusCode >= HttpStatusCode.Ambiguous && res.StatusCode < HttpStatusCode.BadRequest) && location != null)
            {
                if (redirects.Count >= 10)
                    throw new JsonLdParseException("URL could not be dereferenced; there were too many redirects.");
                if (redirects.Contains(url))
                    throw new JsonLdParseException("URL could not be dereferenced; infinite redirection was detected.");
                redirects.Add(url);
                string nextUrl = new Uri(new Uri(url), location).AbsoluteUri;
                (string, string) valueTuple = await LoadDocument(nextUrl, redirects);
                return valueTuple;
            }
            redirects.Add(url);
            string str = await res.Content.ReadAsStringAsync();
            return (str, url);
        }

        protected JObject ParseLinkHeader(string header)
        {
            JObject linkHeader = new JObject();
            MatchCollection matchCollection = LinkHeadersRegex.Matches(header);
            for (int i = 0; i < matchCollection.Count; ++i)
            {
                Match match1 = LinkHeaderRegex.Match(matchCollection[i].Value);
                if (match1.Success)
                {
                    JObject jobject = new JObject();
                    jobject["target"] = (JToken)match1.Groups[1].Value;
                    string input = match1.Groups[2].Value;
                    Match match2;
                    while ((match2 = LinkHeaderParamsRegex.Match(input)).Success)
                        jobject[match2.Groups[1].Value] = (JToken)(match2.Groups[2].Success ? match2.Groups[2] : (Capture)match2.Groups[3]).Value;
                    JToken jtoken1;
                    if (jobject.TryGetValue("rel", out jtoken1))
                    {
                        string propertyName = jtoken1.Value<string>();
                        JToken jtoken2;
                        if (linkHeader.TryGetValue(propertyName, out jtoken2))
                        {
                            if (jtoken2.Type == JTokenType.Array)
                                ((JArray)jtoken2).Add(jobject);
                            else
                                linkHeader[propertyName] = new JArray(new object[2]
                                {
                   jtoken2,
                   jobject
                                });
                        }
                        else
                            linkHeader[propertyName] = jobject;
                    }
                }
            }
            return linkHeader;
        }

        protected void ResolveContextUrls(JToken context, string baseUrl)
        {
            if (context == null || context.Type != JTokenType.Object)
                return;
            JToken jtoken1 = context["@context"];
            if (jtoken1.Type == JTokenType.String)
                context["@context"] = (JToken)Utils.PrependBase(baseUrl, jtoken1.Value<string>());
            else if (jtoken1.Type == JTokenType.Array)
            {
                JArray jarray = (JArray)jtoken1;
                for (int index = 0; index < jarray.Count; ++index)
                {
                    JToken jtoken2 = jarray[index];
                    if (jtoken2.Type == JTokenType.String)
                        jarray[index] = (JToken)Utils.PrependBase(baseUrl, jtoken2.Value<string>());
                    else if (jtoken2.Type == JTokenType.Object)
                        ResolveContextUrls(new JObject()
                        {
                            ["@context"] = jtoken2
                        }, baseUrl);
                }
            }
            else
            {
                if (jtoken1.Type != JTokenType.Object)
                    return;
                foreach (JToken property in ((JObject)jtoken1).Properties())
                    ResolveContextUrls(property, baseUrl);
            }
        }

        protected ResolvedContext CacheResolvedContext(
          string key,
          ResolvedContext resolved,
          string tag = null)
        {
            OperationCache[key] = resolved;
            if (tag != null)
            {
                Dictionary<string, ResolvedContext> dictionary;
                if (!SharedCache.TryGetValue(key, out dictionary))
                {
                    dictionary = new Dictionary<string, ResolvedContext>();
                    SharedCache[key] = dictionary;
                }
                dictionary[tag] = resolved;
            }
            return resolved;
        }
    }
}
