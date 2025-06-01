using Tutorial10_EFCore_CodeFirst.Application.DTOs.Responses;

namespace Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

public interface IPatientService
{
    public Task<GetPatientResponse> GetPatientsAsync(int patientId, CancellationToken cancellationToken = default);
}