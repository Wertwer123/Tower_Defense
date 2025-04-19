using ScriptableObjects.Buildings;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBases", menuName = "DataBases/BuildingDataBase", order = 1)]
public class BuildingDatabase : DataBase<BuildingData>
{
    public BuildingData GetBuildingDataFromDatabaseByGuid(GUID buildingGuid)
    {
        return Find(building => building.BuildingGuid == buildingGuid);
    }
}