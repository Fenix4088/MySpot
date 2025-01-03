using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Application.Security;
using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public class SignUpCommandHandler(IUserRepository userRepository, IPasswordManager passwordManager, IClock clock): ICommandHandler<SignUpCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IClock _clock = clock;
    public async Task HandleAsync(SignUpCommand command)
    {
        var userId = new UserId(command.UserId);
        var email = new Email(command.Email);
        var username = new Username(command.Username);
        var password = new Password(command.Password);
        var fullName = new FullName(command.FullName);
        var role = string.IsNullOrWhiteSpace(command.Role) ? Role.User() : new Role(command.Role);
        
        if (await _userRepository.GetByEmailAsync(email) is not null)
        {
            throw new EmailAlreadyInUseException(email);
        }

        if (await _userRepository.GetByUsernameAsync(username) is not null)
        {
            throw new UsernameAlreadyInUseException(username);
        }

        var securedPassword = _passwordManager.Secure(password);
        var user = new User(userId, email, username, securedPassword, fullName, role, _clock.Current());
        await _userRepository.AddAsync(user);
    }
}