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
        [SerializeField] private ResourceBuildingData resourceBuildingData;
        [SerializeField] private SpriteAnimTemplate produceResourceAnimation;

        private ResourceManager _resourceManager;
        public Ticker ResourceTicker { get; private set; }

        private void OnEnable()
        {
            ResourceTicker = new Ticker(1.0f / resourceBuildingData.ResourceProductionRatePerSecond, ProduceResources);
        }

        public void AssignResourceManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        private void ProduceResources()
        {
            foreach (ResourceValue resource in resourceBuildingData.ProducedResources)
                _resourceManager.AddResource(resource);

            produceResourceAnimation.GetCopy<SpriteAnimTemplate>().PlayAnimation(this);
        }
    }
}