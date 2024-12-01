using MySpot.Application.Services;
using MySpot.Core.Abstractions;

namespace MySpot.Tests.Unit.Shared;

public class TestClock: IClock
{
    public DateTime Current() => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
}