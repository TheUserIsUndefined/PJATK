using Microsoft.EntityFrameworkCore;
using Tutorial10_EFCore_CodeFirst.Application.Repositories;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly PrescriptionContext _context;

    public PatientRepository(PrescriptionContext context) => _context = context;
    
    public async Task<bool> PatientExistsByIdAsync(int patientId, CancellationToken cancellationToken)
    {
        return await _context.Patients.AnyAsync(p => p.IdPatient == patientId, cancellationToken);
    }

    public async Task<int> AddPatientAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        await _context.Patients.AddAsync(patient, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return patient.IdPatient;
    }

    public async Task<Patient?> GetPatientByIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Where(p => p.IdPatient == patientId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}