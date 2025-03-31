using LegacyApp.Interfaces;

namespace LegacyApp.Validators;

public class EmailValidator : IValidator
{
    public bool Validate(User user) => user.EmailAddress.Contains("@") && user.EmailAddress.Contains(".");
}