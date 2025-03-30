using System;
using System.Collections.Generic;
using Game;
using Manager;
using ScriptableObjects.Buildings;
using ScriptableObjects.Data;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents the panel in game from where towers and buildings etc can be selected
/// </summary>
public class BuildingSelectionPanelInGame : MonoBehaviour
{
    [SerializeField] private BuildingSelectionData selectedBuildings;
    [SerializeField] private List<BuildingData> availableBuildings = new List<BuildingData>();
    [SerializeField] private BuildingDatabase buildingDataBase;
    [SerializeField] private GridLayoutGroup buildingGridLayout;
    [SerializeField] private BuildingSelectionButton selectionButtonPrefab;
    
    List<BuildingSelectionButton> buttonInstances = new List<BuildingSelectionButton>();
    
    //TODO creat UI grid layout for selected buildings and dynamiccally fill it with the selected buildings
    //TODO also implement some kind of resource manager probably wouldnt be that bad to have this information as well 

    private void Start()
    {
        CreateBuildingSelectionDisplay();
        BuildingManager.Instance.OnBuildingBuilt += UpdateBuildingSelectionDisplay;
    }

    public void CreateBuildingSelectionDisplay()
    {
        foreach (string buildingId in selectedBuildings.DataObject.selectedBuildings)
        {
            BuildingData buildingData = buildingDataBase.GetBuildingFromDatabaseByGuid(new GUID(buildingId));
            availableBuildings.Add(buildingData);

            var buildingButton = Instantiate(selectionButtonPrefab, buildingGridLayout.transform);
            buildingButton.Init(buildingData);
            
            buttonInstances.Add(buildingButton);
        }
    }
    
    
    
    void UpdateBuildingSelectionDisplay(Building _)
    {
        
    }
}
