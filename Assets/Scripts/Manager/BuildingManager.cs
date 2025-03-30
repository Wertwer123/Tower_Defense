using System;
using System.Collections.Generic;
using Base;
using Game;
using ScriptableObjects.Buildings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class BuildingManager : BaseSingleton<BuildingManager>
    {
        [SerializeField] private BuildingData currentlySelectedBuildingToBuild;
        [SerializeField] private List<Building> allPlacedBuildings = new List<Building>();

        public event Action<Building> OnBuildingBuilt; 

        private bool continousBuildingIsActive = false;
        private readonly TickManager _tickManager = new TickManager();
        
        private void Start()
        {
            _tickManager.StartUpdatingTickables();
        }

        private void OnDestroy()
        {
            _tickManager.StopTicking();
        }

        private void Update()
        {
            _tickManager.Update();
        }

        public void SetCurrentlySelecetedBuildingToBuild(BuildingData building)
        {
            currentlySelectedBuildingToBuild = building;
        }

        public void SetContinousBuildingIsActive(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                continousBuildingIsActive = true;
                
            }
            else if (context.canceled)
            {
                continousBuildingIsActive = false;
            }
        }
        public void PlaceCurrentlySelectedBuildingToBuild(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            
            GridTile tileToPlaceBuildingOn = MouseDataManager.Instance.CurrentlyHoveredTile;
           
            if (tileToPlaceBuildingOn == null 
                || tileToPlaceBuildingOn.IsOccupied 
                || !tileToPlaceBuildingOn.CanHostBaseBuildings
                || currentlySelectedBuildingToBuild == null)
            {
                return;
            }
            
            //Also check for sufficient resources availablity of the tile etc
            Vector2 buildingLocation = tileToPlaceBuildingOn.Position;
            var buildingInstance = Instantiate(currentlySelectedBuildingToBuild.BuildingPrefab, buildingLocation, Quaternion.identity);
            buildingInstance.OnBuild(tileToPlaceBuildingOn);
            
            currentlySelectedBuildingToBuild = continousBuildingIsActive ? currentlySelectedBuildingToBuild :  null;
            allPlacedBuildings.Add(buildingInstance);
            
            OnBuildingBuilt?.Invoke(buildingInstance);
        }
        
        public void RegisterResourceGatheringBuilding(ResourceGatheringBuilding building)
        {
            _tickManager.RegisterTickable(building.ResourceTicker);
        }
    }
}