using AuthorizationInterceptor.Extensions.Abstractions.Options;
using AuthorizationInterceptor.Extensions.MemoryCache.Interceptors;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AuthorizationInterceptor.Extensions.MemoryCache
{
    /// <summary>
    /// Extension methods that Configures the authorization interceptor to use an in-memory cache interceptor for <see cref="AuthorizationInterceptorOptions"/>
    /// </summary>
    public static class AuthorizationInterceptorOptionsExtensions
    {
        /// <summary>
        /// Configures the authorization interceptor to use an in-memory cache interceptor.
        /// </summary>
        /// <param name="options"><see cref="IAuthorizationInterceptorOptions"/></param>
        /// <param name="options"><see cref="MemoryCacheOptions"/></param>
        /// <returns><see cref="IAuthorizationInterceptorOptions"/></returns>
        public static IAuthorizationInterceptorOptions UseMemoryCacheInterceptor(this IAuthorizationInterceptorOptions options, Action<MemoryCacheOptions>? memoryOptions = null)
        {
            memoryOptions ??= (memoryOptions => new MemoryCacheOptions());
            options.UseCustomInterceptor<MemoryCacheInterceptor>(f => f.AddMemoryCache(memoryOptions));

            return options;
        }
    }
}
