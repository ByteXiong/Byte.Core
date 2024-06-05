using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Common.Converters
{
    public class IntToBoolConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool) || objectType == typeof(bool?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Integer)
            {
                int value = (int)reader.Value;
                return value == 1;
            } else if (reader.TokenType == JsonToken.String) {
                var str = reader.Value.ToString().ToLower();
                if (str == "1"|| str == "true")
                    return true;
            }

            return false; // 或者你可以选择抛出异常，如果遇到了非整数的值  
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // 这里不需要写逻辑，因为我们只关心从JSON读取到bool的转换  
            //throw new NotImplementedException();
        }
    }
}
