using Microsoft.EntityFrameworkCore;
using Tutorial9_EFCore_DBFirst.DAL.Models;
using Tutorial9_EFCore_DBFirst.DAL.Repositories.Abstractions;
using Tutorial9_EFCore_DBFirst.DTOs.Responses;
using Tutorial9_EFCore_DBFirst.Services.Abstractions;

namespace Tutorial9_EFCore_DBFirst.DAL.Repositories;

public class TripsRepository : ITripsRepository
{
    private readonly TripsDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public TripsRepository(TripsDbContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken cancellationToken = default)
    {
        var result = await _context.Trips
            .Include(t => t.ClientTrips).ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom)
            .ToListAsync(cancellationToken);

        return result;
    }

    Task<PaginatedResult<Trip>> ITripsRepository.GetPaginatedTripsAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return GetPaginatedTripsAsync(page, pageSize, cancellationToken);
    }

    public async Task<PaginatedResult<Trip>> GetPaginatedTripsAsync(int page, int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var tripsQuery = _context.Trips
            .Include(t => t.ClientTrips).ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom);
        
        double tripsCount = await tripsQuery.CountAsync(cancellationToken);
        var allPages = (int)Math.Ceiling(tripsCount / pageSize);
        
        var toSkip = (page - 1) * pageSize;
        var trips = tripsQuery
            .Skip(toSkip)
            .Take(pageSize)
            .ToList();

        return new PaginatedResult<Trip>
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = allPages,
            Data = trips
        };
    }

    public async Task<bool> IsClientRegisteredOnTripAsync(int clientId, int tripId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ClientTrips
            .AnyAsync(ct => ct.IdClient == clientId && ct.IdTrip == tripId, cancellationToken);
    }

    public async Task<bool> TripExistsByIdAsync(int tripId, CancellationToken cancellationToken = default)
    {
        return await _context.Trips.AnyAsync(ct => ct.IdTrip == tripId, cancellationToken);
    }

    public async Task<bool> HasTripAlreadyOccurredAsync(int tripId, CancellationToken cancellationToken = default)
    {
        return await _context.ClientTrips
            .AnyAsync(ct => ct.IdTrip == tripId && ct.IdTripNavigation.DateFrom < _dateTimeProvider.Now(), 
                cancellationToken);
    }

    public async Task<bool> AddClientTripAsync(ClientTrip clientTrip, CancellationToken cancellationToken = default)
    {
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}