using Microsoft.EntityFrameworkCore;
using Project.Application.DTOs.Requests;
using Project.Application.Exceptions;
using Project.Application.Services.Interfaces;
using Project.Core.Models;
using Project.Infrastructure;

namespace Project.Application.Services;

public class ContractService : IContractService
{
    private const string ContractType = "Upfront";
    private const decimal AdditionalSupportCostPerYear = 1000;
    private const decimal ReturningClientDiscount = 5;
    
    private readonly AppDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ContractService(AppDbContext context,
        IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<int> CreateContractAsync(CreateContractRequest request, CancellationToken cancellationToken = default)
    {
        var timeRange = request.EndDate.DayNumber - request.StartDate.DayNumber;
        if (timeRange is < 3 or > 30)
            throw new ContractExceptions
                .ContractValidationException("Contract time range must be between 3 and 30 days.");
        
        if (request.StartDate < _dateTimeProvider.Today())
            throw new ContractExceptions
                .ContractValidationException("Contract start date cannot be before today's date.");
        
        var client = await _context.Clients
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.ClientId == request.ClientId, cancellationToken);
        
        if (client is null)
            throw new ClientExceptions.ClientNotFoundByIdException(request.ClientId);
        
        var software = await _context.Softwares
            .FirstOrDefaultAsync(s => s.SoftwareId == request.SoftwareId, cancellationToken);
        
        if (software is null)
            throw new SoftwareExceptions.SoftwareNotFoundByIdException(request.SoftwareId);

        if (software.UpfrontCostPerYear is null)
            throw new SoftwareExceptions.SoftwareValidationException(
                "The software is not eligible for upfront payments.");
        
        var hasActiveContract = await _context.Contracts
            .AnyAsync(c => c.ClientId == request.ClientId && 
                           c.SoftwareId == request.SoftwareId && 
                           c.SoftwareVersion == software.CurrentVersion &&
                           c.Status == ContractStatus.Signed &&
                           _dateTimeProvider.Today() <= c.EndDate.AddYears(c.SupportYears), cancellationToken);
        
        if (hasActiveContract)
            throw new ContractExceptions
                .ContractValidationException("Client already has an active contract for this software.");
        
        var basePrice = software.UpfrontCostPerYear.Value;
        var additionalSupportCost = (request.AdditionalSupportYears ?? 0) * AdditionalSupportCostPerYear;
        
        var discountAmount = 
            await CalculateDiscountsAsync(client.ClientId, basePrice, cancellationToken);
            
        var finalPrice = basePrice + additionalSupportCost - discountAmount;
        
        var contract = new Contract
        {
            ClientId = request.ClientId,
            SoftwareId = request.SoftwareId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            SoftwareVersion = software.CurrentVersion,
            Price = finalPrice,
            SupportYears = 1 + request.AdditionalSupportYears ?? 0,
            UpdatesInformation = request.UpdatesInformation,
            Status = ContractStatus.NotSigned
        };
        
        await _context.Contracts.AddAsync(contract, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return contract.ContractId;
    }
    
    private async Task<decimal> CalculateDiscountsAsync(
        int clientId,
        decimal basePrice, 
        CancellationToken cancellationToken)
    {
        decimal totalDiscountAmount = 0;

        var today = _dateTimeProvider.Today();

        var highestDiscount = await _context.Discounts
            .Where(d => d.DiscountType.Name == ContractType && d.StartDate <= today && d.EndDate >= today)
            .OrderByDescending(d => d.PercentageValue)
            .FirstOrDefaultAsync(cancellationToken);

        if (highestDiscount != null)
        {
            var discountAmount = basePrice * (highestDiscount.PercentageValue / 100);
            totalDiscountAmount += discountAmount;
        }

        var isReturningClient = await _context.Contracts
            .AnyAsync(c => c.ClientId == clientId && c.Status == ContractStatus.Signed, cancellationToken);

        if (isReturningClient)
        {
            var returningClientDiscountAmount = basePrice * (ReturningClientDiscount / 100);
            totalDiscountAmount += returningClientDiscountAmount;
        }

        return totalDiscountAmount;
    }
}