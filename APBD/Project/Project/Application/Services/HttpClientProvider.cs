using Project.Application.Services.Interfaces;

namespace Project.Application.Services;

public class HttpClientProvider : IHttpClientProvider
{
    public HttpClient CreateClient() => new HttpClient();
}