using Microsoft.AspNetCore.Mvc;
using Project.Application.DTOs.Requests;
using Project.Application.Services.Interfaces;

namespace Project.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    
    [HttpPost]
    public async Task<IActionResult> AddClientAsync(AddClientRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var clientId = await _clientService.AddClientAsync(request, cancellationToken);
            
            return Created($"api/clients/{clientId}", clientId);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}