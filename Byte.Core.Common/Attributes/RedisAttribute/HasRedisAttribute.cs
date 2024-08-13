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
            //先获取params 参数
            var parameters = context.Parameters;
            if (parameters == null) { 
               throw new Exception("参数为空");
            }
            //获取到Keys 组合
            List<string> fields = new List<string>();
            foreach (var parameter in parameters)
            {

               //获取参数类型
                var type = parameter.GetType();
                //判断是否为数组
                if (type.IsArray)
                {

                }
                else{
                    //获取值

                    fields.Add(parameter.ToString());
                }
            }
           var  list =  await  RedisHelper.HMGetAsync<>(_cacheKey, fields.ToArray());
            if (list != null)
            {
                context.ReturnValue = list;
                return;
            }





               // 调用被拦截的方法并获取结果
               await context.Invoke(next);

            // 处理返回结果
            var returnValue = context.ReturnValue;

            if (returnValue is Task taskResult)
            {
                await taskResult;

                if (taskResult.GetType().IsGenericType)
                {
                    var resultProperty = taskResult.GetType().GetProperty("Result");
                    if (resultProperty != null)
                    {
                        var resultValue = resultProperty.GetValue(taskResult);

                        // 修改返回结果
                        var newResultValue = $"修改后的结果: {resultValue}";

                        // 创建一个新的 Task 包装修改后的结果
                        var newTask = Task.FromResult(newResultValue);

                        context.ReturnValue = resultValue;
                    }
             
                // 处理返回模型上的 FindKey
                if (resultProperty != null)
                {
                    HandleFindKey(resultProperty, _cacheKey);
                }
                //RedisHelper.HSetAsync(_cacheKey, result, expirationMinutes);
                }
            }
            else
            {
                // 修改返回结果
                context.ReturnValue = $"修改后的结果: {returnValue}";
            }
         

        }

     void HandleFindKey(object result, string cacheKey)
    {
        var findKeyProperties = result.GetType().GetProperties()
            .Where(prop => Attribute.IsDefined(prop, typeof(FindKeyAttribute)))
            .ToList();

        foreach (var property in findKeyProperties)
        {
            var findKeyAttribute = property.GetCustomAttribute<FindKeyAttribute>();
            var findKeyValue = property.GetValue(result)?.ToString();
            if (!string.IsNullOrEmpty(findKeyValue))
            {
                // 处理 FindKey 逻辑，如更新缓存索引等
                //Console.WriteLine($"FindKey '{findKeyAttribute.Key}' has value '{findKeyValue}' for cache key '{cacheKey}'");
            }
        }
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
