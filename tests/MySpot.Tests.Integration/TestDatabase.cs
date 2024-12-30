using Microsoft.EntityFrameworkCore;
using MySpot.Infrastructure;
using MySpot.Infrastructure.DAL;

namespace MySpot.Tests.Integration;

public class TestDatabase: IDisposable
{
    public MySpotDbContext MySpotDbContext { get; }

    public TestDatabase()
    {
        var options = new OptionsProvider().Get<PostgresOptions>("postgres");
        MySpotDbContext = new MySpotDbContext(new DbContextOptionsBuilder<MySpotDbContext>()
            .UseNpgsql(options.ConnectionString)
            .Options);
    }

    public void Dispose()
    {
        MySpotDbContext.Database.EnsureDeleted();
        MySpotDbContext.Dispose();
    }
    
}