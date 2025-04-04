﻿using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Game.StatemachineSystem
{
    [Serializable]
    public class StateMachine<T, TStateIdentifier> : ScriptableObject where TStateIdentifier : Enum
    {
        [SerializeField] private List<State<T,TStateIdentifier>> states;
        
        private T _owner;
        private State<T, TStateIdentifier> _currentState;


        public void Init(T owner)
        {
            _owner = owner;
        }

        public void SetIdle()
        {
            if (_currentState != null)
            {
                _currentState.OnStateExit(_owner);
            }
            
            _currentState = null;
        }
        public void SetCurrentState(string stateName)
        {
            State<T, TStateIdentifier> newState = states.Find(x => x.StateIdentifier.ToString() == stateName);

            if (HasActiveState() && 
                StateEqualsCurrentState(newState.StateIdentifier.ToString()))
            {
                return;
            }
            
            if (HasActiveState())
            {
                _currentState.OnStateExit(_owner);
            }


            _currentState = newState;
            _currentState.OnStateEnter(_owner);
        }
        
        public void Update()
        {
            if (HasActiveState())
            {
                _currentState.UpdateState(_owner);
            }
        }

        bool HasActiveState()
        {
            return _currentState;
        }

        bool StateEqualsCurrentState(string stateName)
        {
            return _currentState.StateIdentifier.ToString() == stateName;
        }
    }
}