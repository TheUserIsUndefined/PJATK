namespace Test.Exceptions;

public class TaskTypeDoesNotExistException(int TaskTypeId) : Exception($"Task type {TaskTypeId} does not exist.");