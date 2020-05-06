using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleWebApi.Extends
{
    /// <summary>
    /// 将长整型数据转为字符串
    /// 否则id=1202054749357081610 会变成id = 1.2020547493570816E+18
    /// </summary>
    public class LongToString : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(long).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jt = JValue.ReadFrom(reader);
            return jt.Value<long>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}