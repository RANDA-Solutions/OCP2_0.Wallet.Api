// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.ExpandContext
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    public class ExpandContext
    {
        public Dictionary<string, JObject> Mappings { get; set; } = new Dictionary<string, JObject>();

        public Dictionary<string, bool> Protected { get; set; } = new Dictionary<string, bool>();

        public Dictionary<string, JToken> Fields { get; set; } = new Dictionary<string, JToken>();

        public object Inverse { get; set; }

        public ExpandContext PreviousContext { get; set; }

        public object CreateInverseContext() => throw new NotImplementedException();

        public ExpandContext CloneActiveContext()
        {
            ExpandContext expandContext = new ExpandContext();
            foreach (KeyValuePair<string, JObject> mapping in Mappings)
                expandContext.Mappings[mapping.Key] = (JObject)mapping.Value.DeepClone();
            expandContext.Inverse = null;
            foreach (KeyValuePair<string, bool> keyValuePair in Protected)
                expandContext.Protected[keyValuePair.Key] = keyValuePair.Value;
            if (PreviousContext != null)
                expandContext.PreviousContext = PreviousContext.CloneActiveContext();
            JToken jtoken1;
            if (Fields.TryGetValue("@base", out jtoken1))
                expandContext.Fields["@base"] = jtoken1.DeepClone();
            JToken jtoken2;
            if (Fields.TryGetValue("@language", out jtoken2))
                expandContext.Fields["@language"] = jtoken2.DeepClone();
            JToken jtoken3;
            if (Fields.TryGetValue("@vocab", out jtoken3))
                expandContext.Fields["@vocab"] = jtoken3.DeepClone();
            return expandContext;
        }

        public ExpandContext RevertToPreviousContext()
        {
            return PreviousContext == null ? this : PreviousContext.CloneActiveContext();
        }
    }
}
