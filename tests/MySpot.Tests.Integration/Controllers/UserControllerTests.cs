using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Security;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Integration.Controllers;

public class UserControllerTests : ControllerTests, IDisposable
{
    private readonly TestDatabase _testDatabase;
    private readonly Password _password = "secure";
    private IUserRepository _userRepository;

    public UserControllerTests(OptionsProvider optionsProvider) : base(optionsProvider)
    {
        _testDatabase = new TestDatabase();
    }

    [Fact]
    public async Task get_users_me_should_return_ok_200_status_code_and_user()
    {
        var user = await CreateUserAsync();
        
        Authorize(user.Id, user.Role);
        var userDto = await Client.GetFromJsonAsync<UserDto>("users/me");
        userDto.ShouldNotBeNull();
        userDto.Id.ShouldBe(user.Id);
    }

    [Fact]
    public async Task post_sign_in_should_return_ok_200_status_and_jwt()
    {
        var user = await CreateUserAsync();

        var command = new SignInCommand(
            user.Email,
            _password
        );

        var resp = await Client.PostAsJsonAsync("users/sign-in", command);
        
        resp.StatusCode.ShouldBe(HttpStatusCode.OK);
        var jwt = await resp.Content.ReadFromJsonAsync<JwtDto>();
        jwt.ShouldNotBeNull();
        jwt.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task post_users_should_return_created_201_status_code()
    {
        var command = new SignUpCommand(
            Guid.Empty,
            "test@gmail.com",
            "user",
            _password,
            "user user",
            Role.User()
            );

        var resp = await Client.PostAsJsonAsync("users", command);
        
        resp.StatusCode.ShouldBe(HttpStatusCode.Created);
        resp.Headers.Location.ShouldNotBe(null);
        
    }

    public void Dispose()
    {
        _testDatabase.Dispose();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        _userRepository = new TestUserRepository();
        services.AddSingleton<IUserRepository>(_userRepository);
    }
    
    private async Task<User> CreateUserAsync()
    {
        var passwordManager = new PasswordManager(new PasswordHasher<User>());

        const string email = "some@email.com";
        var user = new User(
            Guid.NewGuid(),
            email,
            "username",
            passwordManager.Secure(_password),
            "user user",
            Role.User(),
            DateTime.Now
        );

        await _testDatabase.MySpotDbContext.Users.AddAsync(user);
        await _testDatabase.MySpotDbContext.SaveChangesAsync();

        return user;
    }
}