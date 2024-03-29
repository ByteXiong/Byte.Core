using Byte.Core.Common.IoC;
using Microsoft.Extensions.Caching.Memory;

namespace Byte.Core.Common.Cache
{
    public class MemoryCacheManager
    {
        public static IMemoryCache GetInstance() => ServiceLocator.Resolve<IMemoryCache>();
    }
}
