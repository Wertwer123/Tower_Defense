using System;
using Base;
using Interfaces;
using Manager;
using ScriptableObjects.Buildings;
using UnityEngine;

namespace Game
{
    public class ResourceGatheringBuilding : Building
    {
        [SerializeField] ResourceBuildingData resourceBuildingData;
        
        private Ticker _resourceTicker;
        public Ticker ResourceTicker => _resourceTicker;
        
        private void Start()
        {
            _resourceTicker = new Ticker(1.0f / resourceBuildingData.ResourceProductionRatePerSecond, ProduceResource);
            
            BuildingManager.Instance.RegisterResourceGatheringBuilding(this);
        }

        private void ProduceResource()
        {
            
        }

        public override void OnBuild(GridTile tileBuildingGetsPlacedOn)
        {
            base.OnBuild(tileBuildingGetsPlacedOn);
            Debug.Log("OnBuild");
        }
    }
}