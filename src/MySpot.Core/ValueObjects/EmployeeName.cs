using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public sealed record EmployeeName(string value)
{
    public string Value { get; } = value ?? throw new InvalidEmployeeNameException();

    public static implicit operator string(EmployeeName employeeName) => employeeName.Value;
    public static implicit operator EmployeeName(string employeeName) => new(employeeName);
};