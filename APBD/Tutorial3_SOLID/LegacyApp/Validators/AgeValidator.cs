using LegacyApp.Implementations;
using LegacyApp.Interfaces;

namespace LegacyApp.Validators;

public class AgeValidator : IValidator
{
    private readonly int _minAge;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AgeValidator() : this(21, new DateTimeProvider()) { }

    public AgeValidator(int minAge, IDateTimeProvider dateTimeProvider)
    {
        _minAge = minAge;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public bool Validate(User user)
    {
        var now = _dateTimeProvider.now();
        var dateOfBirth = user.DateOfBirth;
        
        int age = now.Year - dateOfBirth.Year;
        if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

        return age >= _minAge;
    }
}