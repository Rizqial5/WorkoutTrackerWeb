using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WorkoutTracker.Backend.Services.Interfaces;

namespace WorkoutTracker.Backend.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(1)
            };

            var serializedData = JsonSerializer.Serialize(value);

            await _cache.SetStringAsync(key, serializedData, options);
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            var serializedData = await _cache.GetStringAsync(key);

            if (serializedData == null) return default!;

            return JsonSerializer.Deserialize<T>(serializedData);
        }

        public async Task DeleteCacheAsync<T>(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
