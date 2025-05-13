using Microsoft.Data.SqlClient;
using Test.Entities;
using Test.Infrastructure.Repositories.Abstractions;

namespace Test.Infrastructure.Repositories;

public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly IUnitOfWork _unitOfWork;
    public TeamMemberRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<TeamMember?> GetTeamMemberAsync(int teamMemberId, CancellationToken token)
    {
        TeamMember? teamMember = null;

        var query = """
                    select IdTeamMember,
                           FirstName,
                           LastName,
                           Email
                    from TeamMember
                    where IdTeamMember = @teamMemberId
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        
        command.Parameters.AddWithValue("@teamMemberId", teamMemberId);
        
        await using var reader = await command.ExecuteReaderAsync(token);
        if (await reader.ReadAsync(token))
            teamMember = new TeamMember
            {
                IdTeamMember = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3)
            };
        
        return teamMember;
    }

    public async Task<bool> DoesTeamMemberExistByIdAsync(int teamMemberId, CancellationToken token)
    {
        var query = """
                    SELECT 
                        IIF(EXISTS (SELECT 1 FROM TeamMember 
                                WHERE IdTeamMember = @teamMemberId), 1, 0);   
                    """;

        var connection = await _unitOfWork.GetConnectionAsync();
        await using var cmd = new SqlCommand(query, connection);
        cmd.Transaction = _unitOfWork.Transaction;
        
        cmd.Parameters.AddWithValue("@teamMemberId", teamMemberId);

        var result = (int)await cmd.ExecuteScalarAsync(token);
        return result == 1;
    }
}