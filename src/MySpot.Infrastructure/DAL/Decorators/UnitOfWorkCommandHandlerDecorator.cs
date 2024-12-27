using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.Commands.Handlers;

namespace MySpot.Infrastructure.DAL.Decorators;

internal sealed class UnitOfWorkCommandHandlerDecorator<TCommand>(ICommandHandler<TCommand> commandHandler, IUnitOfWork unitOfWork): ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler = commandHandler;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task HandleAsync(TCommand command)
    {
        await _unitOfWork.ExecuteAsync(() => commandHandler.HandleAsync(command));
    }
}