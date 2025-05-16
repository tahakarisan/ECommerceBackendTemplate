using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.RegularExpressions;
namespace Core.CrossCuttingConcerns.Caching.MicrosoftCaching
{
    public class HybridCacheManager : ICacheManager
    {
        private HybridCache _cache;

        public HybridCacheManager()
        {
            _cache = ServiceTool.ServiceProvider.GetService<HybridCache>();
        }

        #region Sync
        public T Get<T>(string key)
        {
            T? value = _cache.GetOrCreateAsync(
                key,
                async cancellationToken =>
                {
                    // Cache'te yoksa bu kod çalışır
                    return await Task.FromResult<T>(default);
                }
            ).Result;
            return value;
        }

        public object Get(string key)
        {
            object value = _cache.GetOrCreateAsync(
                key,
                async cancellationToken =>
                {
                    // Cache'te yoksa bu kod çalışır
                    return await Task.FromResult<object>(null);
                }
            ).Result;
            return value;
        }

        public async void Add(string key, object data, int duration)
        {
            await _cache.SetAsync(
                 key,
                 data,
                 new HybridCacheEntryOptions
                 {
                     Expiration = TimeSpan.FromMinutes(duration),
                 }
             );
        }

        public bool IsAdd(string key)
        {
            object value = _cache.GetOrCreateAsync(
                key,
                async cancellationToken =>
                {
                    // Cache'te yoksa bu kod çalışır
                    return await Task.FromResult<object>(null);
                }
            ).Result;
            return value != null;
        }

        public void Remove(string key)
        {
            _cache.RemoveAsync(key);
        }

        public void RemoveByPattern(string patterns)
        {
            // Birden fazla desen için listeyi ayır
            string[] patternList = patterns.Split(',');

            // HybridCache'deki tüm anahtarları al
            IEnumerable<string> cacheKeys = getAllCacheKeys();
            if (cacheKeys == null || !cacheKeys.Any())
            {
                return; // Önbellekte anahtar yoksa işlem sonlandırılır
            }

            // Her desen için eşleşen anahtarları bul ve sil
            foreach (string pattern in patternList)
            {
                Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

                // Desene uyan anahtarları seç
                List<string> keysToRemove = cacheKeys.Where(key => regex.IsMatch(key)).ToList();

                // Anahtarları HybridCache'den sil
                foreach (string? key in keysToRemove)
                {
                    Remove(key); // HybridCache'in Remove metodunu çağırıyoruz
                }
            }
        }
        #endregion
        #region Async
        public async Task<T> GetAsync<T>(string key)
        {
            T? value = await _cache.GetOrCreateAsync(
                key,
                async cancellationToken =>
                {
                    // Cache'te yoksa bu kod çalışır
                    return await Task.FromResult<T>(default);
                }
            );
            return value;
        }

        public async Task<object> GetAsync(string key)
        {
            object value = await _cache.GetOrCreateAsync(
                key,
                async cancellationToken =>
                {
                    // Cache'te yoksa bu kod çalışır
                    return await Task.FromResult<object>(null);
                }
            );
            return value;
        }

        public async Task AddAsync(string key, object data, int duration)
        {
            HybridCacheEntryOptions entryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(duration),
                LocalCacheExpiration = TimeSpan.FromMinutes(duration)
            };

            await _cache.SetAsync(key, data, options: entryOptions);
        }

        public async Task<bool> IsAddAsync(string key)
        {
            object value = await _cache.GetOrCreateAsync(
                key,
                async cancellationToken =>
                {
                    // Cache'te yoksa bu kod çalışır
                    return await Task.FromResult<object>(null);
                }
            );
            return value != null;
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task RemoveByPatternAsync(string patterns)
        {
            // Birden fazla desen için listeyi ayır
            string[] patternList = patterns.Split(',');

            // HybridCache'deki tüm anahtarları al
            IEnumerable<string> cacheKeys = getAllCacheKeys();
            if (cacheKeys == null || !cacheKeys.Any())
            {
                return; // Önbellekte anahtar yoksa işlem sonlandırılır
            }

            // Her desen için eşleşen anahtarları bul ve sil
            foreach (string pattern in patternList)
            {
                Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

                // Desene uyan anahtarları seç
                List<string> keysToRemove = cacheKeys.Where(key => regex.IsMatch(key)).ToList();

                // Anahtarları HybridCache'den sil
                foreach (string? key in keysToRemove)
                {
                    await RemoveAsync(key); // HybridCache'in Remove metodunu çağırıyoruz
                }
            }
        }
        #endregion
        private IEnumerable<string> getAllCacheKeys()
        {
            // Önbellekteki tüm anahtarları alma işlemi
            // MemoryCache gibi bir yapıdan çalışıyorsa, Reflection gerekebilir.
            // Burada örnek bir uygulama için MemoryCache üzerinden okuma gösteriliyor:

            HybridCache memoryCache = _cache as HybridCache; // HybridCache'in MemoryCache bileşenine erişim
            if (memoryCache == null) return Enumerable.Empty<string>();

            FieldInfo? coherentStateField = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
            object? coherentState = coherentStateField?.GetValue(memoryCache);

            PropertyInfo? entriesCollectionProperty = coherentState?.GetType().GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            dynamic? cacheEntriesCollection = entriesCollectionProperty?.GetValue(coherentState) as dynamic;

            if (cacheEntriesCollection == null)
            {
                return Enumerable.Empty<string>();
            }

            List<string> keys = new List<string>();
            foreach (object? cacheItem in cacheEntriesCollection)
            {
                dynamic? key = cacheItem.GetType().GetProperty("Key")?.GetValue(cacheItem)?.ToString();
                if (!string.IsNullOrEmpty(key))
                {
                    keys.Add(key);
                }
            }
            return keys;
        }
    }
}
