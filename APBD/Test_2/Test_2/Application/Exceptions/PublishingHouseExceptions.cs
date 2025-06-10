namespace Test_2.Application.Exceptions;

public static class PublishingHouseExceptions
{
    public class PublishingHouseNotFoundException(int publishingHouseId)
        : BaseException.NotFoundException($"Publishing house {publishingHouseId} not found.");
}