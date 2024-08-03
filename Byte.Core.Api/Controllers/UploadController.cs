using Asp.Versioning;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Filters;
using Byte.Core.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Controllers
{
    /// <summary>
    /// 上传
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class UploadController : BaseApiController
    {


        public UploadController()
        {
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        [ApiVersion("2.0", Deprecated = false)]
        public async Task<List<string>> FilesAsync()
        {

            IFormFileCollection cols = Request.Form.Files;
            if (cols == null || cols.Count == 0)
            {
                new BusException("没有上传文件");
            };
            List<string> list = new List<string>();
            foreach (IFormFile file in cols)
            {
                ////定义图片数组后缀格式
                //string[] LimitPictureType = { ".JPG", ".JPEG", ".GIF", ".PNG", ".BMP" };
                ////获取图片后缀是否存在数组中
                string currentPictureExtension = Path.GetExtension(file.FileName).ToLower();
                //if (LimitPictureType.Contains(currentPictureExtension))
                //{
                //为了查看图片就不在重新生成文件名称了
                //var new_path = DateTime.Now.ToString("yyyyMMdd")+ file.FileName;
                var new_path = Path.Combine("upload/files/" + DateTime.Now.ToString("yyyyMMdd") + "/");
                var pathUel = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", new_path);
                if (!Directory.Exists(pathUel))
                    Directory.CreateDirectory(pathUel);
                var ps = DateTime.Now.ToTimeStamp();
                var path = Path.Combine(pathUel, ps + currentPictureExtension);

                using (var stream = new FileStream(path, FileMode.Create))
                {

                    //图片路径保存到数据库里面去
                    bool flage = true;//  QcnApplyDetm.FinancialCreditQcnApplyDetmAdd(EntId, CrtUser, new_path);
                    if (flage == true)
                    {
                        //再把文件保存的文件夹中
                        file.CopyTo(stream);
                        list.Add("/" + new_path + ps + currentPictureExtension);
                    }
                }
                //}
                //else

                //    return ExcutedResult.FailedResult(msg: "请上传指定格式的图片");

            }
            return list;
        }


    }
}
