using System;
using System.Collections.Generic;
using Base;
using Game;
using Game.Enums;
using Game.StatemachineSystem;
using ScriptableObjects.Buildings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class BuildingManager : BaseSingleton<BuildingManager>
    {
        [SerializeField] private BuildingData currentlySelectedBuildingToBuild;
        [SerializeField] private List<Building> allPlacedBuildings = new List<Building>();
        [SerializeField] private List<Building> allBuildings = new List<Building>();
        [SerializeField] StateMachine<BuildingManager,BuildingStates> buildingStateMachine;

        public event Action<Building> OnBuildingBuilt;

        private bool _continousBuildingIsActive = false;
        private readonly TickManager _tickManager = new TickManager();
        
        private void Start()
        {
            buildingStateMachine.Init(this);
            _tickManager.StartUpdatingTickables();
        }

        private void OnDestroy()
        {
            _tickManager.StopTicking();
        }

        private void Update()
        {
            _tickManager.Update();
            buildingStateMachine.Update();
        }

        public void SetCurrentlySelecetedBuildingToBuild(BuildingData building)
        {
            currentlySelectedBuildingToBuild = building;
            
            buildingStateMachine.SetCurrentState(nameof(BuildingStates.Building));
        }

        public void SetContinousBuildingIsActive(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _continousBuildingIsActive = true;
                buildingStateMachine.SetCurrentState(nameof(BuildingStates.Building));
                
            }
            else if (context.canceled)
            {
                _continousBuildingIsActive = false;
                buildingStateMachine.SetIdle();
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
            
            currentlySelectedBuildingToBuild = _continousBuildingIsActive ? currentlySelectedBuildingToBuild :  null;

            if (!_continousBuildingIsActive)
            {
                buildingStateMachine.SetIdle();
            }
            else
            {
                buildingStateMachine.SetCurrentState(nameof(BuildingStates.Building));
            }
            
            allPlacedBuildings.Add(buildingInstance);
            
            OnBuildingBuilt?.Invoke(buildingInstance);
        }
        
        public void RegisterResourceGatheringBuilding(ResourceGatheringBuilding building)
        {
            _tickManager.RegisterTickable(building.ResourceTicker);
        }
    }
}