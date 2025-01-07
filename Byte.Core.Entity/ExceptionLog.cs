﻿using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 系统异常日志
    /// </summary>
    [Tenant(AppConfig.Tenant)]
    [SplitTable(SplitType.Month)]
    [SugarTable($@"{"log_exception"}_{{year}}{{month}}{{day}}")]
    public class ExceptionLog : BaseEntity<long>
    {
        /// <summary>
        /// 区域
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Area { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Controller { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Action { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Method { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 请求url
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [SugarColumn(ColumnDataType = "longtext,text,clob", IsNullable = true)]
        public string RequestParameters { get; set; }

        /// <summary>
        /// 异常短信息
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// 异常完整信息
        /// </summary>
        [SugarColumn(ColumnDataType = "longtext,text,clob", IsNullable = true)]
        public string ExceptionMessageFull { get; set; }

        /// <summary>
        /// 异常堆栈信息
        /// </summary>
        [SugarColumn(ColumnDataType = "longtext,text,clob", IsNullable = true)]
        public string ExceptionStack { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// 请求ip
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string RequestIp { get; set; }

        /// <summary>
        /// ip所属真实地址
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string IpAddress { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string OperatingSystem { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string DeviceType { get; set; }

        /// <summary>
        /// 浏览器名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string BrowserName { get; set; }

        /// <summary>
        /// 浏览器版本
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Version { get; set; }

       
    }

}
