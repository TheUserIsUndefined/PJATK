using System;

namespace LegacyApp.Interfaces;

public interface IDateTimeProvider
{
    public DateTime now();
}