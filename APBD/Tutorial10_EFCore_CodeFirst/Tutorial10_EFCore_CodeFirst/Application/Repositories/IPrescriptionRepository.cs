using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Application.Repositories;

public interface IPrescriptionRepository
{
    public Task<int> AddPrescriptionAsync(Prescription prescription, CancellationToken cancellationToken = default);
    public Task AddPrescriptionMedicamentAsync(PrescriptionMedicament prescriptionMedicament, 
        CancellationToken cancellationToken = default);
    public Task<ICollection<Prescription>> GetPrescriptionsByPatientIdAsync(int patientId, 
        CancellationToken cancellationToken = default);
}