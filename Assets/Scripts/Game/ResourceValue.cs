using System;
using Game.Enums;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ResourceValue
    {
        [SerializeField, Min(0)] private int resourceValue;
        [SerializeField] private ResourceType resourceType;
        
        public int ResourceVal => resourceValue;
        public ResourceType ResourceType => resourceType;

        public ResourceValue(int resourceValue, ResourceType resourceType)
        {
            this.resourceValue = resourceValue;
            this.resourceType = resourceType;
        }
    }
}