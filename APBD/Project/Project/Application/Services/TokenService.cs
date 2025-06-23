using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Project.Application.Services.Interfaces;
using Project.Core.Models;
using Project.Application.DTOs.Responses;
using Project.Application.Helpers;
using Project.Application.Services.Interfaces;
using Project.Core.Models;
using Project.Infrastructure;

namespace Project.Application.Services;

public class TokenService : ITokenService
{
    private readonly AppDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(
        AppDbContext context,
        IDateTimeProvider dateTimeProvider,
        IConfiguration configuration)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
        _secretKey = configuration["SecretKey"] ?? throw new Exception("SecretKey missing.");
        _issuer = configuration["Jwt:Issuer"] ?? throw new Exception("Issuer missing.");
        _audience = configuration["Jwt:Audience"] ?? throw new Exception("Audience missing.");
    }
    
    public async Task<AuthResponse> GenerateTokens(User user, CancellationToken cancellationToken = default)
    {
        
        var userClaims = new List<Claim>
        {  
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username)
        };
        userClaims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: userClaims,
            expires: _dateTimeProvider.Now().AddMinutes(30),
            signingCredentials: creds
        );
        
        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = _dateTimeProvider.Now().AddDays(7);
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = user.RefreshToken
        };
    }
}