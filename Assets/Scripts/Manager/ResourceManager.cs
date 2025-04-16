using System;
using Base;
using Game;
using Game.Enums;
using UnityEngine;

namespace Manager
{
    public class ResourceManager : Observable<ResourceValue>
    {
        [SerializeField] private ResourceValue currentGold;
        [SerializeField] private ResourceValue currentStone;
        [SerializeField] private ResourceValue currentWood;
        [SerializeField] private ResourceValue currentMetal;

        private void OnEnable()
        {
            ObservablesManager.Instance.RegisterObservable(typeof(ResourceManager), this);
        }
        
        public void AddResource(ResourceValue resource)
        {
            switch (resource.ResourceType)
            {
                case ResourceType.Stone:
                {
                    currentStone += resource;
                    Notify(currentStone);
                    break;
                }
                case ResourceType.Wood:
                {
                    currentWood += resource;
                    Notify(currentWood);
                    break;
                }
                case ResourceType.Metal:
                {
                    currentMetal += resource;
                    Notify(currentMetal);
                    break;
                }
                case ResourceType.Gold:
                {
                    currentGold += resource;
                    Notify(currentGold);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RemoveResource(ResourceValue resource)
        {
            switch (resource.ResourceType)
            {
                case ResourceType.Stone:
                {
                    currentStone -= resource;
                    Notify(currentStone);
                    break;
                }
                case ResourceType.Wood:
                {
                    currentWood -= resource;
                    Notify(currentWood);
                    break;
                }
                case ResourceType.Metal:
                {
                    currentMetal -= resource;
                    Notify(currentMetal);
                    break;
                }
                case ResourceType.Gold:
                {
                    currentGold -= resource;
                    Notify(currentGold);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
