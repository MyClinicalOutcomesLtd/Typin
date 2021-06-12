namespace Typin.Modes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Typin.Utilities;

    /// <summary>
    /// Interactive mode input provider.
    /// </summary>
    public class InteractiveInputProvider : IInteractiveInputProvider
    {
        private readonly InteractiveModeOptions _options;

        private readonly IConsole _console;
        private readonly ApplicationMetadata _metadata;
        private readonly IServiceProvider _serviceProvider;

        private readonly AutoCompleteInput? _autoCompleteInput;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveInputProvider"/>.
        /// </summary>
        public InteractiveInputProvider(IOptions<InteractiveModeOptions> options,
                                        IConsole console,
                                        ApplicationMetadata metadata,
                                        IRootSchemaAccessor rootSchemaAccessor,
                                        IServiceProvider serviceProvider)
        {
            _options = options.Value;

            _console = console;
            _metadata = metadata;
            _serviceProvider = serviceProvider;

            if (_options.IsAdvancedInputAvailable && !_console.Input.IsRedirected)
            {
                _autoCompleteInput = new AutoCompleteInput(_console, _options.UserDefinedShortcuts)
                {
                    AutoCompletionHandler = new AutoCompletionHandler(rootSchemaAccessor),
                };

                _autoCompleteInput.History.IsEnabled = true;
            }
        }

        /// <summary>
        /// Gets user input and returns arguments or null if cancelled.
        /// </summary>
        public async ValueTask<IEnumerable<string>> GetInputAsync(CancellationToken cancellationToken)
        {
            IConsole console = _console;

            string scope = _options.Scope;
            bool hasScope = !string.IsNullOrWhiteSpace(scope);

            // Print prompt
            _options.Prompt(_serviceProvider, _metadata, _console);

            // Read user input
            console.ForegroundColor = _options.CommandForeground;

            string? line = string.Empty; // Can be null when Ctrl+C is pressed to close the app.
            if (_autoCompleteInput is null)
            {
                line = await console.Input.ReadLineAsync();
            }
            else
            {
                line = await _autoCompleteInput.ReadLineAsync(cancellationToken);
            }

            console.ForegroundColor = ConsoleColor.Gray;

            IEnumerable<string> arguments = Enumerable.Empty<string>();

            if (!string.IsNullOrWhiteSpace(line))
            {
                if (hasScope) // handle scoped command input
                {
                    List<string> tmp = CommandLineSplitter.Split(line).ToList();

                    int lastDirective = tmp.FindLastIndex(x => x.StartsWith('[') && x.EndsWith(']'));
                    tmp.Insert(lastDirective + 1, scope);

                    arguments = tmp.ToArray();
                }
                else // handle unscoped command input
                {
                    arguments = CommandLineSplitter.Split(line);
                }
            }

            return arguments;
        }
    }
}
