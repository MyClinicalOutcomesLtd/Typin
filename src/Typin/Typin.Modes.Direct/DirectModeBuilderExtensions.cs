namespace Typin.Modes.Direct
{
    using Typin.Hosting.Builder;

    /// <summary>
    /// <see cref="ICliComponentsCollection"/> direct mode configuration extensions.
    /// </summary>
    public static class DirectModeBuilderExtensions
    {
        /// <summary>
        /// Adds a direct mode to the application.
        /// </summary>
        public static ICliComponentsCollection AddDirectMode(this ICliComponentsCollection cli,
                                                             bool asStartup = false)
        {
            cli.RegisterMode<DirectMode>(asStartup);

            return cli;
        }
    }
}
