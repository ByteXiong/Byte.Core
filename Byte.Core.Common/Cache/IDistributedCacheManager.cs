namespace Byte.Core.Common.Cache
{
    public interface IDistributedCacheManager
    {
        void Set(string key, object value);
        Task SetAsync(string key, object value);
        void Set(string key, object value, int expiredSeconds);
        Task SetAsync(string key, object value, int expiredSeconds);
        object Get(string key);
        Task<object> GetAsync(string key);
        object Get(string key, Type type);
        Task<object> GetAsync(string key, Type type);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        object Get(string key, Func<object> func);
        Task<object> GetAsync(string key, Func<object> func);
        T Get<T>(string key, Func<T> func);
        Task<T> GetAsync<T>(string key, Func<T> func);
        object Get(string key, Func<object> func, int expiredSeconds);
        Task<object> GetAsync(string key, Func<object> func, int expiredSeconds);
        T Get<T>(string key, Func<T> func, int expiredSeconds);
        Task<T> GetAsync<T>(string key, Func<T> func, int expiredSeconds);
        Task Del(string key);
        Task DelAsync(string key);
        Task<bool> Exists(string key);
        Task<bool> ExistsAsync(string key);
    }
}
