namespace MySpot.Infrastructure.DAL;

internal sealed class PostgresUnitOfWork(MySpotDbContext mySpotDbContext) : IUnitOfWork
{
    private readonly MySpotDbContext _mySpotDbContext = mySpotDbContext;
    
    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await _mySpotDbContext.Database.BeginTransactionAsync();

        try
        {
            await action();
            await mySpotDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
        
    }
}