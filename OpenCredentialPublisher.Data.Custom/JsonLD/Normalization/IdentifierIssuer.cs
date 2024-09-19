// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.IdentifierIssuer
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal class IdentifierIssuer
    {
        private readonly OrderedDictionary existing;

        public string Prefix { get; set; }

        public int Counter { get; private set; }

        public IdentifierIssuer(string prefix, OrderedDictionary existing = null, int counter = 0)
        {
            Prefix = prefix;
            this.existing = existing ?? new OrderedDictionary();
            Counter = counter;
        }

        public IdentifierIssuer Clone()
        {
            OrderedDictionary existing = new OrderedDictionary();
            foreach (object key in (IEnumerable)this.existing.Keys)
                existing.Add(key, this.existing[key]);
            return new IdentifierIssuer(Prefix, existing, Counter);
        }

        public string GetId(string old = null)
        {
            if (!string.IsNullOrEmpty(old))
            {
                object id = existing[old];
                if (id != null)
                    return (string)id;
            }
            string id1 = Prefix + Counter++.ToString();
            if (!string.IsNullOrEmpty(old))
                existing[old] = id1;
            return id1;
        }

        public bool HasId(string old) => existing.Contains(old);

        public IEnumerable<string> GetOldIds() => existing.Keys.Cast<string>();
    }
}
