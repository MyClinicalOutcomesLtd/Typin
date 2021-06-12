namespace Typin.Hosting
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Typin command line host builder extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds and configures an ASP.NET Core web application.
        /// </summary>
        public static IHostBuilder ConfigureCliHost(this IHostBuilder builder, Action<ICliHostBuilder> configure)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            return builder.ConfigureCliHost(configure, _ => { });
        }

        /// <summary>
        /// Adds and configures a Typin command line application.
        /// </summary>
        public static IHostBuilder ConfigureCliHost(this IHostBuilder builder, Action<ICliHostBuilder> configure, Action<CliHostBuilderOptions> configureCliHostBuilder)
        {
            _ = configure ?? throw new ArgumentNullException(nameof(configure));

            CliHostBuilderOptions cliHostBuilderOptions = new();
            configureCliHostBuilder(cliHostBuilderOptions);

            CliHostBuilder webhostBuilder = new(builder, cliHostBuilderOptions);
            configure(webhostBuilder);

            builder.ConfigureServices((context, services) => services.AddHostedService<CliHostService>());

            return builder;
        }
    }
}
