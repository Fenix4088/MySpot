using System.Security.Authentication;
using MySpot.Application.Abstractions;
using MySpot.Application.Security;
using MySpot.Core.Repositories;

namespace MySpot.Application.Commands.Handlers;

public sealed class SignInCommandHandler(
    IUserRepository userRepository, 
    IAuthenticator authenticator, 
    IPasswordManager passwordManager,
    ITokenStorage tokenStorage): ICommandHandler<SignInCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthenticator _authenticator = authenticator;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly ITokenStorage _tokenStorage = tokenStorage;
    public async Task HandleAsync(SignInCommand command)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);

        if (user is null)
        {
            throw new InvalidCredentialException();
        }

        if (!_passwordManager.Validate(command.Password, user.Password))
        {
            throw new InvalidCredentialException();
        }

        var jwt = _authenticator.CreateToken(user.Id, user.Role);
        //we use token storage to set and get token from it
        //made that because we dont want to broke one of CQRS rules
        //Commands should NOT return anything
        //but in UserController SighnIn endpoint we want to return a toke, so we use this TokenStorage to handle it
        _tokenStorage.Set(jwt);
    }
}