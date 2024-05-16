namespace Byte.Core.SqlSugar
{
    //
    // 摘要:
    //     查询参数的基类
    public abstract class DatePageParam : PageParam
    {
        public DateTime? StartDate
        {
            get;
            set;
        }

        public DateTime? EndDate
        {
            get;
            set;
        }

        //protected TimePageParam()
        //{
        //    StartTime = DateTime.MinValue;
        //    EndTime = DateTime.MaxValue;
        //}
    }
}
