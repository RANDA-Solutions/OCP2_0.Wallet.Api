// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.Extensions.CompactJwsExtensions
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;

namespace OpenCredentialPublisher.Shared.Extensions
{
  public static class CompactJwsExtensions
  {
    public static T DeserializePayload<T>(
      this string signedPayload,
      bool ignoreDeserializationError = false)
      where T : class
    {
      MatchCollection source = Regex.Matches(signedPayload, "^(?<header>[A-Za-z0-9-_]{4,})\\.(?<payload>[-A-Za-z0-9-_]{4,})\\.(?<signature>[A-Za-z0-9-_]{4,})$");
      if (!((IEnumerable<Match>) source).Any<Match>((Func<Match, bool>) (m => m.Groups.ContainsKey("payload"))))
        return default (T);
      string str = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(((IEnumerable<Match>) source).FirstOrDefault<Match>((Func<Match, bool>) (m => m.Groups.ContainsKey("payload"))).Groups["payload"].Value));
      try
      {
        return TWJson.Deserialize<T>(str);
      }
      catch
      {
        if (ignoreDeserializationError)
          return default (T);
        throw;
      }
    }
  }
}
