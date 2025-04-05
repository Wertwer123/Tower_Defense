using System;
using UnityEngine;

namespace Game.StatemachineSystem
{
    [Serializable]
    public abstract class State<T,TIdentifier> : ScriptableObject
    {
        [SerializeField] TIdentifier stateIdentifier;
        public abstract void OnStateEnter(T stateOwner);
        public abstract void UpdateState(T stateOwner);
        public abstract void OnStateExit(T stateOwner);

        public TIdentifier StateIdentifier => stateIdentifier;
    }
}