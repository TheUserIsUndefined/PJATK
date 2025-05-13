using Microsoft.Data.SqlClient;
using Test.Entities;
using Test.Infrastructure.Repositories.Abstractions;

namespace Test.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ProjectRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<Project>> GetProjectsAsync(CancellationToken token)
    {
        var projects = new List<Project>();
        
        var query = """
                    select IdProject,
                        Name,
                        Deadline
                    from Project
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        
        await using var reader = await command.ExecuteReaderAsync(token);

        while (await reader.ReadAsync(token))
        {
            var project = new Project
            {
                IdProject = reader.GetInt32(0),
                Name = reader.GetString(1),
                Deadline = reader.GetDateTime(2)
            };
            
            projects.Add(project);
        }
        
        return projects;
    }

    public async Task<bool> DoesProjectExistByIdAsync(int projectId, CancellationToken token)
    {
        var query = """
                             SELECT 
                                 IIF(EXISTS (SELECT 1 FROM Project 
                                         WHERE IdProject = @projectId), 1, 0);   
                             """;

        var connection = await _unitOfWork.GetConnectionAsync();
        await using var cmd = new SqlCommand(query, connection);
        cmd.Transaction = _unitOfWork.Transaction;
        
        cmd.Parameters.AddWithValue("@projectId", projectId);

        var result = (int)await cmd.ExecuteScalarAsync(token);
        return result == 1;
    }
}