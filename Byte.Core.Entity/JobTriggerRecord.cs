using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Byte.Core.Entity
{

    /// <summary>
    /// 系统作业触发器运行记录表
    /// </summary>
    [SugarTable("JobTriggerRecord", "系统作业触发器运行记录表")]
    public partial class JobTriggerRecord : BaseEntity<long>
    {
        

        /// <summary>
        /// 当前运行次数
        /// </summary>
        [SugarColumn(ColumnDescription = "当前运行次数")]
        public long NumberOfRuns { get; set; }

        /// <summary>
        /// 最近运行时间
        /// </summary>
        [SugarColumn(ColumnDescription = "最近运行时间")]
        public DateTime? LastRunTime { get; set; }

        /// <summary>
        /// 下一次运行时间
        /// </summary>
        [SugarColumn(ColumnDescription = "下一次运行时间")]
        public DateTime? NextRunTime { get; set; }

        /// <summary>
        /// 触发器状态
        /// </summary>
        [SugarColumn(ColumnDescription = "触发器状态")]
        public TriggerStateEnum Status { get; set; }

        /// <summary>
        /// 本次执行结果
        /// </summary>
        [SugarColumn(ColumnDescription = "本次执行结果", Length = 128, IsNullable = true)]
        [MaxLength(128)]
        public string Result { get; set; }

        /// <summary>
        /// 本次执行耗时
        /// </summary>
        [SugarColumn(ColumnDescription = "本次执行耗时")]
        public int ElapsedTime { get; set; }

        /// <summary>
        /// 触发器Id
        /// </summary>
        public long TriggerId { get; set; }
        #region 导航

        /// <summary>
        ///  触发器
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.ManyToOne, nameof(TriggerId), nameof(JobTrigger.Id))]
        public JobTrigger Trigger { get; set; }

        #endregion
    }
}