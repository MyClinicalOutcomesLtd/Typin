namespace Typin
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.Schemas;
    using Typin.Utilities;

    /// <summary>
    /// Command line application facade.
    /// </summary>
    public sealed class CliApplication
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ApplicationMetadata _metadata;
        private readonly IConsole _console;
        private readonly ICliCommandExecutor _cliCommandExecutor;
        private readonly IRootSchemaAccessor _rootSchemaAccessor;
        private readonly CliApplicationLifetime _applicationLifetime;
        private readonly ILogger _logger;
        private readonly Action<ApplicationMetadata, IConsole>? _startupMessage;

        /// <summary>
        /// Initializes an instance of <see cref="CliApplication"/>.
        /// </summary>
        public CliApplication(IServiceProvider serviceProvider,
                              IConsole console,
                              ApplicationMetadata metadata,
                              ILogger<CliApplication> logger,
                              ICliCommandExecutor cliCommandExecutor,
                              IRootSchemaAccessor rootSchemaAccessor,
                              ICliApplicationLifetime cliApplicationLifetime)
        {
            _serviceProvider = serviceProvider;

            _metadata = metadata;
            _console = console;

            _logger = logger;
            _cliCommandExecutor = cliCommandExecutor;
            _rootSchemaAccessor = rootSchemaAccessor;
            _applicationLifetime = (CliApplicationLifetime)cliApplicationLifetime;
        }

        /// <summary>
        /// Runs the application with specified command line arguments and environment variables, and returns the exit code.
        /// </summary>
        /// <remarks>
        /// If a <see cref="CommandException"/>, <see cref="DirectiveException"/>, or <see cref="TypinException"/> is thrown during command execution,
        /// it will be handled and routed to the console. Additionally, if the debugger is not attached (i.e. the app is running in production),
        /// all other exceptions thrown within this method will be handled and routed to the console as well.
        /// </remarks>
        public async ValueTask<int> RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting CLI application...");
                _console.ResetColor();

                RootSchema rootSchema = _rootSchemaAccessor.RootSchema; //Force root schema to resolve. TODO: find a solution to enable lazy root schema resolving.

                //TODO: OnStart()

                _startupMessage?.Invoke(_metadata, _console);

                int exitCode = await StartAppAsync(cancellationToken);

                //TODO: OnStop()
                _logger.LogInformation("CLI application stopped.");

                return exitCode;
            }
            // This may throw pre-execution resolving exceptions which are useful only to the end-user
            catch (TypinException ex)
            {
                _logger.LogDebug(ex, $"{nameof(TypinException)} occured. Trying to find exception handler.");

                IEnumerable<ICliExceptionHandler> exceptionHandlers = _serviceProvider.GetServices<ICliExceptionHandler>();
                foreach (ICliExceptionHandler handler in exceptionHandlers)
                {
                    if (handler.HandleException(ex))
                    {
                        _logger.LogDebug(ex, "Exception handled by {ExceptionHandlerType}.", handler.GetType().FullName);

                        break;
                    }
                }

                _logger.LogCritical(ex, "Unhandled Typin exception caused app to terminate.");

                _console.Error.WithForegroundColor(ConsoleColor.DarkRed, (error) => error.WriteLine($"Unhandled Typin exception caused app to terminate."));
                _console.Error.WriteLine();
                _console.Error.WriteException(ex);

                return ExitCodes.FromException(ex);
            }
            // To prevent the app from showing the annoying Windows troubleshooting dialog,
            // we handle all exceptions and route them to the console nicely.
            // However, we don't want to swallow unhandled exceptions when the debugger is attached,
            // because we still want the IDE to show them to the developer.
            catch (Exception ex) //when (!Debugger.IsAttached)
            {
                _logger.LogCritical(ex, "Unhandled exception caused app to terminate.");

                _console.Error.WithForegroundColor(ConsoleColor.DarkRed, (error) => error.WriteLine($"Fatal error occured in {_metadata.ExecutableName}."));
                _console.Error.WriteLine();
                _console.Error.WriteException(ex);

                return ExitCodes.FromException(ex);
            }
            finally
            {
                if (_console is IDisposable dc)
                {
                    dc.Dispose();
                }
            }
        }

        private async Task<int> StartAppAsync(CancellationToken cancellationToken)
        {
            _applicationLifetime.Initialize();

            int exitCode = ExitCodes.Error;
            //CancellationToken cancellationToken = _console.GetCancellationToken(); //TODO: remove

            while (_applicationLifetime.State == CliLifetimes.Running && !cancellationToken.IsCancellationRequested)
            {
                ICliMode? currentMode = _applicationLifetime.CurrentMode;

                //TODO: remove nulability from CurrentMode
                if (currentMode is not null)
                {
                    exitCode = await currentMode.ExecuteAsync(_cliCommandExecutor, cancellationToken);
                }

                _applicationLifetime.TrySwitchModes();
                _applicationLifetime.TryStop();
            }

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Cancellation requested. Stopping CLI application...");
            }

            _logger.LogInformation("CLI application will stop with '{ExitCode}'.", exitCode);

            _applicationLifetime.State = CliLifetimes.Stopped;

            return exitCode;
        }
    }
}
