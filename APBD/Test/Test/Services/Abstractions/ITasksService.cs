using Test.Contracts.Requests;
using Test.Contracts.Responses;

namespace Test.Services.Abstractions;

public interface ITasksService
{
    public Task<GetTeamMemberTasksResponse> GetTeamMemberTasksAsync(int teamMemberId, CancellationToken token);
    public Task<int> InsertTaskAsync(InsertTaskRequest request, CancellationToken token);
}