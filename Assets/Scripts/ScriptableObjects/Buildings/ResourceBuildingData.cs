using Game.Enums;
using UnityEngine;

namespace ScriptableObjects.Buildings
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/ResourceBuildingData", order = 2)]
    public class ResourceBuildingData : BuildingData
    {
        [SerializeField, Min(0.1f)] float resourceProductionRatePerSecond;
        [SerializeField] private ResourceType resourceProductionType;
        
        public float ResourceProductionRatePerSecond => resourceProductionRatePerSecond;
        public ResourceType ResourceProductionType => resourceProductionType;
    }
}