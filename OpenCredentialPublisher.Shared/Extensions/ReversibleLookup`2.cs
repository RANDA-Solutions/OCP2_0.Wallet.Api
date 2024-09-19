// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.Extensions.ReversibleLookup`2
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace OpenCredentialPublisher.Shared.Extensions
{
  public class ReversibleLookup<T1, T2> : ReadOnlyDictionary<
  #nullable disable
  T1, T2>
    where T1 : 
    #nullable enable
    notnull
    where T2 : notnull
  {
    private readonly 
    #nullable disable
    System.Collections.Generic.Dictionary<T2, T1> reverseLookup = new System.Collections.Generic.Dictionary<T2, T1>();

    public ReversibleLookup(params (T1, T2)[] mappings)
      : base((IDictionary<T1, T2>) new System.Collections.Generic.Dictionary<T1, T2>())
    {
      this.ReverseLookup = new ReadOnlyDictionary<T2, T1>((IDictionary<T2, T1>) this.reverseLookup);
      foreach ((T1, T2) mapping in mappings)
        this.Add(mapping.Item1, mapping.Item2);
    }

    public ReadOnlyDictionary<T2, T1> ReverseLookup { get; }

    [DebuggerHidden]
    public void Add(T1 value1, T2 value2)
    {
      if (this.ContainsKey(value1))
        throw new InvalidOperationException("value1 is not unique");
      if (this.ReverseLookup.ContainsKey(value2))
        throw new InvalidOperationException("value2 is not unique");
      this.Dictionary.Add(value1, value2);
      this.reverseLookup.Add(value2, value1);
    }

    public void Clear()
    {
      this.Dictionary.Clear();
      this.reverseLookup.Clear();
    }
  }
}
