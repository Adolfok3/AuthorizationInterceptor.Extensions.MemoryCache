using AuthorizationInterceptor.Extensions.Abstractions.Options;
using AuthorizationInterceptor.Extensions.MemoryCache.Interceptors;

namespace AuthorizationInterceptor.Extensions.MemoryCache.Extensions
{
    /// <summary>
    /// Extension methods that Configures the authorization interceptor to use a memory cache interceptor for <see cref="IAuthorizationInterceptorOptions"/>
    /// </summary>
    public static class AuthorizationInterceptorOptionsExtensions
    {
        /// <summary>
        /// Configures the authorization interceptor to use a memory cache interceptor.
        /// </summary>
        /// <param name="options"><see cref="IAuthorizationInterceptorOptions"/></param>
        /// <returns><see cref="IAuthorizationInterceptorOptions"/></returns>
        public static IAuthorizationInterceptorOptions UseMemoryCacheInterceptor(this IAuthorizationInterceptorOptions options)
        {
            options.UseCustomInterceptor<MemoryCacheInterceptor>();
            return options;
        }
    }
}
