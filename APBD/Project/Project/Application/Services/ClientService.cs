using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.Application.DTOs.Requests;
using Project.Application.Exceptions;
using Project.Application.Services.Interfaces;
using Project.Core.Models;
using Project.Infrastructure;

namespace Project.Application.Services;

public class ClientService : IClientService
{
    private readonly AppDbContext _context;
    private IClientService _clientServiceImplementation;

    public ClientService(AppDbContext context)
    {
        _context = context;
    }

public async Task<int> AddClientAsync(AddClientRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Individual is not null && request.Company is not null)
            throw new ClientExceptions.ClientValidationException("Client cannot be both Individual and Company.");
        if (request.Individual is null && request.Company is null)
            throw new ClientExceptions.ClientValidationException("Client should be either Individual or Company.");

        if (request.Individual is not null)
        {
            var individualClient = await _context.Individuals
                .FirstOrDefaultAsync(i => i.Pesel == request.Individual.Pesel, cancellationToken);

            if (individualClient is not null)
                throw new ClientExceptions.ClientValidationException("Individual already exists in the database.");
        }

        if (request.Company is not null)
        {
            var companyClient = await _context.Companies
                .FirstOrDefaultAsync(c => c.Krs == request.Company.Krs, cancellationToken);
            
            if (companyClient is not null)
                throw new ClientExceptions.ClientValidationException("Company already exists in the database.");
        }
        
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
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
            }

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return client.ClientId;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task DeleteClientAsync(int clientId, CancellationToken cancellationToken = default)
    {
        var client = await _context.Clients
            .Include(c => c.Individual)
            .Include(c => c.Company)
            .FirstOrDefaultAsync(c => c.ClientId == clientId, cancellationToken);

        if (client is null)
            throw new ClientExceptions.ClientNotFoundByIdException(clientId);

        if (client.Company is not null)
            throw new ClientExceptions.ClientValidationException("Company clients cannot be deleted.");

        if (client.Individual is null)
            throw new ClientExceptions.ClientValidationException("Invalid client: no individual or company data.");

        if (client.Individual.IsDeleted)
            throw new ClientExceptions.ClientValidationException($"Client {clientId} is already deleted.");
        
        client.Individual.IsDeleted = true;
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateClientAsync(int clientId, UpdateClientRequest request, 
        CancellationToken cancellationToken = default)
    {
        if (request.Individual is not null && request.Company is not null)
            throw new ClientExceptions.ClientValidationException("Client cannot be both Individual and Company.");
        if (request.Individual is null && request.Company is null)
            throw new ClientExceptions.ClientValidationException("Client should be either Individual or Company.");

        var client = await _context.Clients
            .Include(c => c.Individual)
            .Include(c => c.Company)
            .FirstOrDefaultAsync(c => c.ClientId == clientId, cancellationToken);

        if (client is null)
            throw new ClientExceptions.ClientNotFoundByIdException(clientId);

        if (request.Individual is not null)
        {
            if (client.Individual is null)
                throw new ClientExceptions.ClientValidationException("This client is not an individual.");

            if (client.Individual.IsDeleted)
                throw new ClientExceptions.ClientValidationException($"Client {clientId} is already deleted.");
        }
        else if (request.Company is not null)
            if (client.Company is null)
                throw new ClientExceptions.ClientValidationException("This client is not a company.");
        
        if (!string.IsNullOrWhiteSpace(request.Address))
            client.Address = request.Address;

        if (!string.IsNullOrWhiteSpace(request.Email))
            client.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            client.PhoneNumber = request.PhoneNumber;

        if (client.Individual is not null)
        {
            if (!string.IsNullOrWhiteSpace(request.Individual?.FirstName))
                client.Individual.FirstName = request.Individual.FirstName;
            
            if (!string.IsNullOrWhiteSpace(request.Individual?.LastName))
                client.Individual.LastName = request.Individual.LastName;
        }
        else if (client.Company is not null)
            if (!string.IsNullOrWhiteSpace(request.Company?.Name))
                client.Company.Name = request.Company.Name;

        await _context.SaveChangesAsync(cancellationToken);
    }
}