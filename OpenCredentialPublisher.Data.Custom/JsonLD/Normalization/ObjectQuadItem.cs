// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.ObjectQuadItem
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using OpenCredentialPublisher;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal class ObjectQuadItem : QuadItem
    {
        public QuadItem DataType { get; set; }

        public string Language { get; set; }

        public ObjectQuadItem()
        {
        }

        public ObjectQuadItem(QuadItem otherItem, ObjectQuadItem otherObj = null)
        {
            TermType = otherItem.TermType;
            Value = otherItem.Value;
            DataType = otherObj?.DataType;
            Language = otherObj?.Language;
        }

        public override string ToString()
        {
            return (DataType?.Value ?? TermType?.Value) + ": " + Value;
        }
    }
}
