using LegacyApp.Interfaces;

namespace LegacyApp.Implementations;

public class UserDataAccessProvider : IUserDataAccessProvider
{
    public void AddUser(User user) => UserDataAccess.AddUser(user);
}