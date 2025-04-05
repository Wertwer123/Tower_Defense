using System.Collections.Generic;
using Game.Enums;
using Game.StatemachineSystem;
using Manager;
using UnityEngine;

namespace Game.States
{
    [CreateAssetMenu(fileName = "BuildingState", menuName = "States/BuildingState", order = 1)]
    public class BuildingState : State<BuildingManager, BuildingStates>
    {
        [SerializeField, Min(0.0f)] float colorInterpolationSpeed = 0.5f;
        [SerializeField, ColorUsage( true)] Color fromColor = Color.white;
        [SerializeField, ColorUsage( true)] Color toColor = Color.white;

        private float _timePassed;

        public override void OnStateEnter(BuildingManager stateOwner)
        {
            GridEffectsManager.Instance.SetGridAlpha(1.0f);
        }

        public override void UpdateState(BuildingManager stateOwner)
        {
            _timePassed += Time.deltaTime;
            
           float t = Mathf.Abs(Mathf.Sin(_timePassed * colorInterpolationSpeed));
           
           Color gridColor = Color.Lerp(fromColor, toColor, t);
           
           GridEffectsManager.Instance.SetGridColor(gridColor);
        }

        public override void OnStateExit(BuildingManager stateOwner)
        {
            _timePassed = 0.0f;
            GridEffectsManager.Instance.SetGridAlpha(0.0f);
        }
    }
}