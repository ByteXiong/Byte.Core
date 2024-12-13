using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Byte.Core.Entity
{

    /// <summary>
    /// 系统作业信息表
    /// </summary>
    [SugarTable("JobDetail", "系统作业信息表")]
    public partial class JobDetail : BaseEntity<long>
    {


        /// <summary>
        /// 组名称
        /// </summary>
        [SugarColumn(ColumnDescription = "名称", Length = 128, IsNullable = true)]
        public string GroupName { get; set; }

        /// <summary>
        /// 作业类型FullName
        /// </summary>
        [SugarColumn(ColumnDescription = "作业类型", Length = 128)]
        public string JobType { get; set; }

        /// <summary>
        /// 程序集Name
        /// </summary>
        [SugarColumn(ColumnDescription = "程序集", Length = 128)]
        
        public string AssemblyName { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [SugarColumn(ColumnDescription = "描述信息", Length = 128)]
        
        public string Description { get; set; }

        /// <summary>
        /// 是否并行执行
        /// </summary>
        [SugarColumn(ColumnDescription = "是否并行执行")]
        public bool Concurrent { get; set; } = true;

        /// <summary>
        /// 是否扫描特性触发器
        /// </summary>
        [SugarColumn(ColumnDescription = "是否扫描特性触发器", ColumnName = "IncludeAnnotation")]
        public bool IncludeAnnotation { get; set; } = false;

        /// <summary>
        /// 额外数据
        /// </summary>
        [SugarColumn(ColumnDescription = "额外数据", ColumnDataType = StaticConfig.CodeFirst_BigString , IsNullable = true)]
        public string Props { get; set; }


        /// <summary>
        /// 作业创建类型
        /// </summary>
        [SugarColumn(ColumnDescription = "作业创建类型")]
        public JobCreateTypeEnum Type { get; set; }

        /// <summary>
        /// 脚本代码
        /// </summary>
        [SugarColumn(ColumnDescription = "脚本代码", ColumnDataType = StaticConfig.CodeFirst_BigString, IsNullable = true)]
        public string ScriptCode { get; set; }

        /// <summary>
        /// 集群Id
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public long ClusterId { get; set; }
        #region 导航



        /// <summary>
        ///  集群
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.ManyToOne, nameof(ClusterId), nameof(JobCluster.Id))]
        public JobCluster Cluster { get; set; }

        /// <summary>
        /// 触发器
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(JobTrigger.JobId), nameof(Id))]
        public List<JobTrigger> Triggers { get; set; }

        #endregion
    }
}