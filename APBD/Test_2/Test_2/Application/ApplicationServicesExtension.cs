using Test_2.Application.Services;
using Test_2.Application.Services.Interfaces;

namespace Test_2.Application;

public static class ApplicationServicesExtension
{
    public static void RegisterApplicationServices(this IServiceCollection app)
    {
        app.AddScoped<IBookService, BookService>();
        app.AddScoped<IPublishingHouseService, PublishingHouseService>();
    }
}