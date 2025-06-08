using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;
using Tutorial10_EFCore_CodeFirst.Application.Exceptions;
using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

namespace Tutorial10_EFCore_CodeFirst.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        await _userService.RegisterUserAsync(request, cancellationToken);
        
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
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