using Tutorial8_ADO.NET_Trans_Proceds.Infrastructure;
using Tutorial8_ADO.NET_Trans_Proceds.Repositories;
using Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;
using Tutorial8_ADO.NET_Trans_Proceds.Services;
using Tutorial8_ADO.NET_Trans_Proceds.Services.Interfaces;

namespace Tutorial8_ADO.NET_Trans_Proceds;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddScoped<IWarehouseService, WarehouseService>();
        builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}