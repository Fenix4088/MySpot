namespace MySpot.Core.Exceptions;

public sealed class InvalidRoleException(string role) : MySpotException($"Role: '{role}' is invalid.")
{
    public string Role { get; } = role;
}