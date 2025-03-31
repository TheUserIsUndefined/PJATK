using LegacyApp.Interfaces;

namespace LegacyApp.Validators;

public class NameValidator : IValidator
{
    public bool Validate(User user) => !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName);
}