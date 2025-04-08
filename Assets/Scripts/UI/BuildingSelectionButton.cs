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
        [SerializeField] private Button selectionButton;
        
        private BuildingData _buildingToSelectOnClick = null;
        
        public void Init(BuildingData buildingData)
        {
            _buildingToSelectOnClick = buildingData;
            selectionButton.onClick.AddListener(SelectBuildingOnClick);
            selectionButton.image.sprite = buildingData.UISprite;
        }
        
        void SelectBuildingOnClick()
        {
            BuildingManager.Instance.SetCurrentlySelecetedBuildingToBuild(_buildingToSelectOnClick);
        }
    }
}