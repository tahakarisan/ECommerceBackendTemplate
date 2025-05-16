namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        object Get(string key);
        void Add(string key, object data, int duration);
        bool IsAdd(string key);
        void Remove(string key);
        void RemoveByPattern(string pattern);
        Task<T> GetAsync<T>(string key);
        Task<object> GetAsync(string key);
        Task AddAsync(string key, object data, int duration);
        Task<bool> IsAddAsync(string key);
        Task RemoveAsync(string key);
        Task RemoveByPatternAsync(string pattern);
    }
}
