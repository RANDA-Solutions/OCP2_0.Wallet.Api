// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.ExpandOptions
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using OpenCredentialPublisher;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    public class ExpandOptions
    {
        private IContextResolver contextResolver;

        public IContextResolver ContextResolver
        {
            get
            {
                if (contextResolver == null)
                    contextResolver = new ContextResolver();
                return contextResolver;
            }
            set => contextResolver = value;
        }

        public string Base { get; set; } = null;

        public string ProtectedMode { get; set; } = null;

        public bool IsFrame { get; set; } = false;

        public bool KeepFreeFloatingNodes { get; set; } = false;
    }
}
