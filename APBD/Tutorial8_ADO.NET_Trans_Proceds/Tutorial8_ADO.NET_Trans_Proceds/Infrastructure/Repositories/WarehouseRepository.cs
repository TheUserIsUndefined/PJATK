using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial8_ADO.NET_Trans_Proceds.Contracts.DTOs;
using Tutorial8_ADO.NET_Trans_Proceds.Contracts.Requests;
using Tutorial8_ADO.NET_Trans_Proceds.Infrastructure;
using Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;
using Tutorial8_ADO.NET_Trans_Proceds.Services.Interfaces;

namespace Tutorial8_ADO.NET_Trans_Proceds.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseRepository(IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> DoesWarehouseExistByIdAsync(int warehouseId, CancellationToken token)
    {
        var query = """
                    select
                        iif(exists(select 1
                            from Warehouse
                            where IdWarehouse = @warehouseId), 1, 0)
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        command.Parameters.AddWithValue("@warehouseId", warehouseId);

        var result = Convert.ToInt32(await command.ExecuteScalarAsync(token));
        return result == 1;
    }

    public async Task<bool> HasOrderBeenCompletedAsync(int orderId, CancellationToken token)
    {
        var query = """
                    select 
                        iif(exists(select 1
                            from Product_Warehouse
                            where IdOrder = @orderId), 1, 0)
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        command.Parameters.AddWithValue("@orderId", orderId);
        
        var result = Convert.ToInt32(await command.ExecuteScalarAsync(token));
        
        return result == 1;
    }

    public async Task<int> InsertWarehouseProductAsync(InsertWarehouseProductDto dto, CancellationToken token)
    {
        var query = """
                    insert into Product_Warehouse
                        (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
                    values (@idWarehouse, @idProduct, @idOrder, @amount, @price, @createdAt)
                    select scope_identity();
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        command.Parameters.AddWithValue("@idWarehouse", dto.IdWarehouse);
        command.Parameters.AddWithValue("@idProduct", dto.IdProduct);
        command.Parameters.AddWithValue("@idOrder", dto.IdOrder);
        command.Parameters.AddWithValue("@amount", dto.Amount);
        command.Parameters.AddWithValue("@price", dto.Price);
        command.Parameters.AddWithValue("@createdAt", _dateTimeProvider.Now());
        
        var result = Convert.ToInt32(await command.ExecuteScalarAsync(token));
        
        return result;
    }

    public async Task<int> AddProductToWarehouseUsingProcAsync(AddProductToWarehouseRequest request, CancellationToken token)
    {
        var connection = await _unitOfWork.GetConnectionAsync();
        var command = new SqlCommand("AddProductToWarehouse", connection);
        command.Transaction = _unitOfWork.Transaction;
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
        command.Parameters.AddWithValue("@IdProduct", request.IdProduct);
        command.Parameters.AddWithValue("@Amount", request.Amount);
        command.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);
        
        var result = Convert.ToInt32(await command.ExecuteScalarAsync(token));
        
        return result;
    }
}