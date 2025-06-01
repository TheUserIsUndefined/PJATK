using Microsoft.EntityFrameworkCore;
using Tutorial10_EFCore_CodeFirst.Application.Repositories;

namespace Tutorial10_EFCore_CodeFirst.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly PrescriptionContext _context;
    
    public DoctorRepository(PrescriptionContext context) => _context = context;
    
    public async Task<bool> DoctorExistsByIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.AnyAsync(d => d.IdDoctor == doctorId, cancellationToken);
    }
}