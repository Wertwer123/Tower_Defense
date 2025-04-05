using System;
using Game;
using Game.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class ResourceValueDisplay : MonoBehaviour
    {
        [SerializeField] ResourceType resourceTypeToDisplay;
        [SerializeField] private Image displayImage;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private Sprite goldSprite;
        [SerializeField] private Sprite stoneSprite;
        [SerializeField] private Sprite woodSprite;
        [SerializeField] private Sprite metalSprite;
        public ResourceType DisplayedResourceType{get => resourceTypeToDisplay; set => resourceTypeToDisplay = value;}
        
        public void Init(ResourceType resourceValue)
        {
            resourceTypeToDisplay = resourceValue;
            costText.text = "0";
            
            switch (resourceTypeToDisplay)
            {
                case ResourceType.Stone:
                {
                    displayImage.sprite = goldSprite;
                    break;
                }
                case ResourceType.Wood:
                {
                    displayImage.sprite = woodSprite;
                    break;
                }
                case ResourceType.Metal:
                {
                    displayImage.sprite = metalSprite;
                    break;
                }
                case ResourceType.Gold:
                {
                    displayImage.sprite = goldSprite;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void UpdateDisplayedAmount(ResourceValue resourceValue)
        {
            costText.text = resourceValue.ResourceVal.ToString();
        }
    }
}