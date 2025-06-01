using Microsoft.EntityFrameworkCore;
using Tutorial10_EFCore_CodeFirst.Application.Repositories;

namespace Tutorial10_EFCore_CodeFirst.Infrastructure.Repositories;

public class MedicamentRepository : IMedicamentRepository
{
    private readonly PrescriptionContext _context;

    public MedicamentRepository(PrescriptionContext context) => _context = context;
    
    public async Task<bool> MedicamentExistsByIdAsync(int medId, CancellationToken cancellationToken = default)
    {
        return await _context.Medicaments.AnyAsync(m => m.IdMedicament == medId, cancellationToken);
    }
}