﻿// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.Quad
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using OpenCredentialPublisher;


// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.Quad
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using OpenCredentialPublisher.Data.Custom.JsonLD.Normalization;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal class Quad
    {
        public QuadItem Subject { get; set; }

        public QuadItem Predicate { get; set; }

        public ObjectQuadItem Object { get; set; }

        public QuadItem Graph { get; set; }
    }
}
