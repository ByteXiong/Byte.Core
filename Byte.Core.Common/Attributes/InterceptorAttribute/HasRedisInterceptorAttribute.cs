using AspectCore.DynamicProxy;
using Autofac;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static NPOI.HSSF.UserModel.HeaderFooter;

namespace Byte.Core.Common.Attributes.RedisAttribute
{
    public class HasRedisInterceptorAttribute : AbstractInterceptorAttribute
    {
        readonly HasRedisRange _range;
        readonly string _cacheKey;
        readonly string _findKey;
        bool isDb = false; //是否走缓存
        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="key"></param>
        /// <param name="findKey"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="range"></param>

        public HasRedisInterceptorAttribute(string cacheKey, string findKey = "", int expireSeconds = -1, HasRedisRange range = HasRedisRange.All)
        {
            _range = range;
            _cacheKey = cacheKey;
            _findKey = findKey;
        }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            Type returnType = context.ServiceMethod.ReturnType;

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                Type genericArgument = returnType.GetGenericArguments()[0];

                if (genericArgument.IsGenericType && genericArgument.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type listGenericArgument = genericArgument.GetGenericArguments()[0];

                    MethodInfo method = this.GetType().GetMethod("HMGetAsync", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    MethodInfo genericMethod = method.MakeGenericMethod(listGenericArgument);


                    // 获取 Role 类中带有 FindKey 特性的属性名
                    var fields = listGenericArgument.GetProperties()
                        .Where(prop => Attribute.IsDefined(prop, typeof(FindKeyAttribute)))
                        .Select(prop => prop.Name)
                        .ToArray();

                    var task = (Task)genericMethod.Invoke(this, new object[] { _cacheKey, fields });
                    await task.ConfigureAwait(true);

                    var resultProperty = task.GetType().GetProperty("Result");
                    var dictResult = resultProperty.GetValue(task) as Dictionary<string, object>;

                    var listResult = new List<object>();
                    foreach (var item in dictResult.Values)
                    {
                        listResult.Add(item);
                    }

                    context.ReturnValue = Task.FromResult(listResult);
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
        }

      async Task<T[]> HMGetAsync<T>(string key, string[] fields)
        {
           var result =await  RedisHelper.HMGetAsync<T>(key, fields);
            return result;
        }
    }


    public enum HasRedisRange
    {

        /// <summary>
        /// 全查询
        /// </summary>
        All = 1,
        /// <summary>
        /// 指定查询
        /// </summary>
        Params = 2,

        /// <summary>
        /// 指定查询
        /// </summary>
        Find = 3
    }

    /// <summary>
    /// 哈希键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Parameter)]
    public class FindKeyAttribute : Attribute
    {

    }
    /// <summary>
    /// 数组键
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FindsKeyAttribute : Attribute
    {
    }



}
