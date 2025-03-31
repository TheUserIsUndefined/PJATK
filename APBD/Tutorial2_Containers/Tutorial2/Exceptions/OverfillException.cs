namespace Tutorial2.Exceptions;

public class OverfillException(string message = "") : Exception(message);