using Game.StatemachineSystem;
using UnityEngine;

namespace Game.Statemachine
{
    public class StateMachine : MonoBehaviour
    {
        
        private State _currentState;

        private void OnValidate()
        {
            _currentState = null;
        }
        
        public void SetIdle()
        {
            if (_currentState != null)
            {
                _currentState.OnStateExit(gameObject);
            }
            
            _currentState = null;
        }
        public void SetCurrentState(State newState)
        {
            if (HasActiveState() && 
                StateEqualsCurrentState(newState))
            {
                return;
            }
            
            if (HasActiveState())
            {
                _currentState.OnStateExit(gameObject);
            }


            _currentState = newState;
            _currentState.OnStateEnter(gameObject);
        }
        
        public void Update()
        {
            if (HasActiveState())
            {
                _currentState.UpdateState(gameObject);
            }
        }

        bool HasActiveState()
        {
            return _currentState;
        }

        bool StateEqualsCurrentState(State newState)
        {
            return _currentState == newState;
        }
    }
}