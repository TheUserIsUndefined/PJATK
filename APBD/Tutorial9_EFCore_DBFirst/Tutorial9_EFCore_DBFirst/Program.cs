using Microsoft.EntityFrameworkCore;
using Tutorial9_EFCore_DBFirst.DAL;
using Tutorial9_EFCore_DBFirst.DAL.Repositories;
using Tutorial9_EFCore_DBFirst.DAL.Repositories.Abstractions;
using Tutorial9_EFCore_DBFirst.Services;
using Tutorial9_EFCore_DBFirst.Services.Abstractions;

namespace Tutorial9_EFCore_DBFirst;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddDbContext<TripsDbContext>(opt =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            opt.UseSqlServer(connectionString);
        });
        
        builder.Services.AddScoped<ITripsService, TripsService>();
        builder.Services.AddScoped<ITripsRepository, TripsRepository>();
        builder.Services.AddScoped<IClientsService, ClientsService>();
        builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
        builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}