using Byte.Core.Common.Extensions;
using Byte.Core.Common.Pager;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System.Linq.Expressions;

namespace Byte.Core.SqlSugar
{

    public static class LinqExt
    {


   
        public static async Task<PagedResults<TOut>> ToPagedResultsAsync<TOut>(this ISugarQueryable<TOut> source, PageParam queryParam)
        {

            if (queryParam.StartIndex < 0)
            {
                throw new InvalidOperationException("起始记录数不能小于0");
            }

            if (queryParam.PageSize <= 0)
            {
                throw new InvalidOperationException("每页记录数不能小于0");
            }

            var pagerInfo = new PagerInfo(queryParam);

            if (queryParam.SortList != null && queryParam.SortList.Count > 0) {
                source = source.OrderBy(string.Join(",", queryParam.SortList.Select(x => $"{x.Key} {x.Value}")));
            }
            RefAsync<int> totalCount = 0;
            var data = await source.ToPageListAsync(pagerInfo.PageIndex, pagerInfo.PageSize, totalCount);
            pagerInfo.TotalRowCount = totalCount;
            if (queryParam.StartIndex >= pagerInfo.TotalRowCount)
            {
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != 0) ? pagerInfo.TotalRowCount : 0);
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != -1) ? pagerInfo.StartIndex : 0);
            }

            return new PagedResults<TOut>
            {
                PagerInfo = pagerInfo,
                Data = data
            };
        }


        public static  async Task<T> FirstOrDefaultAsync<T>(this ISugarQueryable<T> queryable, Expression<Func<T, bool>> whereLambda=null)
        {
            if (whereLambda == null) {
                return await queryable.SingleAsync();
            }
            else
            {
                return await queryable.SingleAsync(whereLambda);
            }
      
        }

    }
}

