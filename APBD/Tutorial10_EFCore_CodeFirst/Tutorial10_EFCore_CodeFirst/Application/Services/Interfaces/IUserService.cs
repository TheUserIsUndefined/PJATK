using Tutorial10_EFCore_CodeFirst.Application.DTOs.Requests;
using Tutorial10_EFCore_CodeFirst.Application.DTOs.Responses;

namespace Tutorial10_EFCore_CodeFirst.Application.Services.Interfaces;

public interface IUserService
{
    public Task RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
    
    public Task<AuthResponse> LoginUserAsync(LoginUserRequest request,
        CancellationToken cancellationToken = default);

    public Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request,
        CancellationToken cancellationToken = default);
}