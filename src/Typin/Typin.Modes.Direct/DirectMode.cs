namespace Typin.Modes.Direct
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Input;

    /// <summary>
    /// Direct CLI mode. If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
    /// </summary>
    public class DirectMode : ICliMode
    {
        private readonly ICliApplicationLifetime _applicationLifetime;
        private readonly IStartupInputProvider _startupInputProvider;

        /// <summary>
        /// Initializes an instance of <see cref="DirectMode"/>.
        /// </summary>
        public DirectMode(ICliApplicationLifetime applicationLifetime,
                          IStartupInputProvider startupInputProvider)
        {
            _applicationLifetime = applicationLifetime;
            _startupInputProvider = startupInputProvider;
        }

        /// <inheritdoc/>
        public async ValueTask<int> ExecuteAsync(ICliCommandExecutor executor, CancellationToken cancellationToken)
        {
            IEnumerable<string> commandLineArguments = await _startupInputProvider.GetInputAsync(cancellationToken);

            int exitCode = await executor.ExecuteCommandAsync(commandLineArguments, cancellationToken);
            _applicationLifetime.RequestStop();

            return exitCode;
        }
    }
}
