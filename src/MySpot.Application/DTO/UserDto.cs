using MySpot.Core.ValueObjects;

namespace MySpot.Application.DTO;

public class UserDto()
{
    public UserId Id { get; set; }
    public Username Username { get; set; }
    public FullName FullName { get; set; }
}