using Drastic.Services;
using Microsoft.UI.Dispatching;

namespace WinUIPlayground
{
    public class WindowsAppDispatche : IAppDispatcher
    {
        private readonly DispatcherQueue dispatcherQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDispatcher"/> class.
        /// </summary>
        /// <param name="dispatcherQueue">Dispatcher Queue.</param>
        public WindowsAppDispatche(DispatcherQueue dispatcherQueue)
        {
            this.dispatcherQueue = dispatcherQueue ?? throw new ArgumentNullException(nameof(dispatcherQueue));
        }

        /// <inheritdoc/>
        public bool Dispatch(Action action)
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));
            return this.dispatcherQueue.TryEnqueue(() => action());
        }
    }
}
