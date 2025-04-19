using System;
using System.Collections.Generic;
using Base;
using Interfaces;

namespace Manager
{
    public class BlockingUIManager : BaseSingleton<BlockingUIManager>
    {
        private readonly List<IBlockingUIElement> _blockingUIElements = new();

        public event Action OnBlockingUIElementEntered;
        public event Action OnBlockingUIElementExited;

        public void AddBlockingUIElement(IBlockingUIElement blockingUIElement)
        {
            blockingUIElement.OnBlockingUIElementEntered += BlockingElementEntered;
            blockingUIElement.OnBlockingUIElementExited += BlockingElementExited;
            blockingUIElement.OnBlockingUIElementDestroyed += BlockingElementExited;

            _blockingUIElements.Add(blockingUIElement);
        }

        public void RemoveBlockingUIElement(IBlockingUIElement blockingUIElement)
        {
            blockingUIElement.OnBlockingUIElementEntered -= BlockingElementEntered;
            blockingUIElement.OnBlockingUIElementExited -= BlockingElementExited;
            blockingUIElement.OnBlockingUIElementDestroyed -= BlockingElementExited;
            _blockingUIElements.Remove(blockingUIElement);
        }

        private void BlockingElementExited()
        {
            OnBlockingUIElementExited?.Invoke();
        }

        private void BlockingElementEntered()
        {
            OnBlockingUIElementEntered?.Invoke();
        }
    }
}