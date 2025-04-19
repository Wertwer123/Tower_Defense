using Game.StatemachineSystem;
using UnityEngine;

namespace Game.Statemachine
{
    public class StateMachine : MonoBehaviour
    {
        private State _currentState;

        public void Update()
        {
            if (HasActiveState()) _currentState.UpdateState(gameObject);
        }

        private void OnValidate()
        {
            _currentState = null;
        }

        public void SetIdle()
        {
            if (_currentState != null) _currentState.OnStateExit(gameObject);

            _currentState = null;
        }

        public void SetCurrentState(State newState)
        {
            if (HasActiveState() &&
                StateEqualsCurrentState(newState))
                return;

            if (HasActiveState()) _currentState.OnStateExit(gameObject);


            _currentState = newState;
            _currentState.OnStateEnter(gameObject);
        }

        public bool IsInState(State state)
        {
            if (_currentState == null)
                return false;

            return _currentState.GetType().Name == state.GetType().Name;
        }

        private bool HasActiveState()
        {
            return _currentState;
        }

        private bool StateEqualsCurrentState(State newState)
        {
            return _currentState == newState;
        }
    }
}