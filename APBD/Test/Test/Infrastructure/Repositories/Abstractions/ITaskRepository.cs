using Test.Contracts.Requests;
using Task = Test.Entities.Task;

namespace Test.Infrastructure.Repositories.Abstractions;

public interface ITaskRepository
{
    public Task<IEnumerable<Task>> GetAssignedTasksByMemberIdAsync(int teamMemberId,
        CancellationToken token);
    public Task<IEnumerable<Task>> GetCreatedTasksByMemberIdAsync(int teamMemberId,
        CancellationToken token);
    public Task<int> CreateTaskAsync(InsertTaskRequest request, CancellationToken token);
}