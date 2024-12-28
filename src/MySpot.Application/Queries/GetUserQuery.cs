using MySpot.Application.Abstractions;
using MySpot.Application.DTO;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Queries;

public class GetUserQuery : IQuery<UserDto>
{
    public Guid UserId { get; set; }
}