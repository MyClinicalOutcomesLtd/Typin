namespace SimpleAppExample
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Typin;
    using Typin.Console;
    using Typin.Hosting;
    using Typin.Hosting.Builder;
    using Typin.Hosting.Startup;
    using Typin.Modes;

    public class Startup : IStartup
    {
        /// <inheritdoc/>
        public void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
        {
            services.AddCli(cli =>
            {
                cli.AddCommandsFromThisAssembly();
                cli.AddDirectMode(true);
                cli.AddConsole<SystemConsole>();
            });

            services.AddSingleton<ApplicationMetadata>(new ApplicationMetadata("App", "exe", "1.0", "Loream"));
        }

        /// <inheritdoc/>
        public void Configure(IApplicationBuilder app)
        {

        }
    }
}