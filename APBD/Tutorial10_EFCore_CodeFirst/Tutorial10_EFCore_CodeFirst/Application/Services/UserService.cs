using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;
using Tutorial10_EFCore_CodeFirst.Application.DTOs.Responses;
using Tutorial10_EFCore_CodeFirst.Application.Exceptions;
using Tutorial10_EFCore_CodeFirst.Application.Helpers;
using Tutorial10_EFCore_CodeFirst.Application.Repositories;
using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Application.Services;

public class UserService : IUserService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public UserService(
        IDateTimeProvider dateTimeProvider,
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _dateTimeProvider = dateTimeProvider;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(request.Password);

        var user = new User
        {
            Username = request.Username,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = _dateTimeProvider.Now().AddDays(7)
        };
        
        await _userRepository.AddUserAsync(user, cancellationToken);
    }

    public async Task<AuthResponse> LoginUserAsync(LoginUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
            throw new UserExceptions.UserNotFoundException(request.Username);
        
        var passwordHashFromDb = user.Password;
        var currentPasswordHash = SecurityHelpers.GetHashedPasswordWithSalt(request.Password, user.Salt);
        if (currentPasswordHash != passwordHashFromDb)
            throw new UserExceptions.WrongPasswordException();

        return await _tokenService.GenerateTokens(user, cancellationToken);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (user is null)
            throw new SecurityTokenException("Invalid refresh token");

        if (user.RefreshTokenExp < _dateTimeProvider.Now())
            throw new SecurityTokenException("Refresh token expired");
        
        return await _tokenService.GenerateTokens(user, cancellationToken);
    }
}