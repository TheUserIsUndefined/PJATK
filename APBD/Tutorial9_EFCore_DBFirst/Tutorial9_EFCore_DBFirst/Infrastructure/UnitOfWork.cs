using Microsoft.EntityFrameworkCore.Storage;

namespace Tutorial9_EFCore_DBFirst.DAL.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly TripsDbContext _context;
    public IDbContextTransaction? Transaction { get; private set; }

    public UnitOfWork(TripsDbContext context)
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