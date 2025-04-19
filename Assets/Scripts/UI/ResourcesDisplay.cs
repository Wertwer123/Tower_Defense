using System.Collections.Generic;
using Game;
using Manager;
using Game.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Displays all kinds of resources in a vertical layout
    /// </summary>
    public class ResourcesDisplay : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup resourcesLayoutGroup;
        [SerializeField] private ResourceValueDisplay resourceDisplayPrefab;
        [SerializeField] private List<ResourceType> resourcesToDisplay;
        [SerializeField] private ResourceManager resourceManager;

        private readonly Dictionary<ResourceType, ResourceValueDisplay> _resourcesDisplays = new();

        //TODO Implement the prefab in unity and connect the resource manager to resource production buildings also then implement the logic in the building manager if you have sufficient resources 
        public void Start()
        {
            resourceManager.OnResourceChanged += OnResourceChanged;

            foreach (var resourceType in resourcesToDisplay)
            {
                ResourceValueDisplay resourceDisplay = Instantiate(resourceDisplayPrefab, resourcesLayoutGroup.transform);
                resourceDisplay.Init(resourceType);

                _resourcesDisplays.TryAdd(resourceType, resourceDisplay);
            }
        }

        void OnResourceChanged(ResourceValue observedValue)
        {
            if (_resourcesDisplays.TryGetValue(observedValue.ResourceType, out ResourceValueDisplay resourceDisplay))
            {
                resourceDisplay.UpdateDisplayedAmount(observedValue);
            }
        }
    }
}
