using Microsoft.EntityFrameworkCore;
using Tutorial9_EFCore_DBFirst.DAL.Models;
using Tutorial9_EFCore_DBFirst.DAL.Infrastructure.Repositories.Abstractions;

namespace Tutorial9_EFCore_DBFirst.DAL.Infrastructure.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly TripsDbContext _context;

    public ClientsRepository(TripsDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> ClientHasTripsAsync(int clientId, CancellationToken cancellationToken = default)
    {
        return await _context.ClientTrips.AnyAsync(ct => ct.IdClient == clientId, cancellationToken);
    }

    public async Task<int> GetClientIdByPeselAsync(string pesel, CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .Where(c => c.Pesel == pesel)
            .Select(c => c.IdClient)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> AddClientAsync(Client client, CancellationToken cancellationToken = default)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync(cancellationToken);

        return client.IdClient;
    }

    public async Task<bool> DeleteClientAsync(int clientId, CancellationToken cancellationToken = default)
    {
        var client = await _context.Clients.FindAsync([clientId], cancellationToken);

        if (client is null)
            return false;
        
        _context.Clients.Remove(client);
        
        return true;
    }
}