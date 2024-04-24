using AuthorizationInterceptor.Extensions.Abstractions.Headers;
using AuthorizationInterceptor.Extensions.Abstractions.Interceptors;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace AuthorizationInterceptor.Extensions.MemoryCache.Interceptors
{
    internal class MemoryCacheInterceptor : IAuthorizationInterceptor
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string CACHE_KEY = "authorization_interceptor_memory_cache_MemoryAuthorizationInterceptor";

        public MemoryCacheInterceptor(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<AuthorizationHeaders?> GetHeadersAsync()
        {
            var headers = _memoryCache.Get<AuthorizationHeaders?>(CACHE_KEY);
            return Task.FromResult(headers);
        }

        public Task UpdateHeadersAsync(AuthorizationHeaders? _, AuthorizationHeaders? newHeaders)
        {
            if (newHeaders == null)
                return Task.CompletedTask;

            _memoryCache.Set(CACHE_KEY, newHeaders, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = newHeaders.GetRealExpiration(),
                Priority = CacheItemPriority.NeverRemove
            });

            return Task.CompletedTask;
        }
    }
}
