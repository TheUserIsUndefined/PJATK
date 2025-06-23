using Project.Application.Services;
using Project.Application.Services.Interfaces;

namespace Project.Application;

public static class ApplicationServicesExtension
{
    public static void RegisterApplicationServices(this IServiceCollection app)
    {
        app.AddScoped<IClientService, ClientService>();
        app.AddScoped<IContractService, ContractService>();
        app.AddScoped<ICurrencyExchangeService, CurrencyExchangeService>();
        app.AddScoped<IDateTimeProvider, DateTimeProvider>();
        app.AddScoped<IHttpClientProvider, HttpClientProvider>();
        app.AddScoped<IPaymentService, PaymentService>();
        app.AddScoped<IRevenueService, RevenueService>();
        app.AddScoped<IUserService, UserService>();
        app.AddScoped<ITokenService, TokenService>();
    }
}