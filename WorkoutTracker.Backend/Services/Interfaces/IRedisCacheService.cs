namespace WorkoutTracker.Backend.Services.Interfaces
{
    public interface IRedisCacheService
    {
        Task SetCacheAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T> GetCacheAsync<T>(string key);
        Task DeleteCacheAsync<T>(string key);
    }
}
