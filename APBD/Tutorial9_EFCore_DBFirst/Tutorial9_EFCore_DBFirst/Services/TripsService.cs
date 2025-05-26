using Tutorial9_EFCore_DBFirst.DAL.Repositories.Abstractions;
using Tutorial9_EFCore_DBFirst.DTOs;
using Tutorial9_EFCore_DBFirst.Services.Abstractions;

namespace Tutorial9_EFCore_DBFirst.Services;

public class TripsService : ITripsService
{
    private readonly ITripsRepository _tripsRepository;

    TripsService(ITripsRepository tripsRepository)
    {
        _tripsRepository = tripsRepository;
    }
    
    public async Task<IEnumerable<GetTripDto>> GetAllTripsAsync(CancellationToken cancellationToken)
    {
        var result = await _tripsRepository.GetAllTripsAsync(cancellationToken);
        
        return result;
    }
}