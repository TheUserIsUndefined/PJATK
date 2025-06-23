using Microsoft.EntityFrameworkCore;
using Project.Application.DTOs.Requests;
using Project.Application.Exceptions;
using Project.Application.Services.Interfaces;
using Project.Core.Models;
using Project.Infrastructure;

namespace Project.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IContractService _contractService;

    public PaymentService(
        AppDbContext context,
        IDateTimeProvider dateTimeProvider,
        IContractService contractService
        )
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
        _contractService = contractService;
    }
    
    public async Task<int> CreatePaymentAsync(
        int contractId,
        CreatePaymentRequest request,
        CancellationToken cancellationToken = default
        )
    {
        var contract = await _context.Contracts
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.ContractId == contractId, cancellationToken);
        
        if (contract == null)
            throw new ContractExceptions.ContractNotFoundByIdException(contractId);
        
        if (contract.EndDate < _dateTimeProvider.Today())
        {
            foreach (var payment in contract.Payments)
                if (payment.Status == PaymentStatus.Paid)
                {
                    payment.Status = PaymentStatus.Refunded;
                    payment.RefundDate = _dateTimeProvider.Now();
                }
            await _context.SaveChangesAsync(cancellationToken);
            
            var startDate = _dateTimeProvider.Today();
            var contractDuration = contract.EndDate.DayNumber - contract.StartDate.DayNumber;
            var endDate = startDate.AddDays(contractDuration);

            var createContractRequest = new CreateContractRequest
            {
                ClientId = contract.ClientId,
                SoftwareId = contract.SoftwareId,
                StartDate = startDate,
                EndDate = endDate,
                AdditionalSupportYears = contract.SupportYears - 1,
                UpdatesInformation = contract.UpdatesInformation
            };
            await _contractService.CreateContractAsync(createContractRequest, cancellationToken);
            
            throw new ContractExceptions
                .ContractPaymentExpiredException(contractId, contract.EndDate, contract.ContractId);
        }

        var paidAmount = contract.Payments
            .Where(p => p.Status == PaymentStatus.Paid)
            .Sum(p => p.Amount);
        
        var remainingAmount = contract.Price - paidAmount;
        
        if (request.Amount > remainingAmount)
            throw new PaymentExceptions.PaymentExceedsContractException(request.Amount, remainingAmount);
        
        if (remainingAmount <= 0)
            throw new ContractExceptions.ContractAlreadyPaidException(contractId);
        
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var payment = new Payment
            {
                ContractId = contractId,
                PaymentDate = _dateTimeProvider.Now(),
                Amount = request.Amount,
                Status = PaymentStatus.Paid
            };
            await _context.Payments.AddAsync(payment, cancellationToken);

            paidAmount += request.Amount;
            if (paidAmount == contract.Price)
            {
                contract.Status = ContractStatus.Signed;
            }

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return payment.PaymentId;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}