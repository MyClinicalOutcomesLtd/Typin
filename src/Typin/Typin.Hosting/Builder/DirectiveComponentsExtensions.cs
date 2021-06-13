namespace Typin.Hosting.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typin.Schemas;

    /// <summary>
    /// <see cref="ICliComponentsCollection"/> directive releated extensions.
    /// </summary>
    public static class DirectiveComponentsExtensions
    {
        /// <summary>
        /// Add a custom directive to the application.
        /// </summary>
        public static ICliComponentsCollection AddDirective<T>(this ICliComponentsCollection cli)
            where T : IDirective
        {
            return cli.AddDirective(typeof(T));
        }

        /// <summary>
        /// Add custom directives to the application.
        /// </summary>
        public static ICliComponentsCollection AddDirectives(this ICliComponentsCollection cli, IEnumerable<Type> directiveTypes)
        {
            foreach (Type directiveType in directiveTypes)
            {
                cli.AddDirective(directiveType);
            }

            return cli;
        }

        /// <summary>
        /// Adds directives from the specified assembly to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public static ICliComponentsCollection AddDirectivesFrom(this ICliComponentsCollection cli, Assembly directiveAssembly)
        {
            foreach (Type directiveType in directiveAssembly.ExportedTypes.Where(KnownTypesHelpers.IsDirectiveType))
            {
                cli.AddDirective(directiveType);
            }

            return cli;
        }

        /// <summary>
        /// Adds directives from the specified assemblies to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public static ICliComponentsCollection AddDirectivesFrom(this ICliComponentsCollection cli, IEnumerable<Assembly> directiveAssemblies)
        {
            foreach (Assembly directiveType in directiveAssemblies)
            {
                cli.AddDirectivesFrom(directiveType);
            }

            return cli;
        }

        /// <summary>
        /// Adds directives from the calling assembly to the application.
        /// Only adds public valid directive types.
        /// </summary>
        public static ICliComponentsCollection AddDirectivesFromThisAssembly(this ICliComponentsCollection cli)
        {
            return cli.AddDirectivesFrom(Assembly.GetCallingAssembly());
        }
    }
}
