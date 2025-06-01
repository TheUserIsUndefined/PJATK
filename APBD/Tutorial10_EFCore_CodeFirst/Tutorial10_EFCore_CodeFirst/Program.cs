using Microsoft.EntityFrameworkCore;
using Tutorial10_EFCore_CodeFirst.Application;
using Tutorial10_EFCore_CodeFirst.Infrastructure;

namespace Tutorial10_EFCore_CodeFirst;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<PrescriptionContext>(opt => 
            opt.UseSqlServer(connectionString));
        builder.Services.RegisterApplicationServices();
        builder.Services.RegisterInfrastructureServices();

        builder.Services.AddControllers();
        
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

/*
 dotnet add package Microsoft.EntityFrameworkCore
 dotnet add package Microsoft.EntityFrameworkCore.SqlServer
 dotnet add package Microsoft.EntityFrameworkCore.Tools
 dotnet add package Microsoft.EntityFrameworkCore.Design
*/