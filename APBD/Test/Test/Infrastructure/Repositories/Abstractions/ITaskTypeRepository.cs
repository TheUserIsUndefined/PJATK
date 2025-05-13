using Test.Entities;

namespace Test.Infrastructure.Repositories.Abstractions;

public interface ITaskTypeRepository
{
    public Task<IEnumerable<TaskType>> GetTaskTypesAsync(CancellationToken token);
    public Task<bool> DoesTaskTypeExistByIdAsync(int taskTypeId, CancellationToken token);
}