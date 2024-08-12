using AspectCore.DynamicProxy;
using Autofac;
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

namespace Byte.Core.Common.Attributes.RedisAttribute
{
    //public class HasRedisAttribute : BaseActionFilter
    //{
    //    readonly bool _isAll;
    //    readonly string _key;
    //    readonly string _findKey;

    //    public HasRedisAttribute(string key, string findKey = "", int expireSeconds = -1, bool isAll = false)
    //    {
    //        _isAll = isAll;
    //        _key = key;
    //        _findKey = findKey;
    //    }
    //    /// <summary>
    //    /// 方法执行前
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns></returns>
    //    public override async Task OnActionExecuting(ActionExecutingContext context)
    //    {
    //        RedisHelper.SetAsync(_key, _findKey, _isAll);
    //          // 获取方法参数并进行修改
    //    if (context.ActionArguments.ContainsKey("param1"))
    //    {
    //        context.ActionArguments["param1"] = "ModifiedValue";
    //    }

    //    if (context.ActionArguments.ContainsKey("param2"))
    //    {
    //        context.ActionArguments["param2"] = 999;
    //    }
    //        await Task.CompletedTask;
    //    }
    //}

    //public enum HasRedisRange { 
    //    /// <summary>
    //    /// 全查询
    //    /// </summary>
    //    All = 1,
    //    /// <summary>
    //    /// 指定查询
    //    /// </summary>
    //    Specific = 2,




    //}


    
//public class MyInterceptor : IInterceptor
//    {
//        public void OnBeforeInvoke(MethodInfo method, object[] args, object target)
//        {
//            // 在方法调用之前执行一些操作
//            Console.WriteLine("Before: {0}", method.Name);
//        }

//        public void OnAfterInvoke(MethodInfo method, object[] args, object target, object result)
//        {
//            // 在方法调用之后执行一些操作
//            Console.WriteLine("After: {0}", method.Name);
//        }

//        public void OnException(MethodInfo method, object[] args, object target, Exception exception)
//        {
//            // 如果方法调用抛出异常，则执行一些操作
//            Console.WriteLine("Exception: {0}", exception);
//        }

//        public void Intercept(IInvocation invocation)
//        {
//            throw new NotImplementedException();
//        }
//    }

    public class CustomInterceptorAttribute : AbstractInterceptorAttribute
    {
        private readonly string _name;

        public CustomInterceptorAttribute(string name)
        {
            _name = name;
        }


        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            //var impType = context.Implementation.GetType();
            //var properties = impType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            //    .Where(p => p.IsDefined(typeof(CustomInterceptorAttribute))).ToList();
            //if (properties.Any())
            //{
            //    context
            //    Console.WriteLine("进");
            //}
            context.Parameters[0] = "修改后的参数";
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
                }
            }
            else
            {
                // 修改返回结果
                context.ReturnValue = $"修改后的结果: {returnValue}";
            }
        }

        private void BeforeMethodExecution(AspectContext context)
        {
            // 这里添加进入方法前的逻辑
            Console.WriteLine("Before service call");
        }

        private void AfterMethodExecution(AspectContext context)
        {
            // 这里添加方法调用后的逻辑
            Console.WriteLine("After service call");
        }

        private void OnException(AspectContext context, Exception ex)
        {
            // 这里添加异常处理逻辑
            Console.WriteLine("Service threw an exception: " + ex.Message);
        }
    }

}
