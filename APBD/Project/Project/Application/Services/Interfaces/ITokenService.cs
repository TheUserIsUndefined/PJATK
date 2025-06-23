using Project.Application.DTOs.Responses;
using Project.Core.Models;

namespace Project.Application.Services.Interfaces;

public interface ITokenService
{
    public Task<AuthResponse> GenerateTokens(User user, CancellationToken cancellationToken = default);
}