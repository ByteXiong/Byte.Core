using Byte.Core.Common.Extensions;
using Byte.Core.Common.Pager;
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

            var pagerInfo = new PagerInfo(queryParam)
            {
                TotalRowCount = source.Count()
            };
            if (queryParam.StartIndex >= pagerInfo.TotalRowCount)
            {
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != 0) ? pagerInfo.TotalRowCount : 0);
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != -1) ? pagerInfo.StartIndex : 0);
            }


            source = source.OrderByIF(queryParam.SortList.Count > 0, string.Join(",", queryParam.SortList .Select(x => $"{x.Key} {x.Value}")));

            var data = await source.ToPageListAsync(pagerInfo.PageIndex, pagerInfo.PageSize, pagerInfo.TotalRowCount);
            return new PagedResults<TOut>
            {
                PagerInfo = pagerInfo,
                Data = data
            };
        }

    }
}

