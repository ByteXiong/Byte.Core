﻿using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Byte.Core.Entity
{

    /// <summary>
    /// 系统作业触发器表
    /// </summary>
    [SugarTable("jobtrigger", "系统作业触发器表")]
    public partial class JobTrigger : BaseEntity<long>
    {


        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnDescription = "名称", Length = 128, IsNullable = true)]
        public string GroupName { get; set; }
        /// <summary>
        /// 触发器类型
        /// </summary>
        [SugarColumn(ColumnDescription = "触发器类型", IsNullable = true)]
        public TriggerTypeEnum TriggerType { get; set; }

        /// <summary>
        /// 程序集
        /// </summary>
        [SugarColumn(ColumnDescription = "程序集", Length = 128, IsNullable = true)]
        [MaxLength(128)]
        public string AssemblyName { get; set; } 

        /// <summary>
        /// 参数
        /// </summary>
        [SugarColumn(ColumnDescription = "参数", Length = 128, IsNullable = true)]
        [MaxLength(128)]
        public string Props { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [SugarColumn(ColumnDescription = "描述信息", Length = 128, IsNullable = true)]
        [MaxLength(128)]
        public string Description { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnDescription = "状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        [SugarColumn(ColumnDescription = "起始时间")]
        public long? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(ColumnDescription = "结束时间")]
        public long? EndTime { get; set; }

        /// <summary>
        /// 最近运行时间
        /// </summary>
        [SugarColumn(ColumnDescription = "最近运行时间")]
        public long? LastRunTime { get; set; }

        /// <summary>
        /// 下一次运行时间
        /// </summary>
        [SugarColumn(ColumnDescription = "下一次运行时间")]
        public long? NextRunTime { get; set; }

        /// <summary>
        /// 触发次数
        /// </summary>
        [SugarColumn(ColumnDescription = "触发次数")]
        public int NumberOfRuns { get; set; }

        /// <summary>
        /// 最大触发次数（0:不限制，n:N次）
        /// </summary>
        [SugarColumn(ColumnDescription = "最大触发次数")]
        public int MaxNumberOfRuns { get; set; }

        /// <summary>
        /// 出错次数
        /// </summary>
        [SugarColumn(ColumnDescription = "出错次数")]
        public int NumberOfErrors { get; set; }

        /// <summary>
        /// 最大出错次数（0:不限制，n:N次）
        /// </summary>
        [SugarColumn(ColumnDescription = "最大出错次数")]
        public int MaxNumberOfErrors { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        [SugarColumn(ColumnDescription = "重试次数")]
        public int NumRetries { get; set; }

        /// <summary>
        /// 重试间隔时间（ms）
        /// </summary>
        [SugarColumn(ColumnDescription = "重试间隔时间(ms)")]
        public int RetryTimeout { get; set; } 

        /// <summary>
        /// 是否立即启动
        /// </summary>
        [SugarColumn(ColumnDescription = "是否立即启动")]
        public bool StartNow { get; set; } 

        /// <summary>
        /// 是否启动时执行一次
        /// </summary>
        [SugarColumn(ColumnDescription = "是否启动时执行一次")]
        public bool RunOnStart { get; set; } 

        /// <summary>
        /// 是否在启动时重置最大触发次数等于一次的作业
        /// </summary>
        [SugarColumn(ColumnDescription = "是否重置触发次数")]
        public bool ResetOnlyOnce { get; set; } 


        /// <summary>
        /// 任务Id
        /// </summary>
        public long JobId { get; set; }

        #region 导航

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public TriggerStateEnum State { get; set; }
        /// <summary>
        ///  任务
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.ManyToOne, nameof(JobId), nameof(JobDetail.Id))]
        public JobDetail Detail { get; set; }

        /// <summary>
        /// 触发器
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(JobTriggerRecord.TriggerId), nameof(Id))]
        public List<JobTriggerRecord> Records { get; set; }

        #endregion
    }
}