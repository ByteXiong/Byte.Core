using Autofac.Extras.DynamicProxy;
using Byte.Core.Common.Attributes.RedisAttribute;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace Byte.Core.Common.Attributes.InterceptorAttribute
{
    public class CustomCacheInterceptor : IInterceptor
    {
        private string _cacheKey;
        private bool _isDb = false;
        private int _expireSeconds = -1;

        public void Intercept(IInvocation invocation)
        {
            try
            {
                var method = invocation.MethodInvocationTarget;
                var interceptAttributes = method.GetCustomAttributes(typeof(RedisInterceptAttribute), true);

                if (interceptAttributes.Length > 0)
                {
                    var redisInterceptAttribute = (RedisInterceptAttribute)interceptAttributes[0];
                    _cacheKey = redisInterceptAttribute.GetGetFieldValue("_cacheKey").ToString();
                    _isDb = redisInterceptAttribute.GetGetFieldValue("_isDb").ToBool();
                    _expireSeconds = redisInterceptAttribute.GetGetFieldValue("_expireSeconds").ToInt();

                    var returnType = invocation.Method.ReturnType;
                    var genericArgument = returnType.GetGenericArguments()[0];

                    var cacheResult = GetCacheResult(genericArgument);
                    if (cacheResult != null)
                    {
                        invocation.ReturnValue = ConvertToReturnType(cacheResult, returnType);
                    }
                    else
                    {
                        invocation.Proceed();
                        SetCacheResult(invocation, genericArgument);
                    }
                }
                else
                {
                    invocation.Proceed();
                }
            }
            catch (Exception ex)
            {
                // 记录异常日志
                Console.WriteLine($"Error in CustomCacheInterceptor: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private object GetCacheResult(Type genericArgument)
        {
            try
            {
                var methodGet = GetType().GetMethod(nameof(Get), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (methodGet == null)
                {
                    throw new InvalidOperationException("Method 'Get' not found.");
                }

                var genericMethod = methodGet.MakeGenericMethod(genericArgument);
                return genericMethod.Invoke(this, new object[] { _cacheKey });
            }
            catch (TargetInvocationException tie)
            {
                // 捕获并处理反射调用的内部异常
                Console.WriteLine($"TargetInvocationException in GetCacheResult: {tie.InnerException?.Message}\n{tie.InnerException?.StackTrace}");
                throw;
            }
            catch (Exception ex)
            {
                // 记录异常日志
                Console.WriteLine($"Error in GetCacheResult: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void SetCacheResult(IInvocation invocation, Type genericArgument)
        {
            try
            {
                var returnType = invocation.Method.ReturnType;
                if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var setMethod = GetType().GetMethod(nameof(Set), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (setMethod == null)
                    {
                        throw new InvalidOperationException("Method 'Set' not found.");
                    }

                    var genericSetMethod = setMethod.MakeGenericMethod(genericArgument);

                    var resultProperty = invocation.ReturnValue.GetType().GetProperty("Result");
                    var result = resultProperty.GetValue(invocation.ReturnValue);
                    genericSetMethod.Invoke(this, new object[] { _cacheKey, result });
                }
            }
            catch (TargetInvocationException tie)
            {
                // 捕获并处理反射调用的内部异常
                Console.WriteLine($"TargetInvocationException in SetCacheResult: {tie.InnerException?.Message}\n{tie.InnerException?.StackTrace}");
                throw;
            }
            catch (Exception ex)
            {
                // 记录异常日志
                Console.WriteLine($"Error in SetCacheResult: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private object ConvertToReturnType(object result, Type returnType)
        {
            var taskResultType = returnType.GetGenericArguments()[0];
            return typeof(Task).GetMethod(nameof(Task.FromResult))
                               .MakeGenericMethod(taskResultType)
                               .Invoke(null, new[] { result });
        }

        private T Get<T>(string key)
        {
            return RedisHelper.Get<T>(key);
        }

        private void Set<T>(string cacheKey, T data)
        {
            if (data != null)
            {
                RedisHelper.Set(cacheKey, data, _expireSeconds);
            }
        }
    }
}