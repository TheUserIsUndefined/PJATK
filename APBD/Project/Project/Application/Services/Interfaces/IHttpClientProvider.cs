namespace Project.Application.Services.Interfaces;

public interface IHttpClientProvider
{
    public HttpClient CreateClient();
}