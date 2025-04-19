using System;

namespace Interfaces
{
    /// <summary>
    ///     If implementing this interface also implement on pointer enter and exit
    /// </summary>
    public interface IBlockingUIElement
    {
        public event Action OnBlockingUIElementEntered;
        public event Action OnBlockingUIElementExited;
        public event Action OnBlockingUIElementDestroyed;
    }
}