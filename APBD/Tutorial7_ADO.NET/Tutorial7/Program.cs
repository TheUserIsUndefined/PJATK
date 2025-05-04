using Tutorial7.Repositories;
using Tutorial7.Repositories.Interfaces;
using Tutorial7.Services;
using Tutorial7.Services.Interfaces;

namespace Tutorial7;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<ITripRepository, TripRepository>();
        builder.Services.AddScoped<ITripService, TripService>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IClientService, ClientService>();    
        builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddProblemDetails();

        var app = builder.Build();
        
        app.UseStatusCodePages();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}