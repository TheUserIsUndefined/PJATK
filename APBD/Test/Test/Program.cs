using Test.Infrastructure;
using Test.Infrastructure.Repositories;
using Test.Infrastructure.Repositories.Abstractions;
using Test.Services;
using Test.Services.Abstractions;

namespace Test;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddScoped<ITasksService, TasksService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();
        builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
        
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