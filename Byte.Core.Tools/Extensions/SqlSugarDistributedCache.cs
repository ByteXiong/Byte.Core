﻿using Byte.Core.Common.Cache;
using Byte.Core.Common.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SqlSugar;
using System.Text;

namespace Byte.Core.Tools.Extensions;

/// <summary>
/// Distributed缓存
/// 实现SqlSugar.ICacheService
/// </summary>
public class SqlSugarDistributedCache : ICacheService
{



    private IDistributedCache _cache;
    public SqlSugarDistributedCache(IDistributedCache cache)
    {
        _cache = cache;
    }


    public void Add<V>(string key, V value)
    {
        var valStr = value.ToJson();
        _cache.SetString(key, valStr);
    }

    public void Add<V>(string key, V value, int cacheDurationInSeconds)
    {
        var valStr = value.ToJson();
        DistributedCacheEntryOptions op = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDurationInSeconds),
        };
        _cache.SetString(key, valStr, op);
    }

    public bool ContainsKey<V>(string key)
    {
        return _cache.Get(key) != null;
    }

    public V Get<V>(string key)
    {
        var val = _cache.GetString(key);
        if (val == null) return default(V);
        return JsonConvert.DeserializeObject<V>(val);
    }

    public IEnumerable<string> GetAllKey<V>()
    {
        //throw new NotImplementedException("IDistributedCache不支持模糊查询KEY");
        //实现思路 创建一个缓存区。然后添加缓存时，把key增加到这个缓存区
        //每次获取时都需要检查一下缓存是否存在 不存在则删除
        //这个获取所有key 获取这个定义得缓存区就行了 
        return new[] { "" };
    }

    public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
    {
        if (ContainsKey<V>(cacheKey))
        {
            return Get<V>(cacheKey);
        }

        V val = create();
        Add(cacheKey, val, cacheDurationInSeconds);
        return val;
    }

    public void Remove<V>(string key)
    {
        _cache.Remove(key);
    }
}
