using Project.Application.DTOs.Responses;
using Project.Application.DTOs.Requests;

namespace Project.Application.Services.Interfaces;

public interface IUserService
{
    public Task RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
    
    public Task<AuthResponse> LoginUserAsync(LoginUserRequest request,
        CancellationToken cancellationToken = default);

    public Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request,
        CancellationToken cancellationToken = default);
}