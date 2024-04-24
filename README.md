![AuthorizationInterceptor Icon](./resources/icon.png)

# AuthorizationInterceptor.Extensions.MemoryCache
[![GithubActions](https://github.com/Adolfok3/AuthorizationInterceptor.Extensions.MemoryCache/actions/workflows/main.yml/badge.svg)](https://github.com/Adolfok3/AuthorizationInterceptor.Extensions.MemoryCache/actions)
[![License](https://img.shields.io/badge/license-MIT-green)](./LICENSE)
[![Coverage Status](https://coveralls.io/repos/github/Adolfok3/AuthorizationInterceptor.Extensions.MemoryCache/badge.svg?branch=main)](https://coveralls.io/github/Adolfok3/AuthorizationInterceptor.Extensions.MemoryCache?branch=main)
[![NuGet Version](https://img.shields.io/nuget/vpre/AuthorizationInterceptor.Extensions.MemoryCache)](https://www.nuget.org/packages/AuthorizationInterceptor.Extensions.MemoryCache)

An interceptor for [AuthorizationInterceptor](https://github.com/Adolfok3/AuthorizationInterceptor) that uses a in Memory cache to handle authorization headers. For more information on how to configure and use Authorization Interceptor, please check the main page of [AuthorizationInterceptor](https://github.com/Adolfok3/AuthorizationInterceptor).

### Installation
Run the following command in package manager console:
```
PM> Install-Package AuthorizationInterceptor.Extensions.MemoryCache
```

Or from the .NET CLI as:
```
dotnet add package AuthorizationInterceptor.Extensions.MemoryCache
```

### Setup
When adding Authorization Interceptor Handler, call the extension method `UseMemoryCacheInterceptor` to options:
```csharp
services.AddHttpClient("TargetApi")
        .AddAuthorizationInterceptorHandler<TargetApiAuthClass>(options =>
		{
			options.UseMemoryCacheInterceptor();
		})
```