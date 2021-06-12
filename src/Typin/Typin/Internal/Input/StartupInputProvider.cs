namespace Typin.Internal.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Input;
    using Typin.Utilities;

    /// <summary>
    /// Default input provider.
    /// </summary>
    internal class StartupInputProvider : IStartupInputProvider
    {
        private readonly IEnumerable<string> _args;

        /// <summary>
        /// Initializes a new instace of <see cref="StartupInputProvider"/>.
        /// </summary>
        /// <param name="commandLineOverride"></param>
        /// <param name="startsWithExecutableName"></param>
        public StartupInputProvider(string? commandLineOverride, bool startsWithExecutableName)
        {
            string commandLine = commandLineOverride ?? Environment.CommandLine;

            _args = CommandLineSplitter.Split(commandLine)
                                       .Skip(commandLineOverride is null || startsWithExecutableName ? 1 : 0);
        }

        /// <inheritdoc/>
        public ValueTask<IEnumerable<string>> GetInputAsync(CancellationToken cancellationToken)
        {
#if NETSTANDARD2_0 || NETSTANDARD2_1
            return new ValueTask<IEnumerable<string>>(_args);
#else
            return ValueTask.FromResult(_args);
#endif
        }
    }
}
