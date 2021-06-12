namespace Typin.Hosting.Builder
{
    using Typin.Modes;

    /// <summary>
    /// <see cref="ICliComponentsCollection"/> mode releated extensions.
    /// </summary>
    public static class ModeComponentsExtensions
    {
        /// <summary>
        /// Registers a CLI mode. Only one mode can be registered as startup mode.
        /// If no mode was registered or none of the registered modes was marked as startup, <see cref="DirectMode"/> will be registered.
        ///
        /// Do not call RegisterMode directly from builder, instead call UseXMode method, e.g. UseDirectMode().
        /// </summary>
        public static ICliComponentsCollection RegisterMode<T>(this ICliComponentsCollection cli, bool asStartup = false)
            where T : ICliMode
        {
            return cli.RegisterMode(typeof(T), asStartup);
        }
    }
}
