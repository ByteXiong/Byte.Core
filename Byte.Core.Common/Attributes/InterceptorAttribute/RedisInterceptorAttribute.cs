using AspectCore.DynamicProxy;
using Newtonsoft.Json;
using System.Reflection;

namespace Byte.Core.Common.Attributes.RedisAttribute
{

    /// <summary>
    /// Json Redis缓存
    /// </summary>
    public class RedisInterceptorAttribute : AbstractInterceptorAttribute, IEquatable<RedisInterceptorAttribute>
    {
        readonly string _baseKey;
        readonly string _cacheKey;
        bool _isDb = false; //是否走缓存
        int _expireSeconds = -1;
        public RedisInterceptorAttribute(string baseKey) {
            _baseKey=baseKey;
        }

        public RedisInterceptorAttribute(string cacheKey, int expireSeconds = -1,bool isDb=false)
        {
            _cacheKey = cacheKey;
            _isDb = isDb;
            _expireSeconds = expireSeconds;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
           
            Type returnType = context.ServiceMethod.ReturnType;
            Type genericArgument = returnType.GetGenericArguments()[0];
            // 生成缓存的key
            string cacheKeyWithParams = GenerateCacheKey(context);

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                MethodInfo method = this.GetType().GetMethod("GetAsync", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                MethodInfo genericMethod = method.MakeGenericMethod(genericArgument);

                var cacheTask = (Task)genericMethod.Invoke(this, new object[] { cacheKeyWithParams });
                await cacheTask.ConfigureAwait(true);

                var resultProperty = cacheTask.GetType().GetProperty("Result");
                var cacheResult = resultProperty.GetValue(cacheTask);


                if (cacheResult != null)
                {
                    context.ReturnValue = ConvertToReturnType(cacheResult, returnType);
                    // 设置缓存的逻辑
                    await Task.CompletedTask;

                }
                else
                {
                    await context.Invoke(next);
                    // 设置缓存
                     if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        MethodInfo setMethod = this.GetType().GetMethod("SetAsync", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        MethodInfo genericSetMethod = setMethod.MakeGenericMethod(genericArgument);

                        var resultProperty1 = context.ReturnValue.GetType().GetProperty("Result");
                        var result = resultProperty1.GetValue(context.ReturnValue);
                        await (Task)genericSetMethod.Invoke(this, new object[] { cacheKeyWithParams, result });
                    }
                }
            }
            else
            {
                await next(context);
            }
        }
        private string GenerateCacheKey(AspectContext context)
        {
            var keyBuilder = new System.Text.StringBuilder(_baseKey).Append(":").Append(_cacheKey);

            foreach (var parameter in context.Parameters)
            {
                keyBuilder.Append(":").Append(FormatParameter(parameter));
            }

            return keyBuilder.ToString();
        }

        private string FormatParameter(object parameter)
        {
            return JsonConvert.SerializeObject(parameter).Replace(":", "=");
        }

        private object ConvertToReturnType(object result, Type returnType)
        {
            Type taskResultType = returnType.GetGenericArguments()[0];
            return typeof(Task).GetMethod(nameof(Task.FromResult))
                               .MakeGenericMethod(taskResultType)
                               .Invoke(null, new[] { result });
        }


        async Task<T> GetAsync<T>(string key)
        {
            var result = await RedisHelper.GetAsync<T>(key);
            return result;
        }

        async Task SetAsync<T>(string cacheKey, T data)
        {
            if (data != null)
            {
                await RedisHelper.SetAsync(cacheKey, data, _expireSeconds);
            }
        }

        public bool Equals(RedisInterceptorAttribute other)
        {
            throw new NotImplementedException();
        }
    }
}
