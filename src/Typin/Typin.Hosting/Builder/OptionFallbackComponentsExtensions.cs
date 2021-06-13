namespace Typin.Hosting.Builder
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.OptionFallback;

    /// <summary>
    /// <see cref="ICliComponentsCollection"/> exceptions releated extensions.
    /// </summary>
    public static class OptionFallbackComponentsExtensions
    {
        /// <summary>
        /// Configures to use a specific option fallback provider with desired lifetime <see cref="EnvironmentVariableFallbackProvider"/>.
        /// </summary>
        public static ICliComponentsCollection UseOptionFallbackProvider(this ICliComponentsCollection cli, Type fallbackProviderType, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            cli.Services.Add(new ServiceDescriptor(typeof(IOptionFallbackProvider), fallbackProviderType, lifetime));

            return cli;
        }

        /// <summary>
        /// Configures to use a specific option fallback provider with desired lifetime <see cref="EnvironmentVariableFallbackProvider"/>.
        /// </summary>
        public static ICliComponentsCollection UseOptionFallbackProvider<T>(this ICliComponentsCollection cli, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where T : IOptionFallbackProvider
        {
            return cli.UseOptionFallbackProvider(typeof(T), lifetime);
        }
    }
}
