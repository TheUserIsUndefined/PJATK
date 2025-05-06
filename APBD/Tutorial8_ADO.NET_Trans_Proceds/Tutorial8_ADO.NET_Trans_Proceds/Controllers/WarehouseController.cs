using Microsoft.AspNetCore.Mvc;
using Tutorial8_ADO.NET_Trans_Proceds.Contracts.Requests;
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
        var result = await _warehouseService.AddProductToWarehouseAsync(request, token);

        return 
    }
}