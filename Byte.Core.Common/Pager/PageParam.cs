﻿namespace Byte.Core.Common.Pager
{
    public class PageParam
    {
        public int StartIndex => (PageIndex - 1) * PageSize;

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

     
        public string SortName
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