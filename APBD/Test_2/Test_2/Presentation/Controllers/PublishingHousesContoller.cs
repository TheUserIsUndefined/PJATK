using Microsoft.AspNetCore.Mvc;
using Test_2.Application.Services.Interfaces;

namespace Test_2.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublishingHousesController : ControllerBase
{
    private readonly IPublishingHouseService _publishingHouseService;

    public PublishingHousesController(IPublishingHouseService publishingHouseService)
    {
        _publishingHouseService = publishingHouseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPublishingHousesAsync(
        [FromQuery] string? city,
        [FromQuery] string? country,
        CancellationToken cancellation = default)
    {
        var response = await _publishingHouseService
            .GetPublishingHousesAsync(city, country, cancellation);
        
        return Ok(response);
    }
}