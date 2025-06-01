using Microsoft.EntityFrameworkCore.Storage;

namespace Tutorial9_EFCore_DBFirst.DAL.Infrastructure;

public interface IUnitOfWork
{
    public IDbContextTransaction? Transaction { get; }
    
    public void BeginTransaction();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
}