using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Application.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    public Task<int> AddUserAsync(User user, CancellationToken cancellationToken = default);
    public Task UpdateUserAsync(User user, CancellationToken cancellationToken = default);
}