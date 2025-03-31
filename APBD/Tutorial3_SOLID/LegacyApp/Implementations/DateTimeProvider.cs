using System;
using LegacyApp.Interfaces;

namespace LegacyApp.Implementations;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime now() => DateTime.Now;
}