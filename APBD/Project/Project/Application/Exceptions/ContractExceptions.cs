namespace Project.Application.Exceptions;

public static class ContractExceptions
{
    public class ContractValidationException(string message) : BaseExceptions.ValidationException(message);

    public class ContractNotFoundByIdException(int contractId) :
        BaseExceptions.NotFoundException($"Contract with ID {contractId} not found.");

    public class ContractPaymentExpiredException(int contractId, DateOnly deadline, int newContractId) :
        BaseExceptions.ValidationException(
            $"Payment deadline for contract {contractId} has expired on {deadline:yyyy-MM-dd}." +
            $" All payments have been returned and new contract with ID {newContractId} has been created."
            );
    
    public class ContractAlreadyPaidException(int contractId) :
        BaseExceptions.ValidationException($"Contract with ID {contractId} already paid.");
}