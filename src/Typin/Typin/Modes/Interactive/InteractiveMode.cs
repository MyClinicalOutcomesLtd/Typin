namespace Typin.Modes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Typin;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Input;

    /// <summary>
    /// Interactive CLI mode.
    /// </summary>
    public class InteractiveMode : ICliMode
    {
        private readonly bool firstEnter = true;

        private readonly InteractiveModeOptions _options;
        private readonly IConsole _console;
        private readonly ApplicationConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IStartupInputProvider _startupInputProvider;
        private readonly IInteractiveInputProvider _interactiveInputProvider;
        private readonly IServiceProvider _serviceProvider;

        private readonly AutoCompleteInput? _autoCompleteInput;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveMode"/>.
        /// </summary>
        public InteractiveMode(IOptions<InteractiveModeOptions> options,
                               IConsole console,
                               ILogger<InteractiveMode> logger,
                               IRootSchemaAccessor rootSchemaAccessor,
                               ApplicationConfiguration configuration,
                               IStartupInputProvider startupInputProvider,
                               IInteractiveInputProvider interactiveInputProvider,
                               IServiceProvider serviceProvider)
        {
            _options = options.Value;

            _console = console;
            _logger = logger;
            _configuration = configuration;
            _startupInputProvider = startupInputProvider;
            _interactiveInputProvider = interactiveInputProvider;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async ValueTask<int> ExecuteAsync(ICliCommandExecutor executor, CancellationToken cancellationToken)
        {
            if (firstEnter && _configuration.StartupMode == typeof(InteractiveMode))
            {
                IEnumerable<string> commandLineArguments = await _startupInputProvider.GetInputAsync(cancellationToken);

                if (commandLineArguments.Any())
                {
                    await executor.ExecuteCommandAsync(commandLineArguments, cancellationToken);
                }
            }

            IEnumerable<string> interactiveArguments;
            try
            {
                interactiveArguments = await _interactiveInputProvider.GetInputAsync(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Interactive mode input cancelled.");
                return ExitCodes.Error;
            }

            _console.ResetColor();

            if (interactiveArguments.Any())
            {
                await executor.ExecuteCommandAsync(interactiveArguments, cancellationToken);
                _console.ResetColor();
            }

            return ExitCodes.Success;
        }
    }
}
