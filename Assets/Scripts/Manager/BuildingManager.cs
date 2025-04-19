using System;
using Base;
using Game;
using Game.Statemachine;
using Game.States;
using ScriptableObjects.Buildings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class BuildingManager : BaseSingleton<BuildingManager>
    {
        [SerializeField] [Min(0.1f)] private float buildingPreviewInterpolationSpeed;
        [SerializeField] private PlacedBuildingsManager placedBuildingsManager;
        [SerializeField] private BuildingData currentlySelectedBuildingToBuild;
        [SerializeField] private BuildingPreview buildingPreview;
        [SerializeField] private ResourceManager resourceManager;

        [Header("States")] [SerializeField] private StateMachine buildingStateMachine;

        [SerializeField] private BuildingState buildingStateTemplate;

        private bool _continousBuildingIsActive;
        private int _continousBuildingsBuilt;

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

        public event Action OnBuildingModeEntered;
        public event Action OnBuildingModeExited;
        public event Action<Building> OnBuildingBuilt;

        public void SetCurrentlySelecetedBuildingToBuild(BuildingData building)
        {
            OnBuildingModeEntered?.Invoke();
            currentlySelectedBuildingToBuild = building;
            buildingPreview.Init(currentlySelectedBuildingToBuild.BuildingPrefab.BuildingSprite);
            buildingStateMachine.SetCurrentState(buildingStateTemplate.CreateInstance());
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

                ExitBuildingMode();
            }
        }

        public void PlaceCurrentlySelectedBuildingToBuild(InputAction.CallbackContext context)
        {
            if (!context.performed || !HasCurrentlySelectedBuilding()) return;

            GridTile tileToPlaceBuildingOn = MouseDataManager.Instance.CurrentlyHoveredTile;

            if (!CanPlaceBuilding(tileToPlaceBuildingOn, currentlySelectedBuildingToBuild)) return;

            Debug.Log("Placing building");
            PlaceBuilding(tileToPlaceBuildingOn);
        }

        public void OnExitBuildingMode(InputAction.CallbackContext context)
        {
            if (context.performed) ExitBuildingMode();
        }

        private void ExitBuildingMode()
        {
            currentlySelectedBuildingToBuild = null;
            buildingStateMachine.SetIdle();
            _continousBuildingsBuilt = 0;
            buildingPreview.Disable();
            OnBuildingModeExited?.Invoke();
        }

        private void SetBuildingPreviewPosition(GridTile currentlyHoveredTile)
        {
            if (currentlyHoveredTile == null) return;

            Vector3 buildingPreviewPosition = Vector3.Lerp(buildingPreview.GetPosition(),
                currentlyHoveredTile.CellCenter,
                Time.deltaTime * buildingPreviewInterpolationSpeed);
            buildingPreview.SetPosition(buildingPreviewPosition);
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
                buildingPreview.ChangeSpriteState(
                    CanPlaceBuilding(currentlyHoveredTile, currentlySelectedBuildingToBuild)
                    && resourceManager.HasEnoughResourcesForBuilding(currentlySelectedBuildingToBuild));
            }
        }

        private void PlaceBuilding(GridTile tileToPlaceBuildingOn)
        {
            //Also check for sufficient resources availablity of the tile etc
            Vector2 buildingLocation = tileToPlaceBuildingOn.CellCenter;

            Building buildingInstance = Instantiate(currentlySelectedBuildingToBuild.BuildingPrefab, buildingLocation,
                Quaternion.identity);
            buildingInstance.OnBuild(tileToPlaceBuildingOn, currentlySelectedBuildingToBuild.BuildingGuid);

            placedBuildingsManager.AddPlacedBuilding(buildingInstance);
            resourceManager.RemoveResources(currentlySelectedBuildingToBuild.ResourceCosts);
            OnBuildingBuilt?.Invoke(buildingInstance);

            currentlySelectedBuildingToBuild = _continousBuildingIsActive ? currentlySelectedBuildingToBuild : null;

            if (!_continousBuildingIsActive)
            {
                ExitBuildingMode();
                return;
            }

            _continousBuildingsBuilt++;
        }

        public bool IsInBuildingMode()
        {
            return buildingStateMachine.IsInState(buildingStateTemplate);
        }

        private bool HasCurrentlySelectedBuilding()
        {
            return currentlySelectedBuildingToBuild;
        }

        private bool CanPlaceBuilding(GridTile tileToPlaceBuildingOn, BuildingData buildingToPlace)
        {
            if (tileToPlaceBuildingOn == null) return false;

            if (buildingToPlace.IsBaseBuilding)
                return !tileToPlaceBuildingOn.IsOccupied
                       && tileToPlaceBuildingOn.CanHostBaseBuildings
                       && resourceManager.HasEnoughResourcesForBuilding(currentlySelectedBuildingToBuild);

            return !tileToPlaceBuildingOn.IsOccupied
                   && !tileToPlaceBuildingOn.CanHostBaseBuildings;
        }
    }
}