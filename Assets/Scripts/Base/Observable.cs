using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace Base
{
    public abstract class Observable<T> : MonoBehaviour
    {
        private readonly HashSet<IObserver<T>> _observers = new ();
        
        public void Subscribe(IObserver<T> observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver<T> observer)
        {
            _observers.Remove(observer);
        }

        protected void Notify(T value)
        {
            foreach (IObserver<T> observer in _observers)
            {
                observer.Notify(value);
            }
        } 
    }
}