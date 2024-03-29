using Byte.Core.Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace Byte.Core.Common.Web
{
    public class WebContext : IWebContext
    {
        public HttpContext CoreContext { get; }

        public WebContext(IHttpContextAccessor accessor)
        {
            CoreContext = accessor?.HttpContext ?? throw new ArgumentNullException($"参数{nameof(accessor)}为null，请先在Startup.cs文件中的ConfigServices方法里使用AddHttpContextAccessor注入IHttpContextAccessor对象。");
        }

        public virtual T GetService<T>()
        {
            return CoreContext.GetService<T>();
        }
    }
}
