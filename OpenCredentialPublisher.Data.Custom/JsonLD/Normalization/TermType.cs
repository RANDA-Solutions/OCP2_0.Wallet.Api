// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.TermType
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using OpenCredentialPublisher;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal class TermType
    {
        public static TermType NamedNode = new TermType(nameof(NamedNode));
        public static TermType BlankNode = new TermType(nameof(BlankNode));
        public static TermType Literal = new TermType(nameof(Literal));
        public static TermType DefaultGraph = new TermType(nameof(DefaultGraph));

        public string Value { get; set; }

        protected TermType(string value) => Value = value;
    }
}
