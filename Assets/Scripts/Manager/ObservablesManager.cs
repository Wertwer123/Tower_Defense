using System;
using System.Collections.Generic;
using Base;

namespace Manager
{
    public class ObservablesManager : BaseSingleton<ObservablesManager>
    {
        private Dictionary<Type, object> _observables = new();

        public Observable<TObservedValueType> GetObservable<T, TObservedValueType>()
        {
            Type type = typeof(T);

            if (ObservableIsRegisteredForType(type))
            {
                return (Observable<TObservedValueType>)_observables[type];
            }

            return null;
        }

        public void RegisterObservable(Type type, object observable)
        {
            if (!ObservableIsRegisteredForType(type))
            {
                _observables.Add(type, observable);
            }
        }

        public bool ObservableIsRegisteredForType(Type observableType)
        {
            return _observables.ContainsKey(observableType);
        }
    }
}