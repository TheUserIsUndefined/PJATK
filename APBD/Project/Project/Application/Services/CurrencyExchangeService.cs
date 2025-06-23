using System.Globalization;
using System.Text.Json;
using Project.Application.Services.Interfaces;

namespace Project.Application.Services;

public class CurrencyExchangeService : ICurrencyExchangeService
{
    private const string BaseCurrency = "PLN";
    private const string ExchangeApiUrl = "https://api.exchangerate-api.com/v4/latest";
    
    private readonly IHttpClientProvider _httpClientProvider;
    
    public CurrencyExchangeService(IHttpClientProvider httpClientProvider) 
        => _httpClientProvider = httpClientProvider;

    public async Task<string> ConvertCurrencyAsync(decimal amount, string? toCurrency,
        CancellationToken cancellationToken = default)
    {
        toCurrency = toCurrency?.ToUpper();
        
        if (toCurrency is not null && !BaseCurrency.Equals(toCurrency, StringComparison.OrdinalIgnoreCase))
        {
            var url = $"{ExchangeApiUrl}/{BaseCurrency}";

            var httpClient = _httpClientProvider.CreateClient();

            var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var doc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            var rate = doc.RootElement
                .GetProperty("rates")
                .GetProperty(toCurrency)
                .GetDecimal();

            amount = Math.Round(amount * rate, 2);
        }

        return amount.ToString(CultureInfo.InvariantCulture) + (toCurrency ?? BaseCurrency);
    }
}