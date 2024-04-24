using AuthorizationInterceptor.Extensions.Abstractions.Options;
using AuthorizationInterceptor.Extensions.MemoryCache.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace AuthorizationInterceptor.Extensions.MemoryCache.Tests.Extensions;

public class AuthorizationInterceptorOptionsExtensionsTests
{
    [Fact]
    public void UseMemoryCacheInterceptor_WithouOptions_ShouldAddSuccessfully()
    {
        //Arrange
        var options = Substitute.For<IAuthorizationInterceptorOptions>();

        //Act
        var act = () => options.UseMemoryCacheInterceptor();

        //Assert
        Assert.Null(Record.Exception(act));
        options.Received(1).UseCustomInterceptor<MemoryCacheInterceptor>(Arg.Any<Func<IServiceCollection, IServiceCollection>?>());
    }


    [Fact]
    public void UseMemoryCacheInterceptor_WithOptions_ShouldAddSuccessfully()
    {
        //Arrange
        var options = Substitute.For<IAuthorizationInterceptorOptions>();

        //Act
        var act = () => options.UseMemoryCacheInterceptor(options => options.CompactionPercentage = 150);

        //Assert
        Assert.Null(Record.Exception(act));
        options.Received(1).UseCustomInterceptor<MemoryCacheInterceptor>(Arg.Any<Func<IServiceCollection, IServiceCollection>?>());
    }
}
