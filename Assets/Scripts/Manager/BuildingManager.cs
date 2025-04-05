using System;
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
        [SerializeField] private PlacedBuildingsManager placedBuildingsManager;
        [SerializeField] private BuildingData currentlySelectedBuildingToBuild;
        [SerializeField] private BuildingPreview buildingPreview;
        [SerializeField] private StateMachine<BuildingManager,BuildingStates> buildingStateMachine;

        private int _continousBuildingsBuilt;
        private bool _continousBuildingIsActive;
        event Action<Building> OnBuildingBuilt;
        
        private void Start()
        {
            buildingStateMachine.Init(this);
        }

        private void Update()
        {
            buildingStateMachine.Update();

            if (HasCurrentlySelectedBuilding())
            {
                GridTile currentlyHoveredTile = MouseDataManager.Instance.CurrentlyHoveredTile;
                SetBuildingPreviewPosition(currentlyHoveredTile);
                SetBuildingPreviewState(currentlyHoveredTile);
            }
        }

        private void SetBuildingPreviewPosition(GridTile currentlyHoveredTile )
        {
            if (currentlyHoveredTile == null)
            {
                return;
            }
            
            buildingPreview.SetPosition(currentlyHoveredTile.Position);
        }

        private void SetBuildingPreviewState(GridTile currentlyHoveredTile)
        {
            if (currentlyHoveredTile == null)
            {
                buildingPreview.Disable();
            }
            else
            {
                buildingPreview.Enable();
                buildingPreview.ChangeSpriteState(CanPlaceBuilding(currentlyHoveredTile, currentlySelectedBuildingToBuild));
            }
        }
        
        private void PlaceBuilding(GridTile tileToPlaceBuildingOn)
        {
            //Also check for sufficient resources availablity of the tile etc
            Vector2 buildingLocation = tileToPlaceBuildingOn.Position;
            
            var buildingInstance = Instantiate(currentlySelectedBuildingToBuild.BuildingPrefab, buildingLocation, Quaternion.identity);
            buildingInstance.OnBuild(tileToPlaceBuildingOn);
            
            currentlySelectedBuildingToBuild = _continousBuildingIsActive ? currentlySelectedBuildingToBuild :  null;

            if (!_continousBuildingIsActive)
            {
                buildingStateMachine.SetIdle();
                _continousBuildingsBuilt = 0;
                buildingPreview.Disable();
            }

            _continousBuildingsBuilt++;
            placedBuildingsManager.AddPlacedBuilding(buildingInstance);
           
            OnBuildingBuilt?.Invoke(buildingInstance);
        }
        
        private bool HasCurrentlySelectedBuilding()
        {
            return currentlySelectedBuildingToBuild;
        }

        private bool CanPlaceBuilding(GridTile tileToPlaceBuildingOn, BuildingData buildingToPlace)
        {
            if (tileToPlaceBuildingOn == null)
            {
                return false;
            }

            if (buildingToPlace.IsBaseBuilding)
            {
                return !tileToPlaceBuildingOn.IsOccupied 
                       && tileToPlaceBuildingOn.CanHostBaseBuildings;
            }
            
            return !tileToPlaceBuildingOn.IsOccupied
                   && !tileToPlaceBuildingOn.CanHostBaseBuildings;          
        }
        
        public void SetCurrentlySelecetedBuildingToBuild(BuildingData building)
        {
            currentlySelectedBuildingToBuild = building;
            buildingPreview.Init(currentlySelectedBuildingToBuild.BuildingPrefab.BuildingSprite);
            buildingStateMachine.SetCurrentState(nameof(BuildingStates.Building));
        }

        public void SetContinousBuildingIsActive(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _continousBuildingIsActive = true;
            }
            else if (context.canceled)
            {
                _continousBuildingIsActive = false;

                //if we cancel continous building, and we have started building continously then rest the building state
                if (_continousBuildingsBuilt <= 0) return;
                
                currentlySelectedBuildingToBuild = null;
                buildingStateMachine.SetIdle();
            }
        }
        public void PlaceCurrentlySelectedBuildingToBuild(InputAction.CallbackContext context)
        {
            if (!context.performed || !HasCurrentlySelectedBuilding())
            {
                return;
            }
            
            GridTile tileToPlaceBuildingOn = MouseDataManager.Instance.CurrentlyHoveredTile;
           
            if (!CanPlaceBuilding(tileToPlaceBuildingOn, currentlySelectedBuildingToBuild))
            {
                return;
            }
            
            PlaceBuilding(tileToPlaceBuildingOn);
        }
        
        public void UnsubcribeFromBuildingBuilt(Action<Building> onBuildingBuilt)
        {
            OnBuildingBuilt -= onBuildingBuilt;
        }
        public void SubscribeToOnBuildingBuilt(Action<Building> onBuildingBuilt)
        {
            OnBuildingBuilt += onBuildingBuilt;
        }
    }
}