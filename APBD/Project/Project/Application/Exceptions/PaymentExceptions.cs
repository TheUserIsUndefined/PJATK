namespace Project.Application.Exceptions;

public static class PaymentExceptions
{
    public class PaymentExceedsContractException(decimal amount, decimal remainingAmount) :
        BaseExceptions.ValidationException(
            $"Payment amount ({amount}) exceeds remaining contract balance ({remainingAmount}).");
}