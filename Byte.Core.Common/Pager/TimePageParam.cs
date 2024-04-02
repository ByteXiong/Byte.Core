namespace Byte.Core.Common.Pager
{
    //
    // 摘要:
    //     查询参数的基类
    public abstract class TimePageParam : PageParam
    {
        public DateTime StartTime
        {
            get;
            set;
        }

        public DateTime EndTime
        {
            get;
            set;
        }

        protected TimePageParam()
        {
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MaxValue;
        }
    }
}
