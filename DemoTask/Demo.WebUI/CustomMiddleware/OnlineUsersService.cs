using Microsoft.Extensions.Caching.Memory;

namespace Demo.WebUI.CustomMiddleware
{
    public class OnlineUsersService
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "LoggedInUsers";

        public OnlineUsersService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddUser(string userName)
        {
            var users = _cache.Get<Dictionary<string, DateTime>>(CacheKey)
                        ?? new Dictionary<string, DateTime>();

            users[userName] = DateTime.Now; 

            _cache.Set(CacheKey, users, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
        }



        public void RemoveUser(string userName)
        {
            var users = _cache.Get<Dictionary<string, DateTime>>(CacheKey);
            if (users != null && users.ContainsKey(userName))
            {
                users.Remove(userName);

                _cache.Set(CacheKey, users, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });
            }
        }



        public Dictionary<string, DateTime> GetUsers()
        {
            return _cache.Get<Dictionary<string, DateTime>>(CacheKey)
                   ?? new Dictionary<string, DateTime>();
        }
    }

}
