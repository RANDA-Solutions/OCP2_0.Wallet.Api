// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.IriRelativeTo
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable enable
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal record IriRelativeTo()
    {
        public static readonly
#nullable disable
        IriRelativeTo BaseSet = new IriRelativeTo()
        {
            Base = true
        };
        public static readonly IriRelativeTo VocabSet = new IriRelativeTo()
        {
            Vocab = true
        };
        public static readonly IriRelativeTo BothSet = new IriRelativeTo()
        {
            Base = true,
            Vocab = true
        };
        public bool Base;
        public bool Vocab;

        [CompilerGenerated]
        protected virtual bool PrintMembers(
#nullable enable
        StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("Base = ");
            builder.Append(Base.ToString());
            builder.Append(", Vocab = ");
            builder.Append(Vocab.ToString());
            return true;
        }

        [CompilerGenerated]
        public override int GetHashCode()
        {
            return (EqualityComparer<System.Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(Base)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(Vocab);
        }

        [CompilerGenerated]
        public virtual bool Equals(IriRelativeTo? other)
        {
            if (this == (object)other)
                return true;
            return (object)other != null && EqualityContract == other.EqualityContract && EqualityComparer<bool>.Default.Equals(Base, other.Base) && EqualityComparer<bool>.Default.Equals(Vocab, other.Vocab);
        }

        [CompilerGenerated]
        protected IriRelativeTo(IriRelativeTo original)
        {
            Base = original.Base;
            Vocab = original.Vocab;
        }
    }
}
