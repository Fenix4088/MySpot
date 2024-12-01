namespace MySpot.Core.ValueObjects;

public record JobTitle(string Value)
{
    public const string Employee = nameof(Employee);
    public const string Manager = nameof(Manager);
    public const string Boss = nameof(Boss);

    public static implicit operator string(JobTitle jobTitle) => jobTitle.Value;
    public static implicit operator JobTitle(string jobTitle) => new (jobTitle);

};