using Microsoft.EntityFrameworkCore.Storage;

namespace Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

public interface IUnitOfWork
{
    public IDbContextTransaction? Transaction { get; }
    
    public void BeginTransaction();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
}