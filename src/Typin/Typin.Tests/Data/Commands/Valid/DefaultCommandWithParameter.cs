﻿namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command(Description = "Default command with parameter description")]
    public class DefaultCommandWithParameter : ICommand
    {
        public const string ExpectedOutputText = nameof(DefaultCommandWithParameter);

        [CommandParameter(0)]
        public string? ParamA { get; init; }

        private readonly IConsole _console;

        public DefaultCommandWithParameter(IConsole console)
        {
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine(ExpectedOutputText);
            _console.Output.WriteLine(ParamA);

            return default;
        }
    }
}