namespace Typin.Hosting.Builder
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Hosting;

    /// <summary>
    /// Cli service collection extensions.
    /// </summary>
    public static class CliServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Typin command line components.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cli"></param>
        /// <returns></returns>
        public static IServiceCollection AddCli(this IServiceCollection services, Action<ICliComponentsCollection> cli)
        {
            ICliComponentsCollection cliComponentsCollection = new CliComponentsCollection(services);
            cli(cliComponentsCollection);

            ApplicationConfiguration configuration = new(cliComponentsCollection.Modes,
                                                         cliComponentsCollection.Commands,
                                                         cliComponentsCollection.Directives,
                                                         null!,
                                                         cliComponentsCollection.StartupMode!);

            services.AddSingleton(configuration);

            return services;
        }
    }
}
