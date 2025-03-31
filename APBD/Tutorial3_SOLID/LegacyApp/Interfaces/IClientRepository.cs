namespace LegacyApp.Interfaces;

public interface IClientRepository
{
    internal Client GetById(int clientId);
}