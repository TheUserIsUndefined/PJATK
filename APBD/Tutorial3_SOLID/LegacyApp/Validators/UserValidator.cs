using System.Collections.Generic;
using LegacyApp.Interfaces;

namespace LegacyApp.Validators;

public class UserValidator : IUserValidator
{
    private readonly IEnumerable<IValidator> _validators;
    private readonly int _minCreditLimit;

    public UserValidator() : this(new List<IValidator>
    {
        new NameValidator(), 
        new EmailValidator(), 
        new AgeValidator()
    }, 500) { }

    public UserValidator(IEnumerable<IValidator> validators, int minCreditLimit)
    {
        _validators = validators;
        _minCreditLimit = minCreditLimit;
    }
    
    public bool ValidatePersonalData(User user)
    {
        foreach (var validator in _validators)
            if (!validator.Validate(user))
                return false;
        
        return true;
    }

    public bool ValidateCreditLimit(User user)
    {
        return !(user.HasCreditLimit && user.CreditLimit < _minCreditLimit);
    }
}