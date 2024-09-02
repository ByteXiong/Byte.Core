using AspectCore.DynamicProxy;
using Newtonsoft.Json;
using System.Reflection;

namespace Byte.Core.Common.Attributes.RedisAttribute
{

    /// <summary>
    /// Json Redis缓存
    /// </summary>
    public class RedisInterceptAttribute : Attribute
    {
        readonly string _cacheKey;
        readonly bool _isDb = false; //是否走缓存
        readonly int _expireSeconds = -1;


        public RedisInterceptAttribute(string cacheKey, int expireSeconds = -1,bool isDb=false)
        {
            _cacheKey = cacheKey;
            _isDb = isDb;
            _expireSeconds = expireSeconds;
        }
    }
}
