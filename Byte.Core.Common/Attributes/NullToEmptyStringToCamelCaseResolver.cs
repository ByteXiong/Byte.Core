using Byte.Core.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Byte.Core.Common.Attributes
{
    public class NullToEmptyStringToCamelCaseResolver : CamelCasePropertyNamesContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties()
                .Select(p =>
                {
                    var jp = base.CreateProperty(p, memberSerialization);
                    jp.ValueProvider = new NullToEmptyStringValueProvider(p);
                    return jp;
                }).ToList();
        }


        /// <summary>
        /// 对长整型做处理
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (objectType == typeof(long) || objectType == typeof(long?))
            {
                return new JsonConverterLong();
            }

            return base.ResolveContractConverter(objectType);
        }
    }
    public class NullToEmptyStringValueProvider : Newtonsoft.Json.Serialization.IValueProvider
    {
        PropertyInfo _MemberInfo;
        public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
        {
            _MemberInfo = memberInfo;
        }
        public object GetValue(object target)
        {
            object result = _MemberInfo.GetValue(target);
            if (_MemberInfo.PropertyType == typeof(string) && result == null) result = "";
            else if (_MemberInfo.PropertyType == typeof(String[]) && result == null) result = new string[] { };
            else if (_MemberInfo.PropertyType == typeof(Nullable<Int32>) && result == null) result = 0;
            else if (_MemberInfo.PropertyType == typeof(Nullable<Decimal>) && result == null) result = 0.00M;
            else if (_MemberInfo.PropertyType == typeof(Nullable<bool>) && result == null) result = false;
            else if (_MemberInfo.PropertyType == typeof(Nullable<Boolean>) && result == null) result = false;
            //else if (_MemberInfo.PropertyType == typeof(Nullable<DateTime>) && result == null) result = DateTime.MinValue;
            //if (result == null) 
            //	result = "";
            return result;
        }
        public void SetValue(object target, object value)
        {
            _MemberInfo.SetValue(target, value);
        }
    }


    public class JsonConverterLong : JsonConverter
    {
        /// <summary>
        /// 是否可以转换
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <summary>
        /// 读json
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if ((reader.ValueType == null || reader.ValueType == typeof(long?) || reader.ValueType == typeof(string)) && (reader.Value == null || reader.Value.ToString() == ""))
            {
                return null;
            }

            long.TryParse(reader.Value != null ? reader.Value.ToString() : "", out long value);
            return value;
        }

        /// <summary>
        /// 写json
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteValue(value);
            else
                writer.WriteValue(value + "");
        }
    }
}
