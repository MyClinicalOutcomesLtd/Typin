namespace Typin
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// CLI mode definition.
    /// </summary>
    public interface ICliMode
    {
        /// <summary>
        /// Executes CLI mode.
        /// </summary>
        ValueTask<int> ExecuteAsync(ICliCommandExecutor executor, CancellationToken cancellationToken);
    }
}
