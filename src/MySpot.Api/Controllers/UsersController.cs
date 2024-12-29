using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Application.Security;

namespace MySpot.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class UsersController(
    ICommandHandler<SignUpCommand> signUpCommandHandler,
    ICommandHandler<SignInCommand> signInCommandHandler,
    IQueryHandler<GetUsersQuery, IEnumerable<UserDto>> getUsersQueryHandler,
    IQueryHandler<GetUserQuery, UserDto> getUserQueryHandler,
    ITokenStorage tokenStorage): ControllerBase
{
    private readonly IQueryHandler<GetUsersQuery, IEnumerable<UserDto>> _getUsersQueryHandler = getUsersQueryHandler;
    private readonly IQueryHandler<GetUserQuery, UserDto> _getUserQueryHandler = getUserQueryHandler;
    private readonly ICommandHandler<SignUpCommand> _signUpCommandHandler = signUpCommandHandler;
    private readonly ICommandHandler<SignInCommand> _signInCommandHandler = signInCommandHandler;
    private readonly ITokenStorage _tokenStorage = tokenStorage;
    
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserDto>> Get(Guid userId)
    {
        var user = await _getUserQueryHandler.HandleAsync(new GetUserQuery {UserId = userId});
        if (user is null)
        {
            return NotFound();
        }

        return user;
    }
    
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Get()
    {
        if (string.IsNullOrWhiteSpace(User.Identity?.Name))
        {
            return NotFound();
        }

        var userId = Guid.Parse(User.Identity?.Name);
        var user = await _getUserQueryHandler.HandleAsync(new GetUserQuery {UserId = userId});

        return user;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] GetUsersQuery query)
        => Ok(await _getUsersQueryHandler.HandleAsync(query));

    [HttpPost]
    public async Task<ActionResult> Post(SignUpCommand command)
    {
        command = command with {UserId = Guid.NewGuid()};
        await _signUpCommandHandler.HandleAsync(command);
        return CreatedAtAction(nameof(Get), new {command.UserId}, null);
    }
    
    [HttpPost("sign-in")]
    public async Task<ActionResult<JwtDto>> Post(SignInCommand command)
    {
        await _signInCommandHandler.HandleAsync(command);
        return Ok(_tokenStorage.Get());
    }
}