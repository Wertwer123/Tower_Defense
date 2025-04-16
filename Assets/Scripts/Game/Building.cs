using System;
using System.Collections.Generic;
using Extensions;
using ScriptableObjects.Buildings;
using SpriteAnimation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Object = System.Object;

namespace Game
{
    public abstract class Building : MonoBehaviour
    {
        [SerializeField] SpriteRenderer buildingSpriteRenderer;
        [SerializeField] private SpriteAnimTemplate onBuildAnimation;
        [SerializeField] Material hoveredMaterial;
        
        private GridTile _occupiedTile;
        private Material _buildingMaterial;
        
        public Sprite BuildingSprite => buildingSpriteRenderer.sprite;
        
        /// <summary>
        /// Always call base from this method first
        /// </summary>
        /// <param name="tileBuildingGetsPlacedOn"></param>
        public virtual void OnBuild(GridTile tileBuildingGetsPlacedOn)
        {
            _buildingMaterial = buildingSpriteRenderer.material;
            _occupiedTile = tileBuildingGetsPlacedOn;
            _occupiedTile.IsOccupied = true;
            onBuildAnimation.GetCopy<SpriteAnimTemplate>().PlayAnimation(this);
        }

        private void OnMouseEnter()
        {
            List<Material> hoveredMaterials = new(){_buildingMaterial, Instantiate(hoveredMaterial)};
            buildingSpriteRenderer.SetSharedMaterials(hoveredMaterials);
            
            Debug.Log("OnPointerEnter");
        }

        private void OnMouseExit()
        {
            List<Material> defaultMaterials = new(){_buildingMaterial};
            buildingSpriteRenderer.SetSharedMaterials(defaultMaterials);
            Debug.Log("OnPointerEnter");
        }
    }
}