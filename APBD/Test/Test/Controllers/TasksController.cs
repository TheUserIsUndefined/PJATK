using Microsoft.AspNetCore.Mvc;
using Test.Contracts.Requests;
using Test.Exceptions;
using Test.Services.Abstractions;

namespace Test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITasksService _tasksService;

    public TasksController(ITasksService tasksService)
    {
        _tasksService = tasksService;
    }
    
    [HttpGet("{teamMemberId:int}")]
    public async Task<IActionResult> GetTeamMemberTasksAsync(int teamMemberId, CancellationToken token = default)
    {
        try
        {
            var response = await _tasksService.GetTeamMemberTasksAsync(teamMemberId, token);
            
            return Ok(response);
        }
        catch (Exception e) when (e is TeamMemberDoesNotExistException)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> InsertTaskAsync([FromBody] InsertTaskRequest request,
        CancellationToken token = default)
    {
        try
        {
            var taskId = await _tasksService.InsertTaskAsync(request, token);
            return Created($"api/tasks/{taskId}", taskId);
        }
        catch (Exception e) when (e is TaskTypeDoesNotExistException
                                      or ProjectDoesNotExistException
                                      or TeamMemberDoesNotExistException)
        {
            return BadRequest(e.Message);
        }
    }
}