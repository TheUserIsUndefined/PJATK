namespace Tutorial10_EFCore_CodeFirst.Application.Exceptions;

public class InvalidPrescriptionDateRangeException(DateOnly date, DateOnly dueDate) :
    Exception($"Prescription due date ({dueDate}) must be greater than or equal to the issue date ({date}).");