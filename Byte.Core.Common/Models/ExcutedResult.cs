using NPOI.SS.Formula.Functions;

namespace Byte.Core.Common.Models
{
    public class ExcutedResult
    {
        public bool success { get; set; }

        public string msg { get; set; }

        public int? code { get; set; }



        public ExcutedResult()
        {
            this.success = success;
            this.msg = msg;
            this.code = code;
        }
        public static ExcutedResult SuccessResult()
        {
            return new ExcutedResult() { 
                success = true,
                msg = null,
                code = 0

            };
        }
        public static ExcutedResult SuccessResult(string msg)
        {
            return new ExcutedResult()
            {
                success = true,
                msg = msg,
                code = 0

            };
        }


        //public static ExcutedResult SuccessResult(object data, string msg = null)
        //{
        //    return new ExcutedResult()
        //    {
        //        success = true,
        //        msg = null,
        //        code = 0,
        //        data = data
        //    };
        //}

        public static ExcutedResult FailedResult(string msg, int? code = 400)
        {
             return new ExcutedResult() { 
              code = code,
              msg = msg,
              success = false

             };
        }
    

    }

    /// <summary>
    /// 配合swagger 输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
      public class ExcutedResult<T> : ExcutedResult
        {

        public T data { get; set; }
        public static ExcutedResult<T> SuccessResult(T data, string msg = null)
        {
            return new ExcutedResult<T>() { 
             code = 0,
             msg = msg,
             success = true,
             data = data
            };
        }
        public static ExcutedResult<T> FailedResultt(T data,bool success=false,  string msg = null, int? code = 400)
        {
            return new ExcutedResult<T>()
            {
                code = code,
                msg = msg,
                success = true,
                data = data
            };
        }
    }

}
