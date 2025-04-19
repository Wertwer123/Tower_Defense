using System;
using Game.Enums;
using Manager;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ResourceValue
    {
        [SerializeField, Min(0)] private int resourceValue;
        [SerializeField] private ResourceType resourceType;

        public int ResourceVal
        {
            get => resourceValue;
            set => resourceValue = value;
        }

        public ResourceType ResourceType => resourceType;

        public ResourceValue(int resourceValue, ResourceType resourceType)
        {
            this.resourceValue = resourceValue;
            this.resourceType = resourceType;
        }
        
        public static ResourceValue operator+(ResourceValue a, ResourceValue b)
        {
            if (a.ResourceType != b.ResourceType)
            {
                throw new ArgumentException("Resource type mismatch");
            }
            
            return new ResourceValue(a.resourceValue + b.resourceValue, a.ResourceType);
        }
        
        public static ResourceValue operator-(ResourceValue a, ResourceValue b)
        {
            if (a.ResourceType != b.ResourceType)
            {
                throw new ArgumentException("Resource type mismatch");
            }
            
            return new ResourceValue(a.resourceValue - b.resourceValue, a.ResourceType);
        }
    }
}