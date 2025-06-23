namespace Project.Application.Services.Interfaces;

public interface ICurrencyExchangeService
{
    public Task<string> ConvertCurrencyAsync(decimal amount, string? toCurrency, 
        CancellationToken cancellationToken = default);
}