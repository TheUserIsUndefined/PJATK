using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Services.Interfaces;

namespace Project.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    public IDbContextTransaction? Transaction { get; private set; }

    public UnitOfWork(DbContext context)
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