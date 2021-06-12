namespace Typin.Input
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Command line input provider.
    /// </summary>
    public interface IInputProvider
    {
        /// <summary>
        /// Get input.
        /// </summary>
        /// <returns>Command line input.</returns>
        ValueTask<IEnumerable<string>> GetInputAsync(CancellationToken cancellationToken);
    }
}
