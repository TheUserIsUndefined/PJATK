using Microsoft.EntityFrameworkCore;
using Tutorial10_EFCore_CodeFirst.Application.Repositories;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Infrastructure.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly PrescriptionContext _context;

    public PrescriptionRepository(PrescriptionContext context) => _context = context;
    
    public async Task<int> AddPrescriptionAsync(Prescription prescription, 
        CancellationToken cancellationToken = default)
    {
        await _context.Prescriptions.AddAsync(prescription, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return prescription.IdPrescription;
    }

    public async Task AddPrescriptionMedicamentAsync(PrescriptionMedicament prescriptionMedicament,
        CancellationToken cancellationToken = default)
    {
        await _context.PrescriptionMedicaments.AddAsync(prescriptionMedicament, cancellationToken);
    }

    public async Task<ICollection<Prescription>> GetPrescriptionsByPatientIdAsync(int patientId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Prescriptions
            .Where(p => p.IdPatient == patientId)
            .Include(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Doctor)
            .ToListAsync(cancellationToken);
    }
}