namespace Tutorial10_EFCore_CodeFirst.Application.Exceptions;

public static class DoctorExceptions
{
    public class DoctorNotFoundException(int doctorId) : 
        BaseExceptions.NotFoundException($"Doctor {doctorId} not found.");
}