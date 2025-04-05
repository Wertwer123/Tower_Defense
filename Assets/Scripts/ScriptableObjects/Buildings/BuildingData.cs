using System;
using System.Collections.Generic;
using Game;
using Game.Enums;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Buildings
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/BuildingData", order = 1)]
    public class BuildingData : ScriptableObject
    {
        [SerializeField, Min(1)] private int level;
        [SerializeField] private string buildingName;
        [SerializeField, TextArea]private string description;
        [SerializeField] private bool isBaseBuilding;
        [SerializeField] private List<ResourceValue> resourceCosts = new List<ResourceValue>()
        {
            new ResourceValue(0, ResourceType.Gold),    
            new ResourceValue(0, ResourceType.Wood),   
            new ResourceValue(0, ResourceType.Stone)    
        };
        
        [SerializeField] private Building buildingPrefab;
        [SerializeField] private Sprite uISprite;
        [SerializeField] private string buildingGuid = string.Empty;
        
        public int Level => level;
        public string Description => description;
        public string BuildingName => buildingName;
        public bool IsBaseBuilding => isBaseBuilding;
        public List<ResourceValue> ResourceCosts => resourceCosts;
        public ResourceValue Cost(ResourceType resourceType) => resourceCosts.Find(resource => resource.ResourceType == resourceType);
        public Building BuildingPrefab => buildingPrefab;
        public Sprite UISprite => uISprite;
        public GUID BuildingGuid => new GUID(buildingGuid);

        private void OnEnable()
        {
            if (buildingGuid == string.Empty)
            {
                buildingGuid = GUID.Generate().ToString();
            }
        }
    }
}