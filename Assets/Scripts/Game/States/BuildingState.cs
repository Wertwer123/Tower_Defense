using System.Collections.Generic;
using Game.Enums;
using Game.StatemachineSystem;
using Manager;
using UnityEngine;
using Grid = Base.Grid;

namespace Game.States
{
    [CreateAssetMenu(fileName = "BuildingState", menuName = "States/BuildingState", order = 1)]
    public class BuildingState : State<BuildingManager, BuildingStates>
    {
        [SerializeField, ColorUsage(true, true)] Color fromColor = Color.white;
        [SerializeField, ColorUsage(true, true)] Color toColor = Color.white;

        private float _timePassed;
        
        public override void OnStateEnter(BuildingManager stateOwner)
        {
           _timePassed = 0f;
        }

        public override void UpdateState(BuildingManager stateOwner)
        {
            _timePassed += Time.deltaTime;
            
            GridEffectsManager.Instance.SetGridColor(toColor);
            Debug.Log(_timePassed);
           
        }

        public override void OnStateExit(BuildingManager stateOwner)
        {
            GridEffectsManager.Instance.SetGridColor(fromColor);
        }
    }
}