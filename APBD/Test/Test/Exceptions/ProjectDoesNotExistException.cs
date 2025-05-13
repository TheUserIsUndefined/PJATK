namespace Test.Exceptions;

public class ProjectDoesNotExistException(int idProject) : Exception($"Project {idProject} does not exist.");