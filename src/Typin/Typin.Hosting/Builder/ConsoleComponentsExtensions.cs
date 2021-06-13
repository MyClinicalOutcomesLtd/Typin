namespace Typin.Hosting.Builder
{
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Consoles;

    /// <summary>
    /// <see cref="ICliComponentsCollection"/> console releated extensions.
    /// </summary>
    public static class ConsoleComponentsExtensions
    {
        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// Console will not be automatically diposed.
        /// </summary>
        public static ICliComponentsCollection AddConsole(this ICliComponentsCollection cli, IConsole console)
        {
            cli.Services.AddSingleton<IConsole>(console);

            return cli;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="IConsole"/>.
        /// </summary>
        public static ICliComponentsCollection AddConsole<T>(this ICliComponentsCollection cli)
            where T : class, IConsole
        {
            cli.Services.AddSingleton<IConsole, T>();

            return cli;
        }
    }
}
