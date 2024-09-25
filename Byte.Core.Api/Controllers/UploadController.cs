using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Filters;
using Byte.Core.Tools;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Byte.Core.Api.Controllers
{
    /// <summary>
    /// 上传
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class UploadController : BaseApiController
    {


        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion((double)VersionEnum.Pc, Deprecated = false)]
        [ApiVersion((double)VersionEnum.App, Deprecated = false)]
        public async Task<List<string>> ImagesAsync()
        {

            IFormFileCollection cols = Request.Form.Files;
            if (cols == null || cols.Count == 0)
            {
                throw new BusException("没有上传文件");
            }

            List<string> list = new List<string>();
            foreach (IFormFile file in cols)
            {
                ////定义图片数组后缀格式
                string[] limitPictureType = [".jpg", ".jpeg", ".gif", ".png"];
                ////获取图片后缀是否存在数组中
                string currentPictureExtension = Path.GetExtension(file.FileName).ToLower();
                if (!limitPictureType.Contains(currentPictureExtension))
                {
                    throw new BusException("格式有误,仅支持jpg、jpeg、gif、png格式");
                }
                //为了查看图片就不在重新生成文件名称了
                //var new_path = DateTime.Now.ToString("yyyyMMdd")+ file.FileName;
                var newPath = Path.Combine("upload/images/" + DateTime.Now.ToString("yyyyMMdd") + "/");
                var pathUel = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newPath);
                if (!Directory.Exists(pathUel))
                    Directory.CreateDirectory(pathUel);
                var ps =  DateTime.Now.ToTimeStamp();
                var path = Path.Combine(pathUel, ps + currentPictureExtension);

                int quality = 90;
                // 使用当前质量压缩图像  
                byte[] imageData = await FileReadAllBytesAsync(file.OpenReadStream());
                // 压缩图片  
                byte[] compressedImageData = CompressImage(imageData, quality);


                #region 反复压缩
                // 假设我们有一个目标文件大小（以字节为单位）  
                long targetSizeBytes = 100 * 1024; // 例如，100KB  

                // 初始压缩质量（可以根据需要调整）  
                long estimatedSizeBytes = imageData.Length;

                // 循环以估算合适的压缩质量  
                while (estimatedSizeBytes > targetSizeBytes && quality > 10) // 假设最小质量为10  
                {
                    // 压缩图片  
                    compressedImageData = CompressImage(imageData, quality);

                    // 降低压缩质量以尝试减小文件大小  
                    quality -= 5; // 每次迭代降低5个质量点  
                }
                #endregion

                await SaveCompressedImageAsync(compressedImageData, path);

                // 保存压缩后的图片到文件系统  
                list.Add("/" + newPath + ps + currentPictureExtension);
            }
            return list;


            byte[] CompressImage(byte[] imageData, int quality)
            {
                using var image = Image.Load(imageData);
                var jpegEncoder = new JpegEncoder { Quality = quality };

                using var memoryStream = new MemoryStream();
                image.SaveAsJpeg(memoryStream, jpegEncoder);
                return memoryStream.ToArray();
            }
            async Task<byte[]> FileReadAllBytesAsync(Stream stream)
            {
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }

            async Task SaveCompressedImageAsync(byte[] compressedImageData, string filePath)
            {
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await fileStream.WriteAsync(compressedImageData, 0, compressedImageData.Length);
            }
        }

    }
}
