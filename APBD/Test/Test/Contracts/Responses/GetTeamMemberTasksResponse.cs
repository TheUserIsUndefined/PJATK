using Test.Entities;

namespace Test.Contracts.Responses;

public class GetTeamMemberTasksResponse
{
    public TeamMember Member { get; set; }
    public List<TaskResponse> AssignedTasks { get; set; }
    public List<TaskResponse> CreatedTasks { get; set; }
}