using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Tutorial10_EFCore_CodeFirst.Application.DTOs.Responses;
using Tutorial10_EFCore_CodeFirst.Application.Helpers;
using Tutorial10_EFCore_CodeFirst.Application.Repositories;
using Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Application.Services;

public class TokenService : ITokenService
{
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(
        IUserRepository userRepository, 
        IDateTimeProvider dateTimeProvider,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
        _secretKey = configuration["SecretKey"] ?? throw new Exception("SecretKey missing.");
        _issuer = configuration["Jwt:Issuer"] ?? throw new Exception("Issuer missing.");
        _audience = configuration["Jwt:Audience"] ?? throw new Exception("Audience missing.");
    }
    
    public async Task<AuthResponse> GenerateTokens(User user, CancellationToken cancellationToken = default)
    {
        var userClaims = new[]  
        {  
            new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: userClaims,
            expires: _dateTimeProvider.Now().AddMinutes(10),
            signingCredentials: creds
        );
        
        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = _dateTimeProvider.Now().AddDays(7);
        await _userRepository.UpdateUserAsync(user, cancellationToken);

        return new AuthResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = user.RefreshToken
        };
    }
}