using MySpot.Application.Abstractions;
using MySpot.Application.DTO;

namespace MySpot.Application.Queries;

public record GetUsersQuery(): IQuery<IEnumerable<UserDto >>;