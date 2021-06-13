namespace Typin.Hosting.Builder
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Exceptions;

    /// <summary>
    /// <see cref="ICliComponentsCollection"/> exceptions releated extensions.
    /// </summary>
    public static class ExceptionsComponentsExtensions
    {
        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="ICliExceptionHandler"/>.
        /// Exception handler is configured as scoped service.
        /// </summary>
        public static ICliComponentsCollection UseExceptionHandler(this ICliComponentsCollection cli, Type exceptionHandlerType)
        {
            cli.Services.AddScoped(typeof(ICliExceptionHandler), exceptionHandlerType);

            return cli;
        }

        /// <summary>
        /// Configures the application to use the specified implementation of <see cref="ICliExceptionHandler"/>.
        /// Exception handler is configured as scoped service.
        /// </summary>
        public static ICliComponentsCollection UseExceptionHandler<T>(this ICliComponentsCollection cli)
            where T : class, ICliExceptionHandler
        {
            return cli.UseExceptionHandler(typeof(T));
        }
    }
}
