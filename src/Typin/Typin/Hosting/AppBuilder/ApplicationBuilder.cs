﻿namespace Typin.Hosting.Builder
{
    using System;
    using System.Collections.Generic;
    using Typin.Hosting;

    /// <summary>
    /// Default implementation for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public class ApplicationBuilder : IApplicationBuilder
    {
        private const string ServerFeaturesKey = "server.Features";
        private const string ApplicationServicesKey = "application.Services";

        private readonly List<Func<RequestDelegate, RequestDelegate>> _components = new();

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for application services.</param>
        public ApplicationBuilder(IServiceProvider serviceProvider)
        {
            Properties = new Dictionary<string, object?>(StringComparer.Ordinal);
            ApplicationServices = serviceProvider;
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> for application services.
        /// </summary>
        public IServiceProvider ApplicationServices
        {
            get => GetProperty<IServiceProvider>(ApplicationServicesKey)!;
            set => SetProperty(ApplicationServicesKey, value);
        }

        /// <summary>
        /// Gets a set of properties for <see cref="ApplicationBuilder"/>.
        /// </summary>
        public IDictionary<string, object?> Properties { get; }

        private T? GetProperty<T>(string key)
        {
            return Properties.TryGetValue(key, out var value) ? (T?)value : default;
        }

        private void SetProperty<T>(string key, T value)
        {
            Properties[key] = value;
        }

        /// <summary>
        /// Adds the middleware to the application request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns>An instance of <see cref="IApplicationBuilder"/> after the operation has completed.</returns>
        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _components.Add(middleware);

            return this;
        }

        /// <summary>
        /// Produces a <see cref="RequestDelegate"/> that executes added middlewares.
        /// </summary>
        /// <returns>The <see cref="RequestDelegate"/>.</returns>
        public RequestDelegate Build()
        {
            //RequestDelegate app = context =>
            //{
            //    //// If we reach the end of the pipeline, but we have an endpoint, then something unexpected has happened.
            //    //// This could happen if user code sets an endpoint, but they forgot to add the UseEndpoint middleware.
            //    //var endpoint = context.GetEndpoint();
            //    //var endpointRequestDelegate = endpoint?.RequestDelegate;
            //    //if (endpointRequestDelegate != null)
            //    //{
            //    //    var message =
            //    //        $"The request reached the end of the pipeline without executing the endpoint: '{endpoint!.DisplayName}'. " +
            //    //        $"Please register the EndpointMiddleware using '{nameof(IApplicationBuilder)}.UseEndpoints(...)' if using " +
            //    //        $"routing.";
            //    //    throw new InvalidOperationException(message);
            //    //}

            //    //context.Response.StatusCode = StatusCodes.Status404NotFound;
            //    return Task.CompletedTask;
            //};

            //for (var c = _components.Count - 1; c >= 0; c--)
            //{
            //    app = _components[c](app);
            //}

            return null!;//app;
        }
    }
}