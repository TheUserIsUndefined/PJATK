using System;

namespace LegacyApp.Interfaces;

public interface IUserCreditService : IDisposable
{
    internal int GetCreditLimit(string lastName, DateTime dateOfBirth);
}