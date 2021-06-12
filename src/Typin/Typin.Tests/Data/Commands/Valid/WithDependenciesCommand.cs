namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Services;

    [Command("cmd")]
    public class WithDependenciesCommand : ICommand
    {
        private readonly IConsole _console;
        private readonly DependencyA _dependencyA;
        private readonly DependencyB _dependencyB;
        private readonly DependencyC _dependencyC;

        public WithDependenciesCommand(IConsole console, DependencyA dependencyA, DependencyB dependencyB, DependencyC dependencyC)
        {
            _console = console;
            _dependencyA = dependencyA;
            _dependencyB = dependencyB;
            _dependencyC = dependencyC;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine($"{_dependencyA.Value}|{_dependencyB.Value}|{_dependencyC.Value}");
            _console.Output.WriteLine($"{_dependencyA.Id}|{_dependencyB.Id}|{_dependencyC.Id}");
            _console.Output.WriteLine(_dependencyC.DependencyBId);

            return default;
        }
    }
}