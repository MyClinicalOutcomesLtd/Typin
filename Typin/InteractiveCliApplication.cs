﻿namespace Typin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Typin.Input;
    using Typin.Internal;
    using Typin.Internal.AutoComplete;
    using Typin.Schemas;

    /// <summary>
    /// Command line application facade.
    /// </summary>
    public partial class InteractiveCliApplication : CliApplication
    {
        private readonly ConsoleColor _promptForeground;
        private readonly ConsoleColor _commandForeground;
        private readonly AutoCompleteInput? _autoCompleteInput;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveCliApplication"/>.
        /// </summary>
        public InteractiveCliApplication(LinkedList<Type> middlewareTypes,
                                         IServiceProvider serviceProvider,
                                         CliContext cliContext,
                                         ConsoleColor promptForeground,
                                         ConsoleColor commandForeground) :
            base(middlewareTypes, serviceProvider, cliContext)
        {
            _promptForeground = promptForeground;
            _commandForeground = commandForeground;

            if (cliContext.Console is SystemConsole s)
            {
                _autoCompleteInput = new AutoCompleteInput(s)
                {
                    AutoCompletionHandler = new AutoCompletionHandler(),
                    IsHistoryEnabled = true
                };
            }
        }

        /// <inheritdoc/>
        protected override async Task<int> PreExecuteCommand(IReadOnlyList<string> commandLineArguments,
                                                             IReadOnlyDictionary<string, string> environmentVariables,
                                                             RootSchema root)
        {
            CommandInput input = CommandInput.Parse(commandLineArguments, root.GetCommandNames());
            CliContext.Input = input;

            if (input.IsInteractiveDirectiveSpecified)
            {
                CliContext.IsInteractiveMode = true;

                // we don't want to run default command for e.g. `[interactive]` but we want to run if there is sth else
                if (!input.IsDefaultCommandOrEmpty)
                    await ExecuteCommand(environmentVariables, root, input);

                await RunInteractivelyAsync(environmentVariables, root);
            }

            return await ExecuteCommand(environmentVariables, root, input);
        }

        private async Task RunInteractivelyAsync(IReadOnlyDictionary<string, string> environmentVariables,
                                                 RootSchema root)
        {
            IConsole console = CliContext.Console;
            string executableName = CliContext.Metadata.ExecutableName;

            while (true) //TODO maybe add CliContext.Exit and CliContext.Status
            {
                string[] commandLineArguments = GetInput(console, executableName);

                CommandInput input = CommandInput.Parse(commandLineArguments, root.GetCommandNames());
                CliContext.Input = input; //TODO maybe refactor with some clever IDisposable class

                await ExecuteCommand(environmentVariables, root, input);
                console.ResetColor();
            }
        }

        private string[] GetInput(IConsole console, string executableName)
        {
            string[] arguments;
            string line = string.Empty;
            do
            {
                // Print prompt
                console.WithForegroundColor(_promptForeground, () =>
                {
                    console.Output.Write(executableName);
                });

                if (!string.IsNullOrWhiteSpace(CliContext.Scope))
                {
                    console.WithForegroundColor(ConsoleColor.Cyan, () =>
                    {
                        console.Output.Write(' ');
                        console.Output.Write(CliContext.Scope);
                    });
                }

                console.WithForegroundColor(_promptForeground, () =>
                {
                    console.Output.Write("> ");
                });

                // Read user input
                console.WithForegroundColor(_commandForeground, () =>
                {
                    if (_autoCompleteInput is null)
                        line = console.Input.ReadLine();
                    else
                        line = _autoCompleteInput.Read();
                });

                if (string.IsNullOrWhiteSpace(CliContext.Scope)) // handle unscoped command input
                {
                    arguments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                    .ToArray();
                }
                else // handle scoped command input
                {
                    List<string> tmp = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                           .ToList();

                    int lastDirective = tmp.FindLastIndex(x => x.StartsWith('[') && x.EndsWith(']'));
                    tmp.Insert(lastDirective + 1, CliContext.Scope);

                    arguments = tmp.ToArray();
                }

            } while (string.IsNullOrWhiteSpace(line)); // retry on empty line

            console.ForegroundColor = ConsoleColor.Gray;

            return arguments;
        }
    }
}
