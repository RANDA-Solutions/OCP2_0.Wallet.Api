using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenCredentialPublisher.Shared.Converters.Newtonsoft
{
#nullable disable
        public class SingleOrArrayConverter<T> : JsonConverter
        {
            public override bool CanConvert(System.Type objectType)
            {
                return objectType == typeof(List<T>) || objectType.IsArray;
            }

            public override object ReadJson(
              JsonReader reader,
              System.Type objectType,
              object existingValue,
              JsonSerializer serializer)
            {
                JToken jtoken1 = JToken.Load(reader);
                if (typeof(T).IsAbstract)
                {
                    List<T> objList = new List<T>();
                    if (jtoken1.Type == JTokenType.Object)
                        objList.Add(serializer.Deserialize<T>(jtoken1.CreateReader()));
                    if (jtoken1.Type == JTokenType.Array)
                    {
                        foreach (JToken jtoken2 in jtoken1 as JArray)
                            objList.Add(serializer.Deserialize<T>(jtoken2.CreateReader()));
                    }
                    if (objList.Count > 0)
                        return objectType.IsArray ? (object)objList.ToArray() : (object)objList;
                }
                if (jtoken1.Type == JTokenType.Array)
                {
                    List<T> objList = jtoken1.ToObject<List<T>>();
                    return objectType.IsArray ? (object)objList.ToArray() : (object)objList;
                }
                List<T> objList1 = new List<T>()
      {
        jtoken1.ToObject<T>()
      };
                return objectType.IsArray ? (object)objList1.ToArray() : (object)objList1;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value.GetType().IsArray)
                {
                    T[] objArray = (T[])value;
                    if (objArray.Length == 1)
                        value = (object)objArray[0];
                }
                else
                {
                    List<T> objList = (List<T>)value;
                    if (objList.Count == 1)
                        value = (object)objList[0];
                }
                serializer.Serialize(writer, value);
            }
        }
    }

