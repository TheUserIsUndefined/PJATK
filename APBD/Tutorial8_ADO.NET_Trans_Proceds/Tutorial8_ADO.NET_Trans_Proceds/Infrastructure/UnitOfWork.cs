using Microsoft.Data.SqlClient;

namespace Tutorial8_ADO.NET_Trans_Proceds.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    public SqlConnection Connection { get; }
    public SqlTransaction Transaction { get; }
}