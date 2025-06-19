using Microsoft.EntityFrameworkCore;
using Project.Application.DTOs.Requests;
using Project.Application.Services.Interfaces;
using Project.Core.Models;

namespace Project.Application.Services;

public class ClientService : IClientService
{
    private readonly DbContext _context;

    public ClientService(DbContext context)
    {
        _context = context;
    }

public async Task<int> AddClientAsync(AddClientRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Individual is not null && request.Company is not null)
            throw new ArgumentException("Client cannot be both Individual and Company.");
        if (request.Individual is null && request.Company is null)
            throw new ArgumentException("Client should be either Individual or Company.");
        
        var client = new Client
        {
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
        await _context.AddAsync(client, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        if (request.Individual is not null)
        {
            var individual = new Individual
            {
                FirstName = request.Individual.FirstName,
                LastName = request.Individual.LastName,
                Pesel = request.Individual.Pesel,
                ClientId = client.ClientId
            };
            
            await _context.AddAsync(individual, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else if (request.Company is not null)
        {
            var company = new Company
            {
                Name = request.Company.Name,
                Krs = request.Company.Krs,
                ClientId = client.ClientId
            };
            
            await _context.AddAsync(company, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        return client.ClientId;
    }
}