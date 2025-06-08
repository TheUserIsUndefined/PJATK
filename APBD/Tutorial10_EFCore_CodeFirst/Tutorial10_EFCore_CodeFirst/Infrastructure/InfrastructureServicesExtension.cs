using Tutorial10_EFCore_CodeFirst.Application.Repositories;
using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;
using Tutorial10_EFCore_CodeFirst.Infrastructure.Repositories;

namespace Tutorial10_EFCore_CodeFirst.Infrastructure;

public static class InfrastructureServicesExtension
{
    public static void RegisterInfrastructureServices(this IServiceCollection app)
    {
        app.AddScoped<IDoctorRepository, DoctorRepository>();
        app.AddScoped<IMedicamentRepository, MedicamentRepository>();
        app.AddScoped<IPatientRepository, PatientRepository>();
        app.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        app.AddScoped<IUnitOfWork, UnitOfWork>();
        app.AddScoped<IUserRepository, UserRepository>();
    }
}