using Microsoft.Data.SqlClient;
using Test.Entities;
using Test.Infrastructure.Repositories.Abstractions;

namespace Test.Infrastructure.Repositories;

public class TaskTypeRepository : ITaskTypeRepository
{
    private readonly IUnitOfWork _unitOfWork;
    
    public TaskTypeRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


    public async Task<IEnumerable<TaskType>> GetTaskTypesAsync(CancellationToken token)
    {
        var taskTypes = new List<TaskType>();
        
        var query = """
                    select IdTaskType,
                        Name
                    from TaskType
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        
        await using var reader = await command.ExecuteReaderAsync(token);

        while (await reader.ReadAsync(token))
        {
            var taskType = new TaskType
            {
                IdTaskType = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
            
            taskTypes.Add(taskType);
        }
        
        return taskTypes;
    }

    public async Task<bool> DoesTaskTypeExistByIdAsync(int taskTypeId, CancellationToken token)
    {
        var query = """
                    SELECT 
                        IIF(EXISTS (SELECT 1 FROM TaskType 
                                WHERE IdTaskType = @taskTypeId), 1, 0);   
                    """;

        var connection = await _unitOfWork.GetConnectionAsync();
        await using var cmd = new SqlCommand(query, connection);
        cmd.Transaction = _unitOfWork.Transaction;
        
        cmd.Parameters.AddWithValue("@taskTypeId", taskTypeId);

        var result = (int)await cmd.ExecuteScalarAsync(token);
        return result == 1;
    }
}