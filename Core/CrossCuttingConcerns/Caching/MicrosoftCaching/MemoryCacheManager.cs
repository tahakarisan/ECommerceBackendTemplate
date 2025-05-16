using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.RegularExpressions;
namespace Core.CrossCuttingConcerns.Caching.MicrosoftCaching
{
    public class MemoryCacheManager : ICacheManager
    {
        private IMemoryCache _memoryCache;

        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }

        #region Sync
        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public void Add(string key, object data, int duration)
        {
            _memoryCache.Set(key, data, TimeSpan.FromMinutes(duration));
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string patterns)
        {
            // Desenleri ayır
            string[] patternList = patterns.Split(',');

            // EntriesCollection erişimi için Reflection kullanımı
            FieldInfo? coherentStateField = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
            object? coherentState = coherentStateField?.GetValue(_memoryCache);
            #region Microsoft.Extensions.Caching.Memory_6_OR_OLDER
            string entriesPropNameV6 = "EntriesCollection";
            #endregion
            #region Microsoft.Extensions.Caching.Memory_7_OR_NEWER
            string entriesPropNameV7 = "StringEntriesCollection";
            #endregion
            PropertyInfo? entriesCollectionProperty = coherentState?.GetType().GetProperty(entriesPropNameV7, BindingFlags.NonPublic | BindingFlags.Instance);
            dynamic? cacheEntriesCollection = entriesCollectionProperty?.GetValue(coherentState) as dynamic;
            if (cacheEntriesCollection == null)
            {
                return; // Girişlere erişilemedi, işlem sonlandırıldı
            }
            // Regex nesnelerini önceden derle
            List<Regex> regexList = patternList.Select(pattern =>
                    new Regex($@"^{Regex.Escape(pattern.Trim())}(?=\()", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase)).ToList();

            // Anahtarları bulmak ve silmek için girişleri dolaş
            foreach (object? cacheItem in cacheEntriesCollection)
            {
                dynamic? key = cacheItem.GetType().GetProperty("Key")?.GetValue(cacheItem)?.ToString();
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                // Anahtar herhangi bir regex ile eşleşirse sil
                if (regexList.Any(regex => regex.IsMatch(key)))
                {
                    _memoryCache.Remove(key);
                }
            }
        }
        #endregion
        #region Async

        public async Task<T> GetAsync<T>(string key)
        {
            return await Task.FromResult(_memoryCache.Get<T>(key));
        }

        public async Task<object> GetAsync(string key)
        {
            return await Task.FromResult(_memoryCache.Get(key));
        }

        public async Task AddAsync(string key, object data, int duration)
        {
            await Task.Run(() => _memoryCache.Set(key, data, TimeSpan.FromMinutes(duration)));
        }

        public async Task<bool> IsAddAsync(string key)
        {
            return await Task.FromResult(_memoryCache.TryGetValue(key, out _));
        }

        public async Task RemoveAsync(string key)
        {
            await Task.Run(() => _memoryCache.Remove(key));
        }

        public async Task RemoveByPatternAsync(string patterns)
        {
            await Task.Run(() => RemoveByPattern(patterns));
        }

        #endregion
    }
}
