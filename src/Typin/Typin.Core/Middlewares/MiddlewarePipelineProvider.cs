namespace Typin.Middlewares
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides application middlewares.
    /// </summary>
    public sealed class MiddlewarePipelineProvider
    {
        /// <summary>
        /// Registered middlewares.
        /// </summary>
        public LinkedList<Type> Middlewares { get; set; } = new();
    }
}
