using Exception = System.Exception;

namespace MySpot.Infrastructure.DAL;

internal interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
}

