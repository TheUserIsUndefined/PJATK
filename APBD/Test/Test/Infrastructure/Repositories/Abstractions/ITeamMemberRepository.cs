using Test.Entities;

namespace Test.Infrastructure.Repositories.Abstractions;

public interface ITeamMemberRepository
{
    public Task<TeamMember?> GetTeamMemberAsync(int teamMemberId, CancellationToken token);
    public Task<bool> DoesTeamMemberExistByIdAsync(int teamMemberId, CancellationToken token);
}