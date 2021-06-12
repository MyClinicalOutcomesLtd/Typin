namespace Typin.Hosting.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="ICliComponentsCollection"/> command releated extensions.
    /// </summary>
    public static class CommandComponentsExtensions
    {
        /// <summary>
        /// Adds a command of specified type to the application.
        /// </summary>
        public static ICliComponentsCollection AddCommand<T>(this ICliComponentsCollection cli)
            where T : ICommand
        {
            return cli.AddCommand(typeof(T));
        }

        /// <summary>
        /// Adds multiple commands to the application.
        /// </summary>
        public static ICliComponentsCollection AddCommands(this ICliComponentsCollection cli, IEnumerable<Type> commandTypes)
        {
            foreach (Type commandType in commandTypes)
            {
                cli.AddCommand(commandType);
            }

            return cli;
        }

        /// <summary>
        /// Adds commands from the specified assembly to the application.
        /// Only adds public valid command types.
        /// </summary>
        public static ICliComponentsCollection AddCommandsFrom(this ICliComponentsCollection cli, Assembly commandAssembly)
        {
            foreach (Type commandType in commandAssembly.ExportedTypes.Where(KnownTypesHelpers.IsCommandType))
            {
                cli.AddCommand(commandType);
            }

            return cli;
        }

        /// <summary>
        /// Adds commands from the specified assemblies to the application.
        /// Only adds public valid command types.
        /// </summary>
        public static ICliComponentsCollection AddCommandsFrom(this ICliComponentsCollection cli, IEnumerable<Assembly> commandAssemblies)
        {
            foreach (Assembly commandAssembly in commandAssemblies)
            {
                cli.AddCommandsFrom(commandAssembly);
            }

            return cli;
        }

        /// <summary>
        /// Adds commands from the calling assembly to the application.
        /// Only adds public valid command types.
        /// </summary>
        public static ICliComponentsCollection AddCommandsFromThisAssembly(this ICliComponentsCollection cli)
        {
            return cli.AddCommandsFrom(Assembly.GetCallingAssembly());
        }
    }
}
