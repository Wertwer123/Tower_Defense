using System;
using Base;
using Extensions;
using Manager;
using ScriptableObjects.Buildings;
using SpriteAnimation;
using UnityEngine;

namespace Game
{
    public class ResourceGatheringBuilding : Building
    {
        [SerializeField] ResourceBuildingData resourceBuildingData;
        [SerializeField] SpriteAnimTemplate produceResourceAnimation;
        
        private ResourceManager _resourceManager;
        private Ticker _resourceTicker;
        public Ticker ResourceTicker => _resourceTicker;

        private void OnEnable()
        {
            _resourceTicker = new Ticker(1.0f / resourceBuildingData.ResourceProductionRatePerSecond, ProduceResources);
        }
        
        public void AssignResourceManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }
        
        private void ProduceResources()
        {
            foreach (ResourceValue resource in resourceBuildingData.ProducedResources)
            {
                _resourceManager.AddResource(resource);
                Debug.Log("produced a resource");
            }
            
            produceResourceAnimation.GetCopy<SpriteAnimTemplate>().PlayAnimation(this);
        }
    }
}