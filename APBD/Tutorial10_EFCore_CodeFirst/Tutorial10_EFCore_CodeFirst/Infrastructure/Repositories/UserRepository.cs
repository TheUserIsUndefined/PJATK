using Microsoft.EntityFrameworkCore;
using Tutorial10_EFCore_CodeFirst.Application.Repositories;
using Tutorial10_EFCore_CodeFirst.Core.Models;

namespace Tutorial10_EFCore_CodeFirst.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PrescriptionContext _context;

    public UserRepository(PrescriptionContext context) => _context = context;
    
    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
    }

    public async Task<int> AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.IdUser;
    }

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        return _context.SaveChangesAsync(cancellationToken);
    }
}