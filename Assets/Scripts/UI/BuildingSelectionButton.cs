using Manager;
using ScriptableObjects.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class BuildingSelectionButton : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup costGrid;
        [SerializeField] private Button selectionButton;
        [SerializeField] private ResourceValueDisplay resourceValueDisplayPrefab;
        
        private BuildingData _buildingToSelectOnClick = null;
        
        public void Init(BuildingData buildingData)
        {
            _buildingToSelectOnClick = buildingData;
            selectionButton.onClick.AddListener(SelectBuildingOnClick);
            selectionButton.image.sprite = buildingData.UISprite;

            //initialize the cost grid with according values and images
            InitializeCostGrid();
        }

        void InitializeCostGrid()
        {
            foreach (var resource in _buildingToSelectOnClick.ResourceCosts)
            {
                var resourceValueDisplay = Instantiate(resourceValueDisplayPrefab, costGrid.transform);
                resourceValueDisplay.Init(resource.ResourceType);
                resourceValueDisplay.UpdateDisplayedAmount(resource);
            }           
        }
        
        void SelectBuildingOnClick()
        {
            BuildingManager.Instance.SetCurrentlySelecetedBuildingToBuild(_buildingToSelectOnClick);
        }
    }
}