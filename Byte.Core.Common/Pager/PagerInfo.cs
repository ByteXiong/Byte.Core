namespace Byte.Core.Common.Pager
{
    //
    // 摘要:
    //     分页信息
    public class PagerInfo
    {
        //
        // 摘要:
        //     获取或设置总记录数
        public int TotalRowCount
        {
            get;
            set;
        }= 0;

        //
        // 摘要:
        //     获取或设置每页记录数
        public int PageSize
        {
            get;
            set;
        }

        //
        // 摘要:
        //     获取或设置起始记录（从0开始）
        public int StartIndex
        {
            get;
            set;
        }

        //
        // 摘要:
        //     获取当前页码（从1开始）
        public int PageIndex
        {
            get
            {
                int num = ComputePageIndex(StartIndex + 1, PageSize);
                if (num > TotalPageCount)
                {
                    num = TotalPageCount;
                }

                return num <= 0 ? 1 : num;
            }
        }

        //
        // 摘要:
        //     是否有下一页
        public bool HasPrev => PageIndex < TotalPageCount;

        //
        // 摘要:
        //     是否有上一页
        public bool HasNext => PageIndex > 1;

        //
        // 摘要:
        //     是否是第一页
        public bool IsFirst => PageIndex == 1;

        //
        // 摘要:
        //     是否是最后一页
        public bool IsLast => PageIndex == TotalPageCount;

        //
        // 摘要:
        //     获取记录的总页数
        public int TotalPageCount => ComputePageIndex(TotalRowCount, PageSize);

        //
        // 摘要:
        //     根据查询参数初始化分页信息
        //
        // 参数:
        //   queryParam:
        public PagerInfo(PageParam queryParam)
        {
            PageSize = queryParam.PageSize;
            StartIndex = queryParam.StartIndex;
        }

        //
        // 摘要:
        //     计算页码
        //
        // 参数:
        //   total:
        //
        //   pageSize:
        private static int ComputePageIndex(int total, int pageSize)
        {
            int result;
            int num = Math.DivRem(total, pageSize, out result);
            if (result > 0)
            {
                num++;
            }

            return num;
        }
    }
}
