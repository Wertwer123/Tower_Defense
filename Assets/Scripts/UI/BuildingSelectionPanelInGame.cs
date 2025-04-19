using System;
using System.Collections.Generic;
using Game;
using Interfaces;
using Manager;
using ScriptableObjects.Buildings;
using ScriptableObjects.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    ///     Represents the panel in game from where towers and buildings etc can be selected
    /// </summary>
    public class BuildingSelectionPanelInGame : MonoBehaviour, IBlockingUIElement, IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] private BuildingSelectionData selectedBuildings;
        [SerializeField] private BuildingDatabase buildingDataBase;
        [SerializeField] private GridLayoutGroup buildingGridLayout;
        [SerializeField] private BuildingSelectionButton selectionButtonPrefab;
        [SerializeField] private List<BuildingData> availableBuildings = new();

        private readonly List<BuildingSelectionButton> _buttonInstances = new();

        private void Start()
        {
            CreateBuildingSelectionDisplay();
            BuildingManager.Instance.OnBuildingBuilt += UpdateBuildingSelectionDisplay;
            BlockingUIManager.Instance.AddBlockingUIElement(this);
        }

        public event Action OnBlockingUIElementEntered;
        public event Action OnBlockingUIElementExited;
        public event Action OnBlockingUIElementDestroyed;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnBlockingUIElementEntered?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnBlockingUIElementExited?.Invoke();
        }

        public void CreateBuildingSelectionDisplay()
        {
            foreach (string buildingId in selectedBuildings.DataObject.selectedBuildings)
            {
                BuildingData buildingData = buildingDataBase.GetBuildingDataFromDatabaseByGuid(new GUID(buildingId));
                availableBuildings.Add(buildingData);

                BuildingSelectionButton buildingButton =
                    Instantiate(selectionButtonPrefab, buildingGridLayout.transform);
                buildingButton.Init(buildingData);

                _buttonInstances.Add(buildingButton);
            }
        }


        private void UpdateBuildingSelectionDisplay(Building _)
        {
        }
    }
}