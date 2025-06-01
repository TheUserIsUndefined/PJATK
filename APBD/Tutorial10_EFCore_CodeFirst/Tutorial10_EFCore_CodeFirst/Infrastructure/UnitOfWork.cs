using Microsoft.EntityFrameworkCore.Storage;
using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

namespace Tutorial10_EFCore_CodeFirst.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly PrescriptionContext _context;
    public IDbContextTransaction? Transaction { get; private set; }

    public UnitOfWork(PrescriptionContext context)
    {
        _context = context;
    }

    public void BeginTransaction() => 
        Transaction = _context.Database.BeginTransaction();

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (Transaction is not null)
                await Transaction.CommitAsync();
        }
        finally
        {
            await DisposeAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (Transaction is not null)
                await Transaction.RollbackAsync();
        }
        finally
        {
            await DisposeAsync();
        }
    }

    private async Task DisposeAsync()
    {
        if (Transaction is not null)
            await Transaction.DisposeAsync();
    }
}