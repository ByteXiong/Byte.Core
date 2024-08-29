using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Common.Attributes.RedisAttribute
{

    public class RedisInterceptorAttribute : AbstractInterceptorAttribute
    {
        readonly HasRedisRange _range;
        readonly string _cacheKey;
        bool isDb = false; //是否走缓存

        public RedisInterceptorAttribute(string cacheKey, int expireSeconds = -1, HasRedisRange range = HasRedisRange.All)
        {
            _range = range;
            _cacheKey = cacheKey;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            Type returnType = context.ServiceMethod.ReturnType;

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                Type genericArgument = returnType.GetGenericArguments()[0];

                MethodInfo method = this.GetType().GetMethod("GetAsync", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                MethodInfo genericMethod = method.MakeGenericMethod(genericArgument);

                var task = (Task)genericMethod.Invoke(this, new object[] { _cacheKey });
                await task.ConfigureAwait(true);

                var resultProperty = task.GetType().GetProperty("Result");
                var dictResult = resultProperty.GetValue(task);

                if (dictResult != null)
                {
                   
                    context.ReturnValue = Task.FromResult(dictResult);
                    // 调用被拦截的方法并获取结果
                    await context.Invoke(next);

                }
                else
                {
                    await next(context);
                }
            }
            else
            {
                await next(context);
            }
            // 如果没有提前返回，则继续执行下一个拦截器或目标方法
            await next(context);
        }

        async Task<T> GetAsync<T>(string key)
        {
            var result = await RedisHelper.GetAsync<T>(key);
            return result;
        }
    }
}
