using SqlSugar;

namespace Byte.Core.SqlSugar.Cache
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public class SqlSugarCsRedisCache : ICacheService
    {

        //注意:SugarRedis 不要扔到构造函数里面， 一定要单例模式  

        public void Add<V>(string key, V value)
        {
            RedisHelper.Set(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            RedisHelper.Set(key, value, cacheDurationInSeconds);
        }

        public bool ContainsKey<V>(string key)
        {
            return RedisHelper.Exists(key);
        }

        public V Get<V>(string key)
        {
            return RedisHelper.Get<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            //性能注意: 只查sqlsugar用到的key 
            return RedisHelper.Keys("cache:SqlSugarDataCache.*");//个人封装问题，key前面会带上cache:缓存前缀，请根据实际情况自行修改 ，如果没有去掉前缀或者使用最下面.如果不确定，可以用最下面前面都加通配符的做法
                                                                 // return RedisHelper.Keys("SqlSugarDataCache.*");
                                                                 // return RedisHelper.Keys("*SqlSugarDataCache.*");
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {

            if (this.ContainsKey<V>(cacheKey))
            {
                var result = this.Get<V>(cacheKey);
                if (result == null)
                {
                    return create();
                }
                else
                {
                    return result;
                }
            }
            else
            {
                var result = create();
                this.Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        public void Remove<V>(string key)
        {
            RedisHelper.Del(key);
        }
    }
}
