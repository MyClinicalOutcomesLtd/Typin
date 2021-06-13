namespace Typin
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Configuration of the application.
    /// </summary>
    public class ApplicationConfiguration
    {
        /// <summary>
        /// Mode types defined in this application.
        /// </summary>
        public IReadOnlyList<Type> ModeTypes { get; }

        /// <summary>
        /// Command types defined in this application.
        /// </summary>
        public IReadOnlyList<Type> CommandTypes { get; }

        /// <summary>
        /// Custom directives defined in this application.
        /// </summary>
        public IReadOnlyList<Type> DirectiveTypes { get; }

        ///// <summary>
        ///// Collection of middlewares in application.
        ///// </summary>
        //public IReadOnlyCollection<Type> MiddlewareTypes { get; }

        /// <summary>
        /// Startup mode type.
        /// </summary>
        public Type StartupMode { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ApplicationConfiguration"/>.
        /// </summary>
        public ApplicationConfiguration(IReadOnlyList<Type> modeTypes,
                                        IReadOnlyList<Type> commandTypes,
                                        IReadOnlyList<Type> customDirectives,
                                        Type startupMode)
        {
            ModeTypes = modeTypes;
            CommandTypes = commandTypes;
            DirectiveTypes = customDirectives;
            StartupMode = startupMode;
        }
    }
}