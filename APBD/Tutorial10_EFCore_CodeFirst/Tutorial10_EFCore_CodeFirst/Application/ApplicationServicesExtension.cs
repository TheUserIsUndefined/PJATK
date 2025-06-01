using Tutorial10_EFCore_CodeFirst.Application.Services;
using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

namespace Tutorial10_EFCore_CodeFirst.Application;

public static class ApplicationServicesExtension
{
    public static void RegisterApplicationServices(this IServiceCollection app)
    {
        app.AddScoped<IPrescriptionService, PrescriptionService>();
        app.AddScoped<IPatientService, PatientService>();
    }
}