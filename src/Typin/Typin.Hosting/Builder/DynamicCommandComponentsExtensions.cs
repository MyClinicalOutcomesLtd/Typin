namespace Typin.Hosting.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typin.Hosting;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="ICliComponentsCollection"/> dynamic command releated extensions.
    /// </summary>
    public static class DynamicCommandComponentsExtensions
    {
        /// <summary>
        /// Adds a dynamic command of specified type to the application.
        /// </summary>
        public static ICliComponentsCollection AddDynamicCommand<T>(this ICliComponentsCollection cli)
            where T : IDynamicCommand
        {
            return cli.AddDynamicCommand(typeof(T));
        }

        /// <summary>
        /// Adds multiple commands to the application.
        /// </summary>
        public static ICliComponentsCollection AddDynamicCommands(this ICliComponentsCollection cli, IEnumerable<Type> commandTypes)
        {
            foreach (Type commandType in commandTypes)
            {
                cli.AddDynamicCommand(commandType);
            }

            return cli;
        }

        /// <summary>
        /// Adds dynamic commands from the specified assembly to the application.
        /// Only adds public valid command types.
        /// </summary>
        public static ICliComponentsCollection AddDynamicCommandsFrom(this ICliComponentsCollection cli, Assembly commandAssembly)
        {
            foreach (Type commandType in commandAssembly.ExportedTypes.Where(KnownTypesHelpers.IsCommandType))
            {
                cli.AddDynamicCommand(commandType);
            }

            return cli;
        }

        /// <summary>
        /// Adds dynamic  commands from the specified assemblies to the application.
        /// Only adds public valid command types.
        /// </summary>
        public static ICliComponentsCollection AddDynamicCommandsFrom(this ICliComponentsCollection cli, IEnumerable<Assembly> commandAssemblies)
        {
            foreach (Assembly commandAssembly in commandAssemblies)
            {
                cli.AddDynamicCommandsFrom(commandAssembly);
            }

            return cli;
        }

        /// <summary>
        /// Adds dynamic commands from the calling assembly to the application.
        /// Only adds public valid command types.
        /// </summary>
        public static ICliComponentsCollection AddDynamicCommandsFromThisAssembly(this ICliComponentsCollection cli)
        {
            return cli.AddDynamicCommandsFrom(Assembly.GetCallingAssembly());
        }
    }
}
