// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.Exceptions.JsonLdParseException
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using System;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization.Exceptions
{
    public class JsonLdParseException : Exception
    {
        public string Line { get; set; }

        public int LineNumber { get; set; }

        public JsonLdParseException(string message, Exception innerException = null)
          : base(message, innerException)
        {
            Line = null;
            LineNumber = -1;
        }

        public JsonLdParseException(string line, int lineNumber)
          : base(string.Format("N-Quads parse error on line {0}.", lineNumber))
        {
            Line = line;
            LineNumber = lineNumber;
        }
    }
}
