namespace Tutorial10_EFCore_CodeFirst.Application.Exceptions;

public static class PatientExceptions
{
    public class PatientNotFoundException(int patientId) :
        BaseExceptions.NotFoundException($"Patient {patientId} not found.");
}