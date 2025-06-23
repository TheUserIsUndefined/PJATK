using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.Application.DTOs.Requests;
using Project.Application.DTOs.Responses;
using Project.Application.Exceptions;
using Project.Application.Helpers;
using Project.Application.Services.Interfaces;
using Project.Core.Models;
using Project.Infrastructure;

namespace Project.Application.Services;

public class UserService : IUserService
{
    private const string DefaultRegisterRole = "Employee";
    
    private readonly AppDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITokenService _tokenService;

    public UserService(
        AppDbContext context,
        IDateTimeProvider dateTimeProvider,
        ITokenService tokenService)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
        _tokenService = tokenService;
    }
    
    public async Task RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var userByUsername = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);
        
        if (userByUsername is not null)
            throw new UserExceptions
                .UserValidationException($"User with username {request.Username} already exists.");
        
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(request.Password);
        
        var defaultRoleId = await _context.Roles
            .Where(r => r.Name == DefaultRegisterRole)
            .Select(r => r.RoleId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (defaultRoleId == 0)
            throw new RoleExceptions.DefaultRoleNotFoundException(DefaultRegisterRole);

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = new User
            {
                Username = request.Username,
                Password = hashedPasswordAndSalt.Item1,
                Salt = hashedPasswordAndSalt.Item2,
                RefreshToken = SecurityHelpers.GenerateRefreshToken(),
                RefreshTokenExp = _dateTimeProvider.Now().AddDays(7)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            var userRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = defaultRoleId
            };
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<AuthResponse> LoginUserAsync(LoginUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);
        
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
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);

        if (user is null)
            throw new SecurityTokenException("Invalid refresh token");
        if (user.RefreshTokenExp < _dateTimeProvider.Now())
            throw new SecurityTokenException("Refresh token expired");
        
        return await _tokenService.GenerateTokens(user, cancellationToken);
    }
}