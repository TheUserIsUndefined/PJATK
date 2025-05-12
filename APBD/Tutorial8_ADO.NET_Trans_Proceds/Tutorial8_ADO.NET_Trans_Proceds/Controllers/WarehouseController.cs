using Microsoft.AspNetCore.Mvc;
using Tutorial8_ADO.NET_Trans_Proceds.Contracts.Requests;
using Tutorial8_ADO.NET_Trans_Proceds.Exceptions;
using Tutorial8_ADO.NET_Trans_Proceds.Services.Interfaces;

namespace Tutorial8_ADO.NET_Trans_Proceds.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToWarehouseAsync([FromBody] AddProductToWarehouseRequest request,
        CancellationToken token = default)
    {
        try
        {
            var result = await _warehouseService.AddProductToWarehouseAsync(request, token);

            return Created($"/api/Warehouse/{request.IdWarehouse}/Product/{result}", result);
        }
        catch (Exception e) when (e is ProductDoesNotExistException
                                      or OrderDoesNotExistException
                                      or WarehouseDoesNotExistException
                                      or OrderHasBeenCompletedException)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("procedure")]
    public async Task<IActionResult> AddProductToWarehouseUsingProcAsync(
        [FromBody] AddProductToWarehouseRequest request,
        CancellationToken token = default)
    {
        try
        {
            var result = await _warehouseService.AddProductToWarehouseUsingProcAsync(request, token);
            return Created($"/api/Warehouse/{request.IdWarehouse}/Product/{result}", result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}