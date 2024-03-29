using Byte.Core.Common.IoC;
using Microsoft.AspNetCore.Http;

namespace Byte.Core.Common.Web
{
    public interface IWebContext : ISingletonDependency
    {
        HttpContext CoreContext { get; }
        T GetService<T>();
    }
}
