﻿using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Byte.Core.Common.Attributes;
using Byte.Core.Common.Cache;
using Byte.Core.Common.Helpers;
using Byte.Core.Common.IoC;
using Byte.Core.Common.Web;
using CSRedis;
using Exceptionless.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Byte.Core.Common.Extensions
{
    /// <summary>
    /// IServiceCollection扩展
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// 用DI批量注入接口程序集中对应的实现类。
        /// <para>
        /// 需要注意的是，这里有如下约定：
        /// IUserService --> UserService, IUserRepository --> UserRepository.
        /// </para>
        /// </summary>
        /// <param name="service"></param>
        /// <param name="interfaceAssemblyName">接口程序集的名称（不包含文件扩展名）</param>
        /// <returns></returns>
        public static IServiceCollection AddTransientAssembly(this IServiceCollection service, string interfaceAssemblyName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(interfaceAssemblyName))
                throw new ArgumentNullException(nameof(interfaceAssemblyName));

            var assembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (assembly == null)
            {
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            }

            //过滤掉非接口及泛型接口
            var types = assembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsGenericType);

            foreach (var type in types)
            {
                var implementTypeName = type.Name.Substring(1);
                var implementType = RuntimeHelper.GetImplementType(implementTypeName, type);
                if (implementType != null)
                    service.AddTransient(type, implementType);
            }
            return service;
        }
        public static IServiceCollection AddScopedAssembly(this IServiceCollection service, string interfaceAssemblyName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(interfaceAssemblyName))
                throw new ArgumentNullException(nameof(interfaceAssemblyName));

            var assembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (assembly == null)
            {
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            }

            //过滤掉非接口及泛型接口
            var types = assembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsGenericType);

            foreach (var type in types)
            {
                var implementTypeName = type.Name.Substring(1);
                var implementType = RuntimeHelper.GetImplementType(implementTypeName, type);
                if (implementType != null)
                    service.AddScoped(type, implementType);
            }
            return service;
        }
        public static IServiceCollection AddSingletonAssembly(this IServiceCollection service, string interfaceAssemblyName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(interfaceAssemblyName))
                throw new ArgumentNullException(nameof(interfaceAssemblyName));

            var assembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (assembly == null)
            {
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            }

            //过滤掉非接口及泛型接口
            var types = assembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsGenericType);

            foreach (var type in types)
            {
                var implementTypeName = type.Name.Substring(1);
                var implementType = RuntimeHelper.GetImplementType(implementTypeName, type);
                if (implementType != null)
                    service.AddSingleton(type, implementType);
            }
            return service;
        }
        /// <summary>
        /// 用DI批量注入接口程序集中对应的实现类。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="interfaceAssemblyName">接口程序集的名称（不包含文件扩展名）</param>
        /// <param name="implementAssemblyName">实现程序集的名称（不包含文件扩展名）</param>
        /// <returns></returns>
        public static IServiceCollection AddScopedAssembly(this IServiceCollection service, string interfaceAssemblyName, string implementAssemblyName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(interfaceAssemblyName))
                throw new ArgumentNullException(nameof(interfaceAssemblyName));
            if (string.IsNullOrEmpty(implementAssemblyName))
                throw new ArgumentNullException(nameof(implementAssemblyName));

            var interfaceAssembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);

            if (interfaceAssembly == null)
            {
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            }

            var implementAssembly = RuntimeHelper.GetAssembly(implementAssemblyName);
            if (implementAssembly == null)
            {
                throw new DllNotFoundException($"the dll \"{implementAssemblyName}\" not be found");
            }

            //过滤掉非接口及泛型接口
            var types = interfaceAssembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsGenericType);
            var str = "";
            foreach (var type in types)
            {
                //过滤掉抽象类、泛型类以及非class
                var implementType = implementAssembly.DefinedTypes
                    .FirstOrDefault(t => t.IsClass && !t.IsAbstract && !t.IsGenericType &&
                                         t.GetInterfaces().Any(b => b.Name == type.Name));


                if (implementType != null)
                {
                    service.AddScoped(type, implementType.AsType());
                }
            }
            Console.WriteLine(str);
            return service;
        }
        public static IServiceCollection AddTransientAssembly(this IServiceCollection service, string interfaceAssemblyName, string implementAssemblyName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(interfaceAssemblyName))
                throw new ArgumentNullException(nameof(interfaceAssemblyName));
            if (string.IsNullOrEmpty(implementAssemblyName))
                throw new ArgumentNullException(nameof(implementAssemblyName));

            var interfaceAssembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (interfaceAssembly == null)
            {
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            }

            var implementAssembly = RuntimeHelper.GetAssembly(implementAssemblyName);
            if (implementAssembly == null)
            {
                throw new DllNotFoundException($"the dll \"{implementAssemblyName}\" not be found");
            }

            //过滤掉非接口及泛型接口
            var types = interfaceAssembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsGenericType);

            foreach (var type in types)
            {
                //过滤掉抽象类、泛型类以及非class
                var implementType = implementAssembly.DefinedTypes
                    .FirstOrDefault(t => t.IsClass && !t.IsAbstract && !t.IsGenericType &&
                                         t.GetInterfaces().Any(b => b.Name == type.Name));
                if (implementType != null)
                {
                    service.AddTransient(type, implementType.AsType());
                }
            }

            return service;
        }
        public static IServiceCollection AddSingletonAssembly(this IServiceCollection service, string interfaceAssemblyName, string implementAssemblyName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(interfaceAssemblyName))
                throw new ArgumentNullException(nameof(interfaceAssemblyName));
            if (string.IsNullOrEmpty(implementAssemblyName))
                throw new ArgumentNullException(nameof(implementAssemblyName));

            var interfaceAssembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (interfaceAssembly == null)
            {
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            }

            var implementAssembly = RuntimeHelper.GetAssembly(implementAssemblyName);
            if (implementAssembly == null)
            {
                throw new DllNotFoundException($"the dll \"{implementAssemblyName}\" not be found");
            }

            //过滤掉非接口及泛型接口
            var types = interfaceAssembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsGenericType);

            foreach (var type in types)
            {
                //过滤掉抽象类、泛型类以及非class
                var implementType = implementAssembly.DefinedTypes
                    .FirstOrDefault(t => t.IsClass && !t.IsAbstract && !t.IsGenericType &&
                                         t.GetInterfaces().Any(b => b.Name == type.Name));
                if (implementType != null)
                {


                    service.AddSingleton(type, implementType.AsType());
                }
            }

            return service;
        }

        public static IServiceCollection RegisterControllers(this IServiceCollection service,
            string controllerAssemblyName)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(controllerAssemblyName))
                throw new ArgumentNullException(nameof(controllerAssemblyName));
            var controllerAssembly = RuntimeHelper.GetAssembly(controllerAssemblyName);
            if (controllerAssembly == null)
            {
                throw new DllNotFoundException($"the dll \"{controllerAssemblyName}\" not be found");
            }

            //过滤掉非接口及泛型接口
            var types = controllerAssembly.GetTypes().Where(t =>
            {
                var typeInfo = t.GetTypeInfo();
                return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType && t.IsAssignableFrom(typeof(Controller));
            });

            foreach (var type in types)
            {

                service.AddScoped(type);
            }

            return service;
        }

        /// <summary>
        /// 自动注册程序集
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dllNames"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoServices(this IServiceCollection services,  string dllNames)
        {
            // 根据名称获取程序集
            var assemblies = RuntimeHelper.GetAssembly(dllNames);
            // 获取程序集内名称以 Service 结尾的 class
            var serviceTypes = assemblies.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Distinct();

            // 遍历，将服务默认注册为瞬态服务（生命周期：Transient）
            foreach (var serviceType in serviceTypes)
            {
                // 注册自身
                RegistrationType(services, serviceType, serviceType);

                // 注册所有实现的实例（!Problem：子类也会实现父类的接口，可能导致父类对接口的实现被覆盖）
                var serviceInterfaces = serviceType.GetInterfaces();
                foreach (var serviceInterface in serviceInterfaces)
                    RegistrationType(services, serviceInterface, serviceType);
            }

            return services;

            // AddAutoServices() 内部静态函数
            static void RegistrationType(IServiceCollection services, Type serviceType, Type implementationType)
            {

                // 设置默认生命周期为 Transient
                var lifecyleType = ServiceLifetime.Transient;

                // 获取服务自动注入标签（AutoInject）
                var autoInjection = serviceType.GetCustomAttribute<AutoInjectionAttribute>();
                if (autoInjection != null)
                {
                    if (!autoInjection.AutoRegister) return;
                    lifecyleType = autoInjection.Lifecycle;
                }

                // 注册服务
                switch (lifecyleType)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(serviceType, implementationType);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(serviceType, implementationType);
                        break;
                    case ServiceLifetime.Transient:
                    default:
                        services.AddTransient(serviceType, implementationType);
                        break;
                }
            }
        }
        /// <summary>
        /// 使用CSRedis代替StackChange.Redis
        /// <remarks>
        /// 关于CSRedis项目，请参考<seealso cref="https://github.com/2881099/csredis"/>
        /// </remarks>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="redisConnectionStrings">redis连接字符串。
        /// <remarks>
        /// 如果是单机模式，请只输入一个连接字符串；如果是集群模式，请输入多个连接字符串
        /// </remarks>
        /// </param>
        /// <returns></returns>
        public static IServiceCollection UseCsRedisClient(this IServiceCollection services, params string[] redisConnectionStrings)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (redisConnectionStrings == null || redisConnectionStrings.Length == 0)
            {
                throw new ArgumentNullException(nameof(redisConnectionStrings));
            }
            CSRedisClient csredis = ((redisConnectionStrings.Length != 1) ? new CSRedisClient((Func<string, string>)null, redisConnectionStrings) : new CSRedisClient(redisConnectionStrings[0]));
            RedisHelper<RedisHelper>.Initialization(csredis);
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper<RedisHelper>.Instance));
            services.AddSingleton<IDistributedCacheManager, DistributedCacheManager>();
            return services;
        }
        public static IServiceProvider BuildAutofacServiceProvider(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            return ServiceLocator.Current = AutofacContainer.Build(services);
        }
        public static IServiceProvider BuildAspectCoreWithAutofacServiceProvider(this IServiceCollection services, Action<IAspectConfiguration> configure = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.ConfigureDynamicProxy(configure);
            return ServiceLocator.Current = AutofacContainer.Build(services, configure);
        }

        public static IServiceContext BuildAspectCoreServiceContainer(this IServiceCollection services,
            Action<IAspectConfiguration> configure = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddAspectServiceContext();
            services.ConfigureDynamicProxy(configure);
            return services.ToServiceContext();
        }

        public static IServiceProvider BuildAspectCoreServiceProvider(this IServiceCollection services,
            Action<IAspectConfiguration> configure = null)
        {
            return ServiceLocator.Current = AspectCoreContainer.BuildServiceProvider(services, configure);
        }

        /// <summary>
        /// 添加自定义Controller。自定义controller项目对应的dll必须复制到程序运行目录
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="controllerAssemblyName">自定义controller文件的名称，比如：xxx.Controllers.dll</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IMvcBuilder AddCustomController(this IMvcBuilder builder, string controllerAssemblyName,
            Func<TypeInfo, bool> filter = null)
        {
            if (filter == null)
                filter = m => true;
            return builder.ConfigureApplicationPartManager(m =>
            {
                var feature = new ControllerFeature();
                m.ApplicationParts.Add(new AssemblyPart(Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + controllerAssemblyName)));
                m.PopulateFeature(feature);
                builder.Services.AddSingleton(feature.Controllers.Where(filter).Select(t => t.AsType()).ToArray());
            });
        }
        /// <summary>
        /// 添加自定义Controller
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="controllerAssemblyDir">Controller文件所在路径</param>
        /// <param name="controllerAssemblyName">Controller文件名称，比如：xxx.Controllers.dll</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IMvcBuilder AddCustomController(this IMvcBuilder builder, string controllerAssemblyDir, string controllerAssemblyName,
            Func<TypeInfo, bool> filter = null)
        {
            if (filter == null)
                filter = m => true;
            return builder.ConfigureApplicationPartManager(m =>
            {
                var feature = new ControllerFeature();
                m.ApplicationParts.Add(
                    new AssemblyPart(Assembly.LoadFile(Path.Combine(controllerAssemblyDir, controllerAssemblyName))));
                m.PopulateFeature(feature);
                builder.Services.AddSingleton(feature.Controllers.Where(filter).Select(t => t.AsType()).ToArray());
            });
        }

        public static IServiceCollection AddDefaultWebContext(this IServiceCollection services)
        {
            return services.AddSingleton<IWebContext, WebContext>();
        }

        public static IServiceCollection AddWebContext<T>(this IServiceCollection services) where T : WebContext
        {
            services.Remove(new ServiceDescriptor(typeof(IWebContext), typeof(WebContext), ServiceLifetime.Singleton));
            return services.AddSingleton<IWebContext, T>();
        }

        /// <summary>
        /// 框架入口。默认开启注入实现了ISingletonDependency、IScopedDependency、ITransientDependency三种不同生命周期的类，以及AddHttpContextAccessor和AddDataProtection。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="aspectConfig"></param>
        /// <returns></returns>
        public static IServiceProvider AddCoreX(this IServiceCollection services, Action<IServiceCollection> config = null, Action<IAspectConfiguration> aspectConfig = null)
        {
            config?.Invoke(services);
            services.RegisterServiceLifetimeDependencies();
            services.AddHttpContextAccessor();
            services.AddDataProtection();
            services.AddDefaultWebContext();
            return services.BuildAspectCoreServiceProvider(aspectConfig);
        }

    }
}

