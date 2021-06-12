namespace Typin.Hosting.Builder
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// CLI components collection.
    /// </summary>
    public interface ICliComponentsCollection
    {
        /// <summary>
        /// Registered commands.
        /// </summary>
        IReadOnlyList<Type> Commands { get; }

        /// <summary>
        /// Registered dynamic commands.
        /// </summary>
        IReadOnlyList<Type> DynamicCommands { get; }

        /// <summary>
        /// Registered directives.
        /// </summary>
        IReadOnlyList<Type> Directives { get; }

        /// <summary>
        /// Registered modes.
        /// </summary>
        IReadOnlyList<Type> Modes { get; }

        /// <summary>
        /// Startup mode type.
        /// </summary>
        Type? StartupMode { get; }

        /// <summary>
        /// Services collection.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Add a custom directive to the application.
        /// </summary>
        /// <exception cref="ArgumentException">Throws when directive type is invalid.</exception>
        ICliComponentsCollection AddDirective(Type directiveType);

        /// <summary>
        /// Adds a command of specified type to the application.
        /// </summary>
        /// <exception cref="ArgumentException">Throws when command type is invalid.</exception>
        ICliComponentsCollection AddCommand(Type commandType);

        /// <summary>
        /// Adds a dunamic command of specified type to the application.
        /// </summary>
        /// <exception cref="ArgumentException">Throws when dynamic command type is invalid.</exception>
        ICliComponentsCollection AddDynamicCommand(Type dynamicCommandType);

        /// <summary>
        /// Registers a CLI mode. Only one mode can be registered as startup mode.
        /// If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
        ///
        /// Do not call RegisterMode directly from builder, instead call UseXMode method, e.g. UseDirectMode().
        /// </summary>
        /// <exception cref="ArgumentException">Throws when mode type is invalid.</exception>
        ICliComponentsCollection RegisterMode(Type modeType, bool asStartup = false);
    }
}
