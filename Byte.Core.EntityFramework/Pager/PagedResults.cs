namespace Byte.Core.EntityFramework.IDbContext
{
    //
    // 摘要:
    //     分页查询结果
    public class PagedResults<T>
    {
        //
        // 摘要:
        //     分页信息
        public PagerInfo PagerInfo
        {
            get;
            set;
        }

        //
        // 摘要:
        //     数据
        public IList<T> Data
        {
            get;
            set;
        }

        //
        // 摘要:
        //     根据传入的转换方法将分页结果转换为指定类型的分页结果
        //
        // 类型参数:
        //   TResult:
        public PagedResults<TResult> ConvertTo<TResult>(Func<T, TResult> converter)
        {
            return new PagedResults<TResult>
            {
                PagerInfo = PagerInfo,
                Data = Data.Select(converter).ToArray()
            };
        }
    }
}
