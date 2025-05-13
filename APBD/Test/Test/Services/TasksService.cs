using Test.Contracts.Requests;
using Test.Contracts.Responses;
using Test.Exceptions;
using Test.Infrastructure;
using Test.Infrastructure.Repositories.Abstractions;
using Test.Services.Abstractions;

namespace Test.Services;

public class TasksService : ITasksService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _projectRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskTypeRepository _taskTypeRepository;

    public TasksService(IProjectRepository projectRepository,
        ITeamMemberRepository teamMemberRepository,
        ITaskRepository taskRepository,
        ITaskTypeRepository taskTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _projectRepository = projectRepository;
        _teamMemberRepository = teamMemberRepository;
        _taskRepository = taskRepository;
        _taskTypeRepository = taskTypeRepository;
    }


    public async Task<GetTeamMemberTasksResponse> GetTeamMemberTasksAsync(int teamMemberId, CancellationToken token)
    {
        if (teamMemberId < 1)
            throw new TeamMemberDoesNotExistException(teamMemberId);
        
        var teamMember = await _teamMemberRepository.GetTeamMemberAsync(teamMemberId, token);
        if(teamMember == null)
            throw new TeamMemberDoesNotExistException(teamMemberId);
        
        var assignedTasks = await _taskRepository.GetAssignedTasksByMemberIdAsync(teamMemberId, token);
        var createdTasks = await _taskRepository.GetCreatedTasksByMemberIdAsync(teamMemberId, token);
        
        var projects = await _projectRepository.GetProjectsAsync(token);
        var taskTypes = await _taskTypeRepository.GetTaskTypesAsync(token);

        var assignedTasksResponse = new List<TaskResponse>();
        foreach (var assignedTask in assignedTasks)
        {
            var taskResponse = new TaskResponse()
            {
                Name = assignedTask.Name,
                Description = assignedTask.Description,
                Deadline = assignedTask.Deadline,
                ProjectName = projects.Where(p => p.IdProject == assignedTask.IdProject)
                    .Select(p => p.Name)
                    .FirstOrDefault(),
                TaskType = taskTypes.Where(tp => tp.IdTaskType == assignedTask.IdTaskType)
                    .Select(tp => tp.Name)
                    .FirstOrDefault()
            };
            assignedTasksResponse.Add(taskResponse);
        }
        
        var createdTasksResponse = new List<TaskResponse>();
        foreach (var createdTask in createdTasks)
        {
            var taskResponse = new TaskResponse()
            {
                Name = createdTask.Name,
                Description = createdTask.Description,
                Deadline = createdTask.Deadline,
                ProjectName = projects.Where(p => p.IdProject == createdTask.IdProject)
                    .Select(p => p.Name)
                    .FirstOrDefault(),
                TaskType = taskTypes.Where(tp => tp.IdTaskType == createdTask.IdTaskType)
                    .Select(tp => tp.Name)
                    .FirstOrDefault()
            };
            createdTasksResponse.Add(taskResponse);
        }

        var response = new GetTeamMemberTasksResponse()
        {
            Member = teamMember,
            AssignedTasks = assignedTasksResponse,
            CreatedTasks = createdTasksResponse,
        };
        
        return response;
    }

    public async Task<int> InsertTaskAsync(InsertTaskRequest request, CancellationToken token)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var projectId = request.IdProject;
            if (!await _projectRepository.DoesProjectExistByIdAsync(projectId, token))
                throw new ProjectDoesNotExistException(projectId);

            var taskTypeId = request.IdTaskType;
            if (!await _taskTypeRepository.DoesTaskTypeExistByIdAsync(taskTypeId, token))
                throw new TaskTypeDoesNotExistException(taskTypeId);

            var assignedToId = request.IdAssignedTo;
            if (!await _teamMemberRepository.DoesTeamMemberExistByIdAsync(assignedToId, token))
                throw new TeamMemberDoesNotExistException(assignedToId);

            var creatorId = request.IdCreator;
            if (!await _teamMemberRepository.DoesTeamMemberExistByIdAsync(creatorId, token))
                throw new TeamMemberDoesNotExistException(creatorId);
            
            var taskId = await _taskRepository.CreateTaskAsync(request, token);

            await _unitOfWork.CommitTransactionAsync();
            return taskId;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}