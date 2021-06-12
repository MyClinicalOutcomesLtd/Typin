namespace Typin.Hosting.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Input;

    /// <summary>
    /// Input provider related service colleciton extensions.
    /// </summary>
    public static class InputProviderServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an input provider with singleton lifetime.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInputProvider<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class, IInputProvider
            where TImplementation : class, TInterface
        {
            services.AddSingleton<TInterface, TImplementation>();
            services.AddSingleton<IInputProvider>(provider => provider.GetRequiredService<TInterface>());

            return services;
        }

        /// <summary>
        /// Adds an input provider with singleton lifetime.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="services"></param>
        /// <param name="implementation"></param>
        /// <returns></returns>
        public static IServiceCollection AddInputProvider<TInterface>(this IServiceCollection services, TInterface implementation)
            where TInterface : class, IInputProvider
        {
            services.AddSingleton<TInterface>(implementation);
            services.AddSingleton<IInputProvider>(provider => provider.GetRequiredService<TInterface>());

            return services;
        }
    }
}
