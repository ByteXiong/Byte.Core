namespace Byte.Core.Common.Pager
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

        private string _sort;
        public string SortOrderDesc
        {
            get => _sort;

            set
            {
                if (value != null)
                {
                    if (value.Contains("asc"))
                    {

                        _sort = "ASC";
                    }
                    if (value.Contains("desc"))
                    {

                        _sort = "DESC";
                    }
                }
            }
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
        public IDictionary<string, string> Props { get; set; }


        public PageParam()
        {
            PageSize = 10;
            PageIndex = 1;
            SortOrderDesc = "Desc";
        }
    }
}
