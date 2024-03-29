﻿using Byte.Core.EntityFramework.Models;

namespace Byte.Core.EntityFramework.Options
{
    public class DbContextOption
    {
        public DatabaseType TagName { get; set; }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 实体程序集名称
        /// </summary>
        public string ModelAssemblyName { get; set; }
        /// <summary>
        /// 是否在控制台输出SQL语句，默认开启
        /// </summary>
        public bool IsOutputSql { get; set; } = true;

        public string DatabaseName { get; set; }


    }
}
