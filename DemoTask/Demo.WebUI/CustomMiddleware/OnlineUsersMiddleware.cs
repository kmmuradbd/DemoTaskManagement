using Microsoft.Extensions.Caching.Memory;

namespace Demo.WebUI.CustomMiddleware
{
    public class OnlineUsersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "LoggedInUsers";
        public OnlineUsersMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var loggedInUsers = _cache.Get<Dictionary<string, DateTime>>(CacheKey)
                                ?? new Dictionary<string, DateTime>();

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userName = context.User.Identity.Name;

                // Update last activity timestamp
                loggedInUsers[userName] = DateTime.Now;
            }

            // Cleanup inactive users (>10 mins)
            foreach (var item in loggedInUsers.ToList())
            {
                if (item.Value < DateTime.Now.AddMinutes(-10))
                {
                    loggedInUsers.Remove(item.Key);
                }
            }

            // Update cache with expiration (optional)
            _cache.Set(CacheKey, loggedInUsers, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            await _next(context);
        }
    }

}
