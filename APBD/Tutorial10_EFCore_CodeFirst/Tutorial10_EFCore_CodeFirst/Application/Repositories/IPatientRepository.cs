using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Application.Repositories;

public interface IPatientRepository
{
    public Task<bool> PatientExistsByIdAsync(int patientId, CancellationToken cancellationToken);
    public Task<int> AddPatientAsync(Patient patient, CancellationToken cancellationToken = default);
    public Task<Patient?> GetPatientByIdAsync(int patientId, CancellationToken cancellationToken = default);
}