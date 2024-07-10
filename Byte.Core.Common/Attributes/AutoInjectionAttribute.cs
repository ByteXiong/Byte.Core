﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Common.Attributes
{
    /// <summary>
    /// 自动注入规则（只能注释类/接口，不允许相同标签，不允许继承）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class AutoInjectionAttribute : Attribute
    {
        public AutoInjectionAttribute()
        {
        }

        public AutoInjectionAttribute(bool autoRegister)
        {
            AutoRegister = autoRegister;
        }

        public AutoInjectionAttribute(ServiceLifetime lifecycle)
        {
            Lifecycle = lifecycle;
        }

        /// <summary>
        /// 生命周期类型。默认：瞬态（Transient）
        /// </summary>
        public ServiceLifetime Lifecycle { get; set; } = ServiceLifetime.Transient;

        /// <summary>
        /// 是否自动注册。默认：是
        /// </summary>
        public bool AutoRegister { get; set; } = true;

    }
}
