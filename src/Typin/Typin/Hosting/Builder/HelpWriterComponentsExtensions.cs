namespace Typin.Hosting.Builder
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Help;

    /// <summary>
    /// <see cref="ICliComponentsCollection"/> exceptions releated extensions.
    /// </summary>
    public static class HelpWriterComponentsExtensions
    {
        /// <summary>
        /// Configures to use a specific help writer with transient lifetime.
        /// </summary>
        public static ICliComponentsCollection UseHelpWriter(this ICliComponentsCollection cli, Type helpWriterType)
        {
            cli.Services.AddTransient(typeof(IHelpWriter), helpWriterType);

            return cli;
        }

        /// <summary>
        /// Configures to use a specific help writer with transient lifetime.
        /// </summary>
        public static ICliComponentsCollection UseHelpWriter<T>(this ICliComponentsCollection cli)
            where T : IHelpWriter
        {
            return cli.UseHelpWriter(typeof(T));
        }
    }
}
