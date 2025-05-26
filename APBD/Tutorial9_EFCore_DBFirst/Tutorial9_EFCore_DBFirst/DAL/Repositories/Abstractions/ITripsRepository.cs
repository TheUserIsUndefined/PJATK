using Tutorial9_EFCore_DBFirst.DTOs;

namespace Tutorial9_EFCore_DBFirst.DAL.Repositories.Abstractions;

public interface ITripsRepository
{
    public Task<IEnumerable<GetTripDto>> GetAllTripsAsync(CancellationToken cancellationToken); 
}