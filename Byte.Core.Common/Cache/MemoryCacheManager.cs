using Byte.Core.Common.Helpers;
using Byte.Core.Common.IoC;
using MathNet.Numerics.Distributions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Logging;
using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Byte.Core.Common.Cache
{
    public static class MemoryCacheManager
    {
        public static IMemoryCache _memoryCache => ServiceLocator.Resolve<IMemoryCache>();
        //private static readonly MemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            return _memoryCache.TryGetValue(key, out _);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="seconds">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="secondsAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public static bool Set<T>(string key, T value, int seconds, int secondsAbsoulte)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _memoryCache.Set<T>(key, value,
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(seconds))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(secondsAbsoulte)));
            return Exists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="secondsIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public static bool Set<T>(string key, T value, int secondsIn, bool isSliding = false)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _memoryCache.Set<T>(key, value,
                isSliding
                    ? new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(secondsIn))
                    : new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(secondsIn)));

            return Exists(key);
        }

        #region 删除缓存

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static void Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _memoryCache.Remove(key);
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <returns></returns>
        public static void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            keys.ToList().ForEach(item => _memoryCache.Remove(item));
        }
        #endregion

        #region 获取缓存

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return _memoryCache.Get(key) as T;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static object Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return _memoryCache.Get(key);
        }

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public static IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            var dict = new Dictionary<string, object>();
            keys.ToList().ForEach(item => dict.Add(item, _memoryCache.Get(item)));
            return dict;
        }
        #endregion

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public static void RemoveCacheAll()
        {
            var l = GetCacheKeys();
            foreach (var s in l)
            {
                Remove(s);
            }
        }

        /// <summary>
        /// 删除匹配到的缓存
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static void RemoveCacheRegex(string pattern)
        {
            IList<string> l = SearchCacheRegex(pattern);
            foreach (var s in l)
            {
                Remove(s);
            }
        }

        /// <summary>
        /// 搜索 匹配到的缓存
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IList<string> SearchCacheRegex(string pattern)
        {
            var cacheKeys = GetCacheKeys();
            var l = cacheKeys.Where(k => Regex.IsMatch(k, pattern)).ToList();
            return l.AsReadOnly();
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCacheKeys()
        {
            
                 //8.0.0 版本
                
                 const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
                 var coherentState = _memoryCache.GetType().GetField("_coherentState", flags).GetValue(_memoryCache);

                 var entries = coherentState.GetType().GetField("_stringEntries", flags).GetValue(coherentState);
                 var cacheItems = entries as IDictionary;
                 var keys = new List<string>();
                 if (cacheItems == null) return keys;
                 foreach (DictionaryEntry cacheItem in cacheItems)
                 {
                     keys.Add(cacheItem.Key.ToString());
                 }
                 return keys;


            //7.0.0 版本
            //const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            //var coherentState = _memoryCache.GetType().GetField("_coherentState", flags).GetValue(_memoryCache);
            //var entries = coherentState.GetType().GetField("_entries", flags).GetValue(coherentState);
            //var cacheItems = entries as IDictionary;
            //var keys = new List<string>();
            //if (cacheItems == null) return keys;
            //foreach (DictionaryEntry cacheItem in cacheItems)
            //{
            //    keys.Add(cacheItem.Key.ToString());
            //}
            //return keys;
        }
    }
}
