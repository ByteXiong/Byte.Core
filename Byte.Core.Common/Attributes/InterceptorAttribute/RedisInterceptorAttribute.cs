using AspectCore.DynamicProxy;
using Newtonsoft.Json;
using System.Reflection;

namespace Byte.Core.Common.Attributes.RedisAttribute
{

    /// <summary>
    /// Json Redis缓存
    /// </summary>
    public class RedisInterceptorAttribute : AbstractInterceptorAttribute
    { 
        readonly string _cacheKey;
        bool _isDb = false; //是否走缓存
        int _expireSeconds = -1;
     

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
                //通过类型获取缓存方法,并且调用 GetAsync 方法 
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
            var keyBuilder = new System.Text.StringBuilder(_cacheKey);

            foreach (var parameter in context.Parameters)
            {
                keyBuilder.Append(":").Append(FormatParameter(parameter));
            }

            return keyBuilder.ToString();
        }

        /// <summary>
        /// 格式化参数(类似url过滤一下数据,: 会影响阅读)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string FormatParameter(object parameter)
        {
            
            return JsonConvert.SerializeObject(parameter).Replace(":", "=").Replace("{", "").Replace("}", "").Replace("\"", "").Replace(",", "&");
        }
        /// <summary>
        /// 将返回值转换为原有类型
        /// </summary>
        /// <param name="result"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
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

    }
}
