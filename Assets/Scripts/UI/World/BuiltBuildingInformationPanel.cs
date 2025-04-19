using Game;
using Manager;
using ScriptableObjects.Buildings;
using TMPro;
using UnityEngine;

namespace UI.World
{
    public class BuiltBuildingInformationPanel : BuildingInformationPanel
    {
        [SerializeField] private TextMeshProUGUI healthPercentageText;

        private void OnEnable()
        {
            BlockingUIManager.Instance.OnBlockingUIElementEntered += Disable;
        }

        private void OnDisable()
        {
            BlockingUIManager.Instance.OnBlockingUIElementEntered -= Disable;
        }

        public void Enable(Building clickedBuilding, BuildingData clickedBuildingData, Vector3 clickedBuildingPosition)
        {
            base.Enable(clickedBuildingData, clickedBuildingPosition);
            healthPercentageText.text = "100%";
        }

        public bool IsInsidePanel()
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            return rectTransform.rect.Contains(MouseDataManager.Instance.CurrentMousePositionWorld);
        }
    }
}