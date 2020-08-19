﻿namespace Typin.Directives
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;

    /// <summary>
    /// If application rans in interactive mode, this [..] directive can be used to reset current scope to default (global scope).
    /// <example>
    ///             > [>] cmd1 sub
    ///     cmd1 sub> list
    ///     cmd1 sub> [..]
    ///             >
    /// </example>
    /// </summary>
    [Directive("..", Description = "Resets the scope to default value.", InteractiveModeOnly = true)]
    public sealed class ScopeResetDirective : IDirective
    {
        private readonly CliContext _cliContext;

        /// <inheritdoc/>
        public bool ContinueExecution => false;

        /// <summary>
        /// Initializes an instance of <see cref="ScopeResetDirective"/>.
        /// </summary>
        public ScopeResetDirective(ICliContext cliContext)
        {
            _cliContext = (CliContext)cliContext;
        }

        /// <inheritdoc/>
        public ValueTask HandleAsync(IConsole console)
        {
            _cliContext.Scope = string.Empty;

            return default;
        }
    }
}
