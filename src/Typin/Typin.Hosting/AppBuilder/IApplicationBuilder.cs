namespace Typin.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an async continuation for the next task to execute in the pipeline.
    /// </summary>
    /// <returns>Awaitable task</returns>
    public delegate ValueTask RequestDelegate();

    /// <summary>
    /// Defines a class that provides the mechanisms to configure an application's request pipeline.
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the application's service container.
        /// </summary>
        IServiceProvider ApplicationServices { get; set; }

        /// <summary>
        /// Gets a key/value collection that can be used to share data between middleware.
        /// </summary>
        IDictionary<string, object?> Properties { get; }

        /// <summary>
        /// Adds a middleware delegate to the application's request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware delegate.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);

        /// <summary>
        /// Builds the delegate used by this application to process HTTP requests.
        /// </summary>
        /// <returns>The request handling delegate.</returns>
        RequestDelegate Build();
    }
}