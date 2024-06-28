namespace Byte.Core.Common.Models
{
    public class ExcutedResult
    {
        public bool success { get; set; }

        public string msg { get; set; }

        public object data { get; set; }

        public int? code { get; set; }

        public ExcutedResult(bool success, string msg, object data, int? code = 0)
        {
            this.success = success;
            this.msg = msg;
            this.data = data;
            this.code = code;
        }
        public static ExcutedResult SuccessResult()
        {
            return new ExcutedResult(true, null, null);
        }
        public static ExcutedResult SuccessResult(string msg = null)
        {
            return new ExcutedResult(true, msg, null);
        }
        public static ExcutedResult SuccessResult(object data)
        {
            return new ExcutedResult(true, null, data);
        }

        public static ExcutedResult SuccessResult(object data = null, string msg = null)
        {
            return new ExcutedResult(true, msg, data);
        }

        public static ExcutedResult FailedResult(string msg, int? code = 400)
        {
            return new ExcutedResult(false, msg, null, code);
        }
        public static ExcutedResult FailedResult(bool success=false, object data = null, string msg = null, int? code = 400)
        {
            return new ExcutedResult(false, msg, data, code);
        }
    }

    public class PaginationResult : ExcutedResult
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount => total % pageSize == 0 ? total / pageSize : total / pageSize + 1;

        public PaginationResult(bool success, string msg, object data) : base(success, msg, data)
        {
        }

        public static PaginationResult PagedResult(object data, int total, int size, int index)
        {
            return new PaginationResult(true, null, data)
            {
                total = total,
                pageSize = size,
                pageIndex = index
            };
        }


    }
}
