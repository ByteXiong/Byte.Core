using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 系统作业集群表
    /// </summary>
    [SugarTable("jobcluster", "系统作业集群表")]

    public  class JobCluster  : BaseEntity<long>
    {


        /// <summary>
        /// 分组
        /// </summary>
        [SugarColumn(ColumnDescription = "分组", Length = 128)]
        public string GroupName { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [SugarColumn(ColumnDescription = "描述信息", Length = 128)]
        public string Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnDescription = "状态")]
        public ClusterStatus Status { get; set; }
        #region 导航

        /// <summary>
        /// 集群
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(JobDetail.ClusterId), nameof(Id))]
        public List<JobDetail> Details { get; set; }

        #endregion

    }
}
