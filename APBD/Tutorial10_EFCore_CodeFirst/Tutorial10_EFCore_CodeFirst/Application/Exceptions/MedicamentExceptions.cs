namespace Tutorial10_EFCore_CodeFirst.Application.Exceptions;

public static class MedicamentExceptions
{
    public class MedicamentNotFoundException(int medId) : 
        BaseExceptions.NotFoundException($"Medicament {medId} not found.");
    
    public class MedicamentsAmountExceededException(int maxAmount) : 
        ArgumentException($"Medicaments amount exceeds maximum amount of {maxAmount}.");
}