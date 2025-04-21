using Game;
using ScriptableObjects.Buildings;
using UI.World;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class BuildingSelectionManager : MonoBehaviour
    {
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private BuildingDatabase buildingDatabase;
        [SerializeField] private BuiltBuildingInformationPanel buildingInformationPanel;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask buildingLayerMask;

        private bool _canSelectBuilding = true;
        private Building _previouslySelectedBuilding;

        private void Start()
        {
            buildingManager.OnBuildingModeEntered += DisableSelection;
            buildingManager.OnBuildingModeExited += EnableSelection;
            BlockingUIManager.Instance.OnBlockingUIElementEntered += DisableSelection;
            BlockingUIManager.Instance.OnBlockingUIElementExited += EnableSelection;
        }

        private void DisableSelection()
        {
            _canSelectBuilding = false;
            _previouslySelectedBuilding = null;
        }

        private void EnableSelection()
        {
            if (buildingManager.IsInBuildingMode())
                return;

            _canSelectBuilding = true;
        }

        public void SelectBuilding(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (!_canSelectBuilding) return;

            //cast a ray from the mouse to get the selected building under the mouse
            Ray ray = mainCamera.ScreenPointToRay(MouseDataManager.Instance.CurrentMousePositionScreen);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, buildingLayerMask);

            if (hit)
            {
                if (hit.collider.TryGetComponent(out Building building))
                {
                    if (IsPreviouslySelectedBuilding(building)) return;

                    BuildingData selectedBuildingData =
                        buildingDatabase.GetBuildingDataFromDatabaseByGuid(building.BuildingGuid);

                    //open the building informatiion panel for the given building
                    buildingInformationPanel.Enable(building, selectedBuildingData, building.transform.position);
                    _previouslySelectedBuilding = building;
                }
            }
            else
            {
                if (buildingInformationPanel.IsInsidePanel()) return;

                _previouslySelectedBuilding = null;
                buildingInformationPanel.Disable();
            }
        }

        public bool IsPreviouslySelectedBuilding(Building building)
        {
            return building.Equals(_previouslySelectedBuilding);
        }
    }
}