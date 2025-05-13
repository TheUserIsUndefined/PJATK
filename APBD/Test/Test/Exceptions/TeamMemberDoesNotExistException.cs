namespace Test.Exceptions;

public class TeamMemberDoesNotExistException(int teamMemberId) : Exception($"Team Member {teamMemberId} does not exist.");