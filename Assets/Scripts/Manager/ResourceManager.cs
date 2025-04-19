using System;
using System.Collections.Generic;
using Game;
using Game.Enums;
using ScriptableObjects.Buildings;
using UnityEngine;

namespace Manager
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private ResourceValue currentGold;
        [SerializeField] private ResourceValue currentStone;
        [SerializeField] private ResourceValue currentWood;
        [SerializeField] private ResourceValue currentMetal;

        public event Action<ResourceValue> OnResourceChanged;
        
        private void Start()
        {
            OnResourceChanged?.Invoke(currentGold);
            OnResourceChanged?.Invoke(currentStone);
            OnResourceChanged?.Invoke(currentWood);
            OnResourceChanged?.Invoke(currentMetal);
        }

        public bool HasEnoughResourcesForBuilding(BuildingData buildingData)
        {
            foreach (var resourceValue in buildingData.ResourceCosts)
            {
                ResourceValue availableResource = GetResource(resourceValue.ResourceType);
                
                if (availableResource.ResourceVal < resourceValue.ResourceVal)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        public void AddResource(ResourceValue resource)
        {
            ResourceValue availableResource = GetResource(resource.ResourceType);
            availableResource.ResourceVal += resource.ResourceVal;
            OnResourceChanged?.Invoke(availableResource);
        }

        public void AddResources(List<ResourceValue> resources)
        {
            foreach (var resourceValue in resources)
            {
                ResourceValue availableResource = GetResource(resourceValue.ResourceType);
                availableResource.ResourceVal += resourceValue.ResourceVal;
                OnResourceChanged?.Invoke(availableResource);
            }
        }

        public void RemoveResource(ResourceValue resource)
        {
            ResourceValue availableResource = GetResource(resource.ResourceType);
            availableResource.ResourceVal -= resource.ResourceVal;
            OnResourceChanged?.Invoke(availableResource);
        }

        public void RemoveResources(List<ResourceValue> resources)
        {
            foreach (var resource in resources)
            {
                ResourceValue availableResource = GetResource(resource.ResourceType);
                availableResource.ResourceVal -= resource.ResourceVal;
                OnResourceChanged?.Invoke(availableResource);
            }
        }

        ResourceValue GetResource(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Stone:
                {
                    return currentStone;
                }
                case ResourceType.Wood:
                {
                    return currentWood;
                }
                case ResourceType.Metal:
                {
                    return currentMetal;
                }
                case ResourceType.Gold:
                {
                    return currentGold;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
            }
        }

        
    }
}
