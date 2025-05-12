using Tutorial8_ADO.NET_Trans_Proceds.Contracts.DTOs;
using Tutorial8_ADO.NET_Trans_Proceds.Contracts.Requests;
using Tutorial8_ADO.NET_Trans_Proceds.Exceptions;
using Tutorial8_ADO.NET_Trans_Proceds.Infrastructure;
using Tutorial8_ADO.NET_Trans_Proceds.Repositories.Interfaces;
using Tutorial8_ADO.NET_Trans_Proceds.Services.Interfaces;

namespace Tutorial8_ADO.NET_Trans_Proceds.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IWarehouseRepository warehouseRepository, 
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _warehouseRepository = warehouseRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> AddProductToWarehouseAsync(AddProductToWarehouseRequest request, CancellationToken token)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var productId = request.IdProduct;
            var productPrice = await _productRepository.GetProductPriceAsync(productId, token);
            if (productPrice == null)
                throw new ProductDoesNotExistException(productId);
            
            var warehouseId = request.IdWarehouse;
            if (!await _warehouseRepository.DoesWarehouseExistByIdAsync(warehouseId, token))
                throw new WarehouseDoesNotExistException(warehouseId);

            var orderDto = new ProductOrderCheckDto
            {
                IdProduct = request.IdProduct,
                CreatedAt = request.CreatedAt,
                Amount = request.Amount
            };
            var orderId = await _orderRepository.GetOrderIdAsync(orderDto, token);
            if (orderId == 0)
                throw new OrderDoesNotExistException(productId, orderDto.Amount);
            
            if (await _warehouseRepository.HasOrderBeenCompletedAsync(orderId, token))
                throw new OrderHasBeenCompletedException(orderId);
            
            await _orderRepository.UpdateOrderFulfilledAtAsync(orderId, token);

            var insertDto = new InsertWarehouseProductDto
            {
                IdProduct = productId,
                IdWarehouse = request.IdWarehouse,
                IdOrder = orderId,
                Amount = request.Amount,
                Price = Convert.ToDouble(productPrice) * request.Amount
            };
            var result = await _warehouseRepository.InsertWarehouseProductAsync(insertDto, token);

            await _unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<int> AddProductToWarehouseUsingProcAsync(AddProductToWarehouseRequest request, CancellationToken token)
    {
        return await _warehouseRepository.AddProductToWarehouseUsingProcAsync(request, token);
    }
}