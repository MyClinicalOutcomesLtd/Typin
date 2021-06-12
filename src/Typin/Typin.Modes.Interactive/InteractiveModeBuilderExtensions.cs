namespace Typin.Modes
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Commands;
    using Typin.Directives;
    using Typin.Hosting.Builder;
    using Typin.Hosting.Extensions;

    /// <summary>
    /// <see cref="ICliComponentsCollection"/> interactive mode configuration extensions.
    /// </summary>
    public static class InteractiveModeBuilderExtensions
    {
        /// <summary>
        /// Adds an interactive mode to the application (enabled with [interactive] directive or `interactive` command).
        /// By default this adds [interactive], [default], [>], [.], and [..], as well as `interactive` command and advanced command input.
        /// </summary>
        public static ICliComponentsCollection AddInteractiveMode(this ICliComponentsCollection cli,
                                                                  bool asStartup = false,
                                                                  Action<InteractiveModeOptions>? options = null,
                                                                  InteractiveModeBuilderSettings? builderSettings = null)
        {
            builderSettings ??= new InteractiveModeBuilderSettings();

            cli.RegisterMode<InteractiveMode>(asStartup);

            options ??= (InteractiveModeOptions cfg) => { };
            cli.Services.Configure(options);
            cli.Services.AddInputProvider<IInteractiveInputProvider, InteractiveInputProvider>();

            cli.AddDirective<DefaultDirective>();

            if (builderSettings.AddInteractiveCommand)
            {
                cli.AddCommand<InteractiveCommand>();
            }

            if (builderSettings.AddInteractiveDirective)
            {
                cli.AddDirective<InteractiveDirective>();
            }

            if (builderSettings.AddScopeDirectives)
            {
                cli.AddDirective<ScopeDirective>();
                cli.AddDirective<ScopeResetDirective>();
                cli.AddDirective<ScopeUpDirective>();
            }

            return cli;
        }
    }
}
