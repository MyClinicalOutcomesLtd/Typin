namespace Typin.Middlewares
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;

    /// <summary>
    /// Executes a command.
    /// </summary>
    public sealed class ExecuteCommand : IMiddleware
    {
        /// <inheritdoc/>
        public async Task HandleAsync(ICliContext context, CommandPipelineHandlerDelegate next, CancellationToken cancellationToken)
        {
            // Get command instance from context
            ICommand instance = context.Command;

            // Execute command
            await instance.ExecuteAsync(context.Console.GetCancellationToken());

            context.ExitCode ??= ExitCodes.Success;

            await next();
        }
    }
}
