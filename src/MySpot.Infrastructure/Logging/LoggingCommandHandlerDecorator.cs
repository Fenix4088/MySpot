using System.Diagnostics;
using Humanizer;
using Microsoft.Extensions.Logging;
using MySpot.Application.Abstractions;

namespace MySpot.Infrastructure.Logging;

internal sealed class LoggingCommandHandlerDecorator<TCommand>(ICommandHandler<TCommand> commandHandler, ILogger<ICommandHandler<ICommand>> logger): ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler = commandHandler;
    private readonly ILogger<ICommandHandler<ICommand>> _logger = logger;
    public async Task HandleAsync(TCommand command)
    {
        var commandName = typeof(TCommand).Name.Underscore();
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        _logger.LogInformation("Starting handling a command {CommandName}...", commandName);
        await commandHandler.HandleAsync(command);
        stopWatch.Stop();
        _logger.LogInformation("Completed handling a command {CommandName} in {Elapsed}", commandName, stopWatch.Elapsed);

    }
}