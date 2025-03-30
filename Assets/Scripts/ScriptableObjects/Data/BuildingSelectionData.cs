using System.Collections.Generic;
using ScriptableObjects.Buildings;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "BuildingSelectionData", menuName = "BuildingSelectionData", order = 1)]
    public class BuildingSelectionData : SaveableDataObject<SelectionData>
    {
        public void AddSelectedBuilding(BuildingData building)
        {
            dataObject.selectedBuildings.Add(building.BuildingGuid.ToString());
        }

        public void RemoveSelectedBuilding(BuildingData building)
        {
            dataObject.selectedBuildings.Remove(building.BuildingGuid.ToString());
        }
    }

    [System.Serializable]
    public class SelectionData
    {
        public List<string> selectedBuildings = new List<string>();
    }
}
