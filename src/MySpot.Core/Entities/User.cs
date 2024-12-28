using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class User(UserId id, Email email, Username username, Password password, FullName fullName, Role role, DateTime createdAt)
{
    public UserId Id { get; private set; } = id;
    public Email Email { get; private set; } = email;
    public Username Username { get; private set; } = username;
    public Password Password { get; private set; } = password;
    public FullName FullName { get; private set; } = fullName;
    public Role Role { get; private set; } = role;
    public DateTime CreatedAt { get; private set; } = createdAt;
}