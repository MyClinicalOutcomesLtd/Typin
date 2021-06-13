namespace SimpleAppExample
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Typin;
    using Typin.Consoles.System;
    using Typin.Hosting;
    using Typin.Hosting.Builder;
    using Typin.Hosting.Startup;
    using Typin.Middlewares;
    using Typin.Modes.Interactive;

    public class Startup : IStartup
    {
        /// <inheritdoc/>
        public void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
        {
            services.AddCli(cli =>
            {
                cli.AddCommandsFromThisAssembly();
                //cli.AddDirectMode(true);
                cli.AddInteractiveMode(true);
                cli.AddConsole<SystemConsole>();
            });

            services.AddScoped<ResolveCommandSchemaAndInstance>()
                    .AddScoped<InitializeDirectives>()
                    .AddScoped<ExecuteDirectivesSubpipeline>()
                    .AddScoped<HandleSpecialOptions>()
                    .AddScoped<BindInput>()
                    .AddScoped<ExecuteCommand>();

            services.AddSingleton<ApplicationMetadata>(new ApplicationMetadata("App", "exe", "1.0", "Loream"));
        }

        /// <inheritdoc/>
        public void Configure(IApplicationBuilder app)
        {
            app.Use<ResolveCommandSchemaAndInstance>();
            app.Use<InitializeDirectives>();
            app.Use<ExecuteDirectivesSubpipeline>();
            app.Use<HandleSpecialOptions>();
            app.Use<BindInput>();
            app.Use<ExecuteCommand>();
        }
    }
}