using Microsoft.Data.SqlClient;
using Test.Contracts.Requests;
using Test.Infrastructure.Repositories.Abstractions;
using Task = Test.Entities.Task;


namespace Test.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IUnitOfWork _unitOfWork;
    
    public TaskRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<Task>> GetAssignedTasksByMemberIdAsync(int teamMemberId, CancellationToken token)
    {
        var tasks = new List<Task>();

        var query = """
                    select IdTask,
                           Name,
                           Description,
                           Deadline,
                           IdProject,
                           IdTaskType,
                           IdAssignedTo,
                           IdCreator
                    from Task
                    where IdAssignedTo = @teamMemberId
                    order by Deadline desc
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        
        command.Parameters.AddWithValue("@teamMemberId", teamMemberId);
        
        await using var reader = await command.ExecuteReaderAsync(token);

        while (await reader.ReadAsync(token))
        {
            var task = new Task
            {
                IdTask = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Deadline = reader.GetDateTime(3),
                IdProject = reader.GetInt32(4),
                IdTaskType = reader.GetInt32(5),
                IdAssignedTo = reader.GetInt32(6),
                IdCreator = reader.GetInt32(7),
            };
            
            tasks.Add(task);
        }
        
        return tasks;
    }

    public async Task<IEnumerable<Task>> GetCreatedTasksByMemberIdAsync(int teamMemberId, CancellationToken token)
    {
        var tasks = new List<Task>();

        var query = """
                    select IdTask,
                           Name,
                           Description,
                           Deadline,
                           IdProject,
                           IdTaskType,
                           IdAssignedTo,
                           IdCreator
                    from Task
                    where IdCreator = @teamMemberId
                    order by Deadline desc
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        
        command.Parameters.AddWithValue("@teamMemberId", teamMemberId);
        
        await using var reader = await command.ExecuteReaderAsync(token);

        while (await reader.ReadAsync(token))
        {
            var task = new Task
            {
                IdTask = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Deadline = reader.GetDateTime(3),
                IdProject = reader.GetInt32(4),
                IdTaskType = reader.GetInt32(5),
                IdAssignedTo = reader.GetInt32(6),
                IdCreator = reader.GetInt32(7),
            };
            
            tasks.Add(task);
        }
        
        return tasks;
    }

    public async Task<int> CreateTaskAsync(InsertTaskRequest request, CancellationToken token)
    {
        var query = """
                    insert into Task (Name, Description, Deadline, IdProject, IdTaskType, IdAssignedTo, IdCreator)
                    values(@name, @description, @deadline, @idProject, @idTaskType, @idAssignedTo, @idCreator)
                    select SCOPE_IDENTITY()
                    """;
        var connection = await _unitOfWork.GetConnectionAsync();
        await using var command = new SqlCommand(query, connection);
        command.Transaction = _unitOfWork.Transaction;
        
        command.Parameters.AddWithValue("@name", request.Name);
        command.Parameters.AddWithValue("@description", request.Description);
        command.Parameters.AddWithValue("@deadline", request.Deadline);
        command.Parameters.AddWithValue("@idProject", request.IdProject);
        command.Parameters.AddWithValue("@idTaskType", request.IdTaskType);
        command.Parameters.AddWithValue("@idAssignedTo", request.IdAssignedTo);
        command.Parameters.AddWithValue("@idCreator", request.IdCreator);
        
        var result = await command.ExecuteScalarAsync(token);
        
        return Convert.ToInt32(result);
    }
}