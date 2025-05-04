using Tutorial7.Contracts.Responses;
using Tutorial7.Repositories.Interfaces;
using Tutorial7.Services.Interfaces;

namespace Tutorial7.Services;

public class TripService : ITripService
{
    
    private ITripRepository _tripRepository;
    
    public TripService(ITripRepository tripRepository) => _tripRepository = tripRepository;
    
    public async Task<IEnumerable<GetAllTripsResponse>> GetAllTripsAsync(CancellationToken cancellationToken)
    {
        var trips = await _tripRepository.GetAllTripsAsync(cancellationToken);
        
        return trips;
    }
}