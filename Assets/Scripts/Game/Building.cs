using System;
using System.Collections.Generic;
using ScriptableObjects.Buildings;
using SpriteAnimation;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

namespace Game
{
    public abstract class Building : MonoBehaviour
    {
        [SerializeField] SpriteRenderer buildingSpriteRenderer;
        [SerializeField] private SpriteAnimTemplate onBuildAnimation;
        
        private GridTile _occupiedTile;
        public Sprite BuildingSprite => buildingSpriteRenderer.sprite;
        
        /// <summary>
        /// Always call base from this method first
        /// </summary>
        /// <param name="tileBuildingGetsPlacedOn"></param>
        public virtual void OnBuild(GridTile tileBuildingGetsPlacedOn)
        {
            _occupiedTile = tileBuildingGetsPlacedOn;
            _occupiedTile.IsOccupied = true;
            var animInstanceToPlay = Instantiate(onBuildAnimation);
            animInstanceToPlay.PlayAnimation(this);
        }
    }
}