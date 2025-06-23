namespace Project.Application.Exceptions;

public class InvalidCurrencyException(string? currency) : 
    BaseExceptions.NotFoundException($"Invalid currency '{currency}' specified.");