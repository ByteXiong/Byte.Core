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
}
