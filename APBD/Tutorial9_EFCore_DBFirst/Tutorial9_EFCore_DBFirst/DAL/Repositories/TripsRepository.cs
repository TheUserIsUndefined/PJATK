using Microsoft.EntityFrameworkCore;
using Tutorial9_EFCore_DBFirst.DAL.Models;
using Tutorial9_EFCore_DBFirst.DAL.Repositories.Abstractions;
using Tutorial9_EFCore_DBFirst.DTOs;

namespace Tutorial9_EFCore_DBFirst.DAL.Repositories;

public class TripsRepository : ITripsRepository
{
    private readonly TripsDbContext _context;

    public TripsRepository(TripsDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken cancellationToken)
    {
        var result = await _context.Trips
            .Include(t => t.ClientTrips).ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom)
            .ToListAsync(cancellationToken);

        return result;
    }
}