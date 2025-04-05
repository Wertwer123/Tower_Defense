using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Manager
{
    public class PlacedBuildingsManager : MonoBehaviour
    {
        [SerializeField] private List<Building> allPlacedBuildings = new List<Building>();
        
        private readonly TickManager _tickManager = new TickManager();

        private void Start()
        {
            _tickManager.StartUpdatingTickables();
        }

        private void Update()
        {
            _tickManager.Update();
        }
        
        private void OnDestroy()
        {
            _tickManager.StopTicking();
        }
        
        public void AddPlacedBuilding(Building building)
        {
            allPlacedBuildings.Add(building);

            if (building is ResourceGatheringBuilding buildingCastToResourceGatheringBuilding)
            {
                _tickManager.RegisterTickable(buildingCastToResourceGatheringBuilding.ResourceTicker);
            }
        }

        public void RemovePlacedBuilding(Building building)
        {
            allPlacedBuildings.Remove(building);
        }
    }
}