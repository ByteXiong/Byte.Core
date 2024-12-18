﻿namespace Byte.Core.SqlSugar
{
    public class PageParam
    {
        public int StartIndex => (PageIndex - 1) * PageSize;

        public Dictionary<string, Dictionary<string, string>> Search { get; set; }

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }




        public IDictionary<string, string> SortList
        {
            get;
            set;
        }


        public PageParam()
        {
            PageSize = 10;
            PageIndex = 1;
        }

    }
}
