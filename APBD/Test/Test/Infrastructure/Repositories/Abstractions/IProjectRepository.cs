using Test.Entities;

namespace Test.Infrastructure.Repositories.Abstractions;

public interface IProjectRepository
{
    public Task<IEnumerable<Project>> GetProjectsAsync(CancellationToken token);
    public Task<bool> DoesProjectExistByIdAsync(int projectId, CancellationToken token);
}