
namespace Byte.Core.SqlSugar.ConfigOptions;
/// <summary>
/// 缓存
/// </summary>
public class CacheConfig
    {
        /// <summary>
        /// 分布式
        /// </summary>
        public DistributedCacheSwitch DistributedCacheSwitch { get; set; }
        /// <summary>
        /// redis
        /// </summary>
        public RedisCacheSwitch RedisCacheSwitch { get; set; }
    }

    #region 缓存
    /// <summary>
    /// 分布式缓存
    /// </summary>
    public class DistributedCacheSwitch
    {
        public bool Enabled { get; set; }
    }
    /// <summary>
    /// redis
    /// </summary>
    public class RedisCacheSwitch
    {
        public bool Enabled { get; set; }
    }
    #endregion