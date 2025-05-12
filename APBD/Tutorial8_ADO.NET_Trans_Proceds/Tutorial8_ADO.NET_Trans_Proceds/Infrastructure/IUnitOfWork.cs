using Microsoft.Data.SqlClient;

namespace Tutorial8_ADO.NET_Trans_Proceds.Infrastructure;

public interface IUnitOfWork
{
    public ValueTask<SqlConnection> GetConnectionAsync();
    public SqlTransaction? Transaction { get; }
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
}