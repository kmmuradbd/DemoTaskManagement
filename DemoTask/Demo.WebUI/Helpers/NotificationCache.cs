using Microsoft.Extensions.Caching.Memory;

namespace Demo.WebUI.Helpers
{
    public static class NotificationCache
    {
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public static void SetLastCreatedDate(string userName, DateTime date)
        {
            _cache.Set(userName + "_lastCreatedDate", date, TimeSpan.FromHours(5)); // expires in 1 hour
        }

        public static DateTime? GetLastCreatedDate(string userName)
        {
            if (_cache.TryGetValue(userName + "_lastCreatedDate", out DateTime date))
                return date;
            return null;
        }
    }
}
