using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Exceptions;
using Project.Application.Services.Interfaces;

namespace Project.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;
    
    public RevenueController(IRevenueService revenueService) => _revenueService = revenueService;

    [Authorize(Roles = "Employee")]
    [HttpGet("current")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetTotalRevenueAsync(
        int? softwareId = null,
        string? currency = null,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            var result = await _revenueService.GetCurrentRevenueAsync(softwareId, currency, cancellationToken);

            return Ok(result);
        }
        catch (BaseExceptions.NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [Authorize(Roles = "Employee")]
    [HttpGet("predicted")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetPredictedRevenueAsync(
        int? softwareId = null,
        string? currency = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var result = await _revenueService.GetPredictedRevenueAsync(softwareId, currency, cancellationToken);

            return Ok(result);
        }
        catch (BaseExceptions.NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}