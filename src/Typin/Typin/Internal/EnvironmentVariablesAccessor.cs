namespace Typin.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <inheritdoc/>
    internal class EnvironmentVariablesAccessor : IEnvironmentVariablesAccessor
    {
        /// <inheritdoc/>
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }

        public EnvironmentVariablesAccessor(IReadOnlyDictionary<string, string>? environmentVariablesOverride)
        {
            EnvironmentVariables = environmentVariablesOverride ??
                Environment.GetEnvironmentVariables()
                           .Cast<DictionaryEntry>()
                           .ToDictionary(x => (string)x.Key,
                                         x => (x.Value as string) ?? string.Empty,
                                         StringComparer.Ordinal);
        }
    }
}