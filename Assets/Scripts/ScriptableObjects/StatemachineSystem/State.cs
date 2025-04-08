using System;
using Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.StatemachineSystem
{
    [Serializable]
    public abstract class State : ScriptableObject
    {
        public abstract void OnStateEnter(GameObject stateOwner);
        public abstract void UpdateState(GameObject stateOwner);
        public abstract void OnStateExit(GameObject stateOwner);

        public State CreateInstance() => this.GetCopy<State>();
    }
}