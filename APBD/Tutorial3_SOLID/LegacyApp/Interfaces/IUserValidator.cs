namespace LegacyApp.Interfaces;

public interface IUserValidator
{
    public bool ValidatePersonalData(User user);
    public bool ValidateCreditLimit(User user);
}