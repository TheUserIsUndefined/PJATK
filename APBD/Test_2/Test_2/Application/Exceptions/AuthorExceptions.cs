namespace Test_2.Application.Exceptions;

public static class AuthorExceptions
{
    public class AuthorNotFoundException(int authorId)
        : BaseException.NotFoundException($"Author {authorId} not found.");
}