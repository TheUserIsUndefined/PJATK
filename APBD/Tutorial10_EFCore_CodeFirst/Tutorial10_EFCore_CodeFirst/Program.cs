using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tutorial10_EFCore_CodeFirst.Application;
using Tutorial10_EFCore_CodeFirst.Application.Middlewares;
using Tutorial10_EFCore_CodeFirst.Infrastructure;

namespace Tutorial10_EFCore_CodeFirst;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var secretKey = builder.Configuration["SecretKey"];

        builder.Services.AddDbContext<PrescriptionContext>(opt => 
            opt.UseSqlServer(connectionString));
        builder.Services.RegisterApplicationServices();
        builder.Services.RegisterInfrastructureServices();
        
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2),
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
            opt.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        context.Response.Headers.Add("Token-Expired", "true");

                    return Task.CompletedTask;
                }
            };
        }).AddJwtBearer("IgnoreTokenExpirationScheme", opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.FromMinutes(2),
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };
        });

        builder.Services.AddControllers();
        
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
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