using AuthorizationInterceptor.Extensions;
using AuthorizationInterceptor.Extensions.Abstractions.Handlers;
using AuthorizationInterceptor.Extensions.Abstractions.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using AuthorizationInterceptor.Extensions.MemoryCache.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient("TargetApiAuth")
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5121"));

builder.Services.AddHttpClient("TargetApi")
    .AddAuthorizationInterceptorHandler<TargetApiAuthClass>(options =>
    {
        options.UseMemoryCacheInterceptor();
    })
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5121"));

builder.Services.AddHttpClient("TargetApiAuth2")
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5121"));

builder.Services.AddHttpClient("TargetApi2")
    .AddAuthorizationInterceptorHandler<TargetApiAuthClass2>(options =>
    {
        options.UseMemoryCacheInterceptor();
    })
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5121"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/data", async (IHttpClientFactory factory) =>
{
    var client = factory.CreateClient("TargetApi");
    return await client.GetAsync("/data");
});

app.MapGet("/data/2", async (IHttpClientFactory factory) =>
{
    var client = factory.CreateClient("TargetApi2");
    return await client.GetAsync("/data");
});

app.Run();

public class User
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("refresh_token_expires_in")]
    public int RefreshTokenExpiresIn { get; set; }
}

public class TargetApiAuthClass : IAuthenticationHandler
{
    private readonly HttpClient _client;

    public TargetApiAuthClass(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("TargetApiAuth");
    }

    public async ValueTask<AuthorizationHeaders?> AuthenticateAsync(AuthorizationHeaders? expiredHeaders, CancellationToken cancellation)
    {
        HttpResponseMessage? response = null;

        if (expiredHeaders == null)
        {
            response = await _client.PostAsync("auth", null, cancellation);
        }
        else
        {
            response = await _client.PostAsync($"refresh?refresh={expiredHeaders.OAuthHeaders!.RefreshToken}", null, cancellation);
        }

        var content = await response.Content.ReadAsStringAsync(cancellation);
        var newHeaders = JsonSerializer.Deserialize<User>(content)!;
        return new OAuthHeaders(newHeaders.AccessToken, newHeaders.TokenType, newHeaders.ExpiresIn, newHeaders.RefreshToken, newHeaders.RefreshTokenExpiresIn);
    }
}

public class TargetApiAuthClass2 : IAuthenticationHandler
{
    private readonly HttpClient _client;

    public TargetApiAuthClass2(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("TargetApiAuth2");
    }

    public async ValueTask<AuthorizationHeaders?> AuthenticateAsync(AuthorizationHeaders? expiredHeaders, CancellationToken cancellation)
    {
        var response = expiredHeaders != null
            ? await _client.PostAsync($"refresh?refresh={expiredHeaders.OAuthHeaders!.RefreshToken}", null, cancellation)
            : await _client.PostAsync("auth", null, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);
        var user = JsonSerializer.Deserialize<User>(content)!;
        return new OAuthHeaders(user.AccessToken, user.TokenType, user.ExpiresIn, user.RefreshToken, user.RefreshTokenExpiresIn);
    }
}