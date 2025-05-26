using Tutorial9_EFCore_DBFirst.DAL.Repositories.Abstractions;
using Tutorial9_EFCore_DBFirst.DTOs;

namespace Tutorial9_EFCore_DBFirst.DAL.Repositories;

public class TripsRepository : ITripsRepository
{
    public async Task<IEnumerable<GetTripDto>> GetAllTripsAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}