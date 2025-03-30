using Game.Enums;
using Manager;
using UnityEngine;

namespace Game.StatemachineSystem.StatemachineClassInstances
{
    [CreateAssetMenu(menuName = "StatemachineSystem/StatemachineClassInstances/BuildingManagerStateMachine", fileName = "BuildingManagerStatemachine")]
    public class BuildingManagerStateMachine : StateMachine<BuildingManager, BuildingStates> {}
}