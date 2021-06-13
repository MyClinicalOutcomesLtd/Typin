namespace SimpleAppExample.Commands
{
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Consoles;

    [Command]
    public class SampleCommand : ICommand
    {
        [CommandParameter(0)]
        public int? ParamB { get; init; }

        [CommandOption("str", 's')]
        public string? StrOption { get; init; }

        [CommandOption("int", 'i')]
        public int IntOption { get; init; }

        [CommandOption("bool", 'b')]
        public bool BoolOption { get; init; }

        [CommandOption('v')]
        public bool VOption { get; init; }

        [CommandOption('x')]
        public bool XOption { get; init; }

        private readonly IConsole _console;

        public SampleCommand(IConsole console)
        {
            _console = console;
        }

        public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            await _console.Output.WriteLineAsync(JsonSerializer.Serialize(this));
        }
    }
}