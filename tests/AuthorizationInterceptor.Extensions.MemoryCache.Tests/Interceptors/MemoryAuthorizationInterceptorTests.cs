using AuthorizationInterceptor.Extensions.Abstractions.Headers;
using AuthorizationInterceptor.Extensions.Abstractions.Interceptors;
using AuthorizationInterceptor.Extensions.MemoryCache.Interceptors;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace AuthorizationInterceptor.Extensions.MemoryCache.Tests.Interceptors;

public class MemoryAuthorizationInterceptorTests
{
    private readonly IMemoryCache _memory;
    private IAuthorizationInterceptor _interceptor;
    private readonly string CACHE_KEY = "authorization_interceptor_memory_cache_MemoryAuthorizationInterceptor";

    public MemoryAuthorizationInterceptorTests()
    {
        _memory = Substitute.For<IMemoryCache>();
        _interceptor = new MemoryCacheInterceptor(_memory);
    }

    [Fact]
    public async Task GetHeadersAsync_ShouldReturnNull()
    {
        //Act
        var headers = await _interceptor.GetHeadersAsync();

        //Assert
        Assert.Null(headers);
        _memory.Received(1).Get<AuthorizationHeaders?>(CACHE_KEY);
    }

    [Fact]
    public async Task GetHeadersAsync_ShouldReturnHeaders()
    {
        //Arrange
        AuthorizationHeaders obj = new OAuthHeaders("accesstoken", "tokentype");
        _memory.TryGetValue(CACHE_KEY, out Arg.Any<object?>()).Returns(x =>
        {
            x[1] = obj;
            return true;
        });

        //Act
        var headers = await _interceptor.GetHeadersAsync();

        //Assert
        Assert.NotNull(headers);
        Assert.NotNull(headers.OAuthHeaders);
        Assert.Equal("accesstoken", headers.OAuthHeaders.AccessToken);
        Assert.Equal("tokentype", headers.OAuthHeaders.TokenType);
        _memory.Received(1).Get<AuthorizationHeaders?>(CACHE_KEY);
    }

    [Fact]
    public async Task UpdateHeadersAsync_WithNullHeaders_ShouldNotUpdateInMemoryCache()
    {
        //Act
        Task act() => _interceptor.UpdateHeadersAsync(null, null);

        //Assert
        Assert.Null(await Record.ExceptionAsync(act));
        _memory.Received(0).CreateEntry(Arg.Any<object>());
    }

    [Fact]
    public async Task UpdateHeadersAsync_WithHeaders_ShouldUpdateInMemoryCache()
    {
        //Act
        Task act() => _interceptor.UpdateHeadersAsync(null, new OAuthHeaders("accesstoken", "tokentype"));

        //Assert
        Assert.Null(await Record.ExceptionAsync(act));
        _memory.Received(1).CreateEntry(Arg.Any<object>());
    }
}
