// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.QuadItem
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using OpenCredentialPublisher;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal class QuadItem
    {
        public const string RDF = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        public const string RDF_LANGSTRING = "http://www.w3.org/1999/02/22-rdf-syntax-ns#langString";
        public const string XSD = "http://www.w3.org/2001/XMLSchema#";
        public const string XSD_STRING = "http://www.w3.org/2001/XMLSchema#string";

        public TermType TermType { get; set; }

        public string Value { get; set; }

        public override string ToString() => TermType?.Value + ": " + Value;
    }
}
