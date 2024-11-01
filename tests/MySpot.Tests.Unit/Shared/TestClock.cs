using MySpot.Application.Services;

namespace MySpot.Tests.Unit.Shared;

public class TestClock: IClock
{
    public DateTime Current() => new DateTime(2024, 10, 31);
}