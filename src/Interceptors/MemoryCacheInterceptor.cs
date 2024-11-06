using AuthorizationInterceptor.Extensions.Abstractions.Headers;
using AuthorizationInterceptor.Extensions.Abstractions.Interceptors;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace AuthorizationInterceptor.Extensions.MemoryCache.Interceptors
{
    internal class MemoryCacheInterceptor : IAuthorizationInterceptor
    {
        private const string CacheKey = "authorization_interceptor_memory_cache_MemoryAuthorizationInterceptor_{0}";
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheInterceptor(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<AuthorizationHeaders?> GetHeadersAsync(string name)
        {
            var headers = _memoryCache.Get<AuthorizationHeaders?>(string.Format(CacheKey, name));
            return Task.FromResult(headers);
        }

        public Task UpdateHeadersAsync(string name, AuthorizationHeaders? _, AuthorizationHeaders? newHeaders)
        {
            if (newHeaders == null)
                return Task.CompletedTask;

            _memoryCache.Set(string.Format(CacheKey, name), newHeaders, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = newHeaders.GetRealExpiration(),
                Priority = CacheItemPriority.NeverRemove
            });

            return Task.CompletedTask;
        }
    }
}
