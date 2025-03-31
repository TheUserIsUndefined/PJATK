using LegacyApp.Interfaces;

namespace LegacyApp.Validators;

public class UserCreditLimit : IUserCreditLimit 
{
    private readonly IUserCreditService _userCreditService;
    
    public UserCreditLimit() : this(new UserCreditService()) { }

    public UserCreditLimit(IUserCreditService userCreditService)
    {
        _userCreditService = userCreditService;
    }
    
    public void applyLimit(User user)
    {
        var client = (Client) user.Client;
        
        if (client.Type == "VeryImportantClient")
        {
            user.HasCreditLimit = false;
        }
        else if (client.Type == "ImportantClient")
        {
            using (_userCreditService)
            {
                int creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit *= 2;
                user.CreditLimit = creditLimit;
            }
        }
        else
        {
            user.HasCreditLimit = true;
            using (_userCreditService)
            {
                int creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.CreditLimit = creditLimit;
            }
        }
    }
}