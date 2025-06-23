using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.Application.DTOs.Requests;
using Project.Application.Exceptions;
using Project.Application.Services.Interfaces;

namespace Project.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        await _userService.RegisterUserAsync(request, cancellationToken);
        
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> LoginUserAsync(LoginUserRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _userService.LoginUserAsync(request, cancellationToken);

            return Ok(response);
        }
        catch (BaseExceptions.NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UserExceptions.WrongPasswordException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _userService.RefreshTokenAsync(request, cancellationToken);

            return Ok(response);
        }
        catch (SecurityTokenException e)
        {
            return Unauthorized(e.Message);
        }
    }
}