using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Manager
{
    public class PlacedBuildingsManager : MonoBehaviour
    {
        [SerializeField] private List<Building> allPlacedBuildings = new List<Building>();
        [SerializeField] private ResourceManager resourceManager;
        
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
                Debug.Log("placed a resource building");
                _tickManager.RegisterTickable(buildingCastToResourceGatheringBuilding.ResourceTicker);
                buildingCastToResourceGatheringBuilding.AssignResourceManager(resourceManager);
            }
        }

        public void RemovePlacedBuilding(Building building)
        {
            allPlacedBuildings.Remove(building);
        }
    }
}