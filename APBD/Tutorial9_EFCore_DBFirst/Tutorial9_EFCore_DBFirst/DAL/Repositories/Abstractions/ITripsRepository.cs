using Tutorial9_EFCore_DBFirst.DAL.Models;
using Tutorial9_EFCore_DBFirst.DTOs;

namespace Tutorial9_EFCore_DBFirst.DAL.Repositories.Abstractions;

public interface ITripsRepository
{
    public Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken cancellationToken); 
}