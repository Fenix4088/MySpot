using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace MySpot.Tests.Integration;

internal class MySpotTestApp: WebApplicationFactory<Program>
{
    public HttpClient Client { get; }
    
    public MySpotTestApp(Action<IServiceCollection> services)
    {
        Client = WithWebHostBuilder(builder =>
        {
            if (services is not null)
            {
                builder.ConfigureServices(services);
            }

            builder.UseEnvironment("test");
        }).CreateClient();
    }

}