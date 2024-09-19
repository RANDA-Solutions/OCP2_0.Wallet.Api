// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.Extensions.TWJson
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Extensions
{
  public class TWJson
  {
    public static T Deserialize<T>(string value) where T : class
    {
      if (string.IsNullOrEmpty(value))
        return default (T);
      return JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions()
      {
        DefaultIgnoreCondition = (JsonIgnoreCondition) 3
      });
    }

    public static JsonSerializerOptions IgnoreNulls
    {
      get
      {
        return new JsonSerializerOptions()
        {
          DefaultIgnoreCondition = (JsonIgnoreCondition) 3
        };
      }
    }
  }
}
