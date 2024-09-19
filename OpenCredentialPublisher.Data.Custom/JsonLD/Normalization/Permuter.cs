// Decompiled with JetBrains decompiler
// Type: OpenCredentialPublisher.Credentials.JsonLd.Normalization.Permuter
// Assembly: OpenCredentialPublisher.Credentials, Version=2.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 88B6179C-54BB-4AC8-A653-96A586457798
// Assembly location: C:\Users\jlcj1\.nuget\packages\opencredentialpublisher.credentials\2.2.1\lib\netcoreapp3.1\OpenCredentialPublisher.Credentials.dll

#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCredentialPublisher.Data.Custom.JsonLD.Normalization
{
    internal class Permuter
    {
        private List<string> current;
        private bool done;
        private Dictionary<string, bool> dir;

        public Permuter(List<string> list)
        {
            current = list.OrderBy(s => s).ToList();
            done = false;
            dir = new Dictionary<string, bool>();
            for (int index = 0; index < list.Count; ++index)
                dir[list[index]] = true;
        }

        public bool HasNext() => !done;

        public List<string> Next()
        {
            List<string> list = current.ToList();
            string str1 = null;
            int index1 = 0;
            int count = current.Count;
            for (int index2 = 0; index2 < count; ++index2)
            {
                string str2 = current[index2];
                bool flag = dir[str2];
                if ((str1 == null || string.Compare(str2, str1) > 0) && (flag && index2 > 0 && string.Compare(str2, current[index2 - 1]) > 0 || !flag && index2 < count - 1 && string.Compare(str2, current[index2 + 1]) > 0))
                {
                    str1 = str2;
                    index1 = index2;
                }
            }
            if (str1 == null)
            {
                done = true;
            }
            else
            {
                int index3 = dir[str1] ? index1 - 1 : index1 + 1;
                current[index1] = current[index3];
                current[index3] = str1;
                foreach (string str3 in current)
                {
                    if (string.Compare(str3, str1) > 0)
                        dir[str3] = !dir[str3];
                }
            }
            return list;
        }
    }
}
