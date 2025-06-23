using Microsoft.EntityFrameworkCore;
using Project.Application.Exceptions;
using Project.Application.Services.Interfaces;
using Project.Core.Models;
using Project.Infrastructure;

namespace Project.Application.Services;

public class RevenueService : IRevenueService
{
    private readonly AppDbContext _context;
    private readonly ICurrencyExchangeService _currencyExchangeService;

    public RevenueService(
        AppDbContext context,
        ICurrencyExchangeService currencyExchangeService
        )
    {
        _context = context;
        _currencyExchangeService = currencyExchangeService;
    }
    
    public async Task<string> GetCurrentRevenueAsync(int? softwareId = null, string? currency = null,
        CancellationToken cancellationToken = default)
    {
        return await GetRevenueAsync([ContractStatus.Signed], softwareId, currency, cancellationToken);
    }

    public async Task<string> GetPredictedRevenueAsync(int? softwareId = null, string? currency = null,
        CancellationToken cancellationToken = default)
    {
        return await GetRevenueAsync([ContractStatus.Signed, ContractStatus.NotSigned], 
            softwareId, currency, cancellationToken);
    }

    public async Task<string> GetRevenueAsync(ContractStatus[] filterBy, int? softwareId = null, string? currency = null,
        CancellationToken cancellationToken = default)
    {
        decimal amount;

        if (softwareId is not null)
        {
            var software = await _context.Softwares
                .Include(s => s.Contracts)
                .FirstOrDefaultAsync(s => s.SoftwareId == softwareId, cancellationToken);
            
            if (software is null)
                throw new SoftwareExceptions.SoftwareNotFoundByIdException(softwareId.Value);

            amount = software.Contracts
                .Where(c => filterBy.Contains(c.Status))
                .Sum(c => c.Price);
        }
        else
            amount = await _context.Contracts
                .Where(c => filterBy.Contains(c.Status))
                .SumAsync(c => c.Price, cancellationToken);
        
        try
        {
            var convertedAmount = await _currencyExchangeService
                .ConvertCurrencyAsync(amount, currency, cancellationToken);
                
            return convertedAmount;
        }
        catch (KeyNotFoundException)
        {
            throw new InvalidCurrencyException(currency);
        }
    }
}