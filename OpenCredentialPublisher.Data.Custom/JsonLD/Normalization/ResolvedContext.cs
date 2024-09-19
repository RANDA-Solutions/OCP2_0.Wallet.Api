// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.ResolvedContext
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    public class ResolvedContext
    {
        public JToken Document { get; }

        protected Dictionary<ExpandContext, ExpandContext> Cache { get; set; } = new Dictionary<ExpandContext, ExpandContext>();

        public ResolvedContext(JToken document) => Document = document;

        public ExpandContext GetProcessed(ExpandContext activeCtx)
        {
            ExpandContext processed;
            Cache.TryGetValue(activeCtx, out processed);
            return processed;
        }

        public void SetProcessed(ExpandContext activeCtx, ExpandContext processedCtx)
        {
            Cache[activeCtx] = processedCtx;
        }
    }
}
