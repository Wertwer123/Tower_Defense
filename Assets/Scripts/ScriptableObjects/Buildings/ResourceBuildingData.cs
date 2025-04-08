using System.Collections.Generic;
using Game;
using Game.Enums;
using UnityEngine;

namespace ScriptableObjects.Buildings
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/ResourceBuildingData", order = 2)]
    public class ResourceBuildingData : BuildingData
    {
        [SerializeField, Min(0.1f)] float resourceProductionRatePerSecond;
        [SerializeField] private List<ResourceValue> producedResourcesPerTick;
        
        public float ResourceProductionRatePerSecond => resourceProductionRatePerSecond;
        public List<ResourceValue> ProducedResources => producedResourcesPerTick;
    }
}