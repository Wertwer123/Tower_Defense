﻿using UnityEngine;

namespace Game
{
    public class BuildingPreview : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;

        public bool IsActive => gameObject.activeSelf;
        public void Init(Sprite buildingSprite)
        {
            spriteRenderer.sprite = buildingSprite;
            gameObject.SetActive(true);
        }

        public void ChangeSpriteState(bool canPlaceBuilding)
        {
            spriteRenderer.color = canPlaceBuilding ? Color.green : Color.red;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
        
        public Vector3 GetPosition()
        {
            return transform.position;
        }
        
        public void Enable()
        {
            gameObject.SetActive(true);
        }
        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}