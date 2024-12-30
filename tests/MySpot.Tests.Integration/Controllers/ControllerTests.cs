using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MySpot.Application.DTO;
using MySpot.Application.Security;
using MySpot.Infrastructure;
using MySpot.Infrastructure.Auth;
using MySpot.Infrastructure.Security;
using MySpot.Infrastructure.Time;
using Xunit;

namespace MySpot.Tests.Integration.Controllers;

//this attribute make tests run one by one but not in parallel(default)
[Collection("api")]
public abstract class ControllerTests: IClassFixture<OptionsProvider>
{
    private readonly IAuthenticator _authenticator;
    protected HttpClient Client { get;}
    
    public  ControllerTests(OptionsProvider optionsProvider)
    {
        // var postgresOptions = optionsProvider.Get<PostgresOptions>("postgres");
        var authOptions = optionsProvider.Get<AuthOptions>("auth");
        _authenticator = new Authenticator(new OptionsWrapper<AuthOptions>(authOptions), new Clock());
        
        var app = new MySpotTestApp(ConfigureServices);
        Client = app.Client;

    }
    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }
    protected JwtDto Authorize(Guid userId, string role)
    {
        var jwt = _authenticator.CreateToken(userId, role);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.AccessToken);
        return jwt;
    }

}