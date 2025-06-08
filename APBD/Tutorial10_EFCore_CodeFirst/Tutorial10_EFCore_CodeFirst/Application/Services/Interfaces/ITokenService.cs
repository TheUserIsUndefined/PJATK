using Tutorial10_EFCore_CodeFirst.Application.DTOs.Responses;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

public interface ITokenService
{
    public Task<AuthResponse> GenerateTokens(User user, CancellationToken cancellationToken = default);
}