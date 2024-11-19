﻿using AuthorizationInterceptor.Extensions.Abstractions.Headers;
using AuthorizationInterceptor.Extensions.MemoryCache.Interceptors;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace AuthorizationInterceptor.Extensions.MemoryCache.Tests.Interceptors;

public class MemoryAuthorizationInterceptorTests
{
    private const string CacheKey = "authorization_interceptor_memory_cache_MemoryCacheInterceptor_test";

    private readonly IMemoryCache _memory;
    private readonly MemoryCacheInterceptor _interceptor;

    public MemoryAuthorizationInterceptorTests()
    {
        _memory = Substitute.For<IMemoryCache>();
        _interceptor = new MemoryCacheInterceptor(_memory);
    }

    [Fact]
    public async Task GetHeadersAsync_ShouldReturnNull()
    {
        //Act
        var headers = await _interceptor.GetHeadersAsync("test");

        //Assert
        headers.Should().BeNull();
        _memory.Received(1).Get<AuthorizationHeaders?>(CacheKey);
    }

    [Fact]
    public async Task GetHeadersAsync_ShouldReturnHeaders()
    {
        //Arrange
        AuthorizationHeaders obj = new OAuthHeaders("accesstoken", "tokentype");
        _memory.TryGetValue(CacheKey, out Arg.Any<object?>()).Returns(x =>
        {
            x[1] = obj;
            return true;
        });

        //Act
        var headers = await _interceptor.GetHeadersAsync("test");

        //Assert
        headers.Should().NotBeNull();
        headers!.OAuthHeaders.Should().NotBeNull();
        headers.OAuthHeaders!.AccessToken.Should().Be("accesstoken");
        headers.OAuthHeaders.TokenType.Should().Be("tokentype");
        _memory.Received(1).Get<AuthorizationHeaders?>(CacheKey);
    }

    [Fact]
    public async Task UpdateHeadersAsync_WithNullHeaders_ShouldNotUpdateInMemoryCache()
    {
        //Act
        var act = () => _interceptor.UpdateHeadersAsync("test", null, null);

        //Assert
        await act.Should().NotThrowAsync();
        _memory.Received(0).CreateEntry(Arg.Any<object>());
    }

    [Fact]
    public async Task UpdateHeadersAsync_WithHeaders_ShouldUpdateInMemoryCache()
    {
        //Act
        var act = () => _interceptor.UpdateHeadersAsync("test", null, new OAuthHeaders("accesstoken", "tokentype"));

        //Assert
        await act.Should().NotThrowAsync();
        _memory.Received(1).CreateEntry(Arg.Any<object>());
    }
}
