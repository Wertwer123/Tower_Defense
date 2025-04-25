using System.Collections.Generic;
using Extensions;
using SpriteAnimation;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public abstract class Building : MonoBehaviour
    {
        [SerializeField] private MeshRenderer buildingMeshRenderer;
        [SerializeField] private MeshFilter buildingMesh;

        [SerializeField] private SpriteAnimTemplate onBuildAnimation;
        [SerializeField] private Material hoveredMaterial;

        private Material _buildingMaterial;
        private GridTile _occupiedTile;

        public GUID BuildingGuid { get; private set; }

        public Mesh BuildingMesh => buildingMesh.sharedMesh;

        private void OnMouseEnter()
        {
            List<Material> hoveredMaterials = new() { _buildingMaterial, Instantiate(hoveredMaterial) };
            buildingMeshRenderer.SetSharedMaterials(hoveredMaterials);
        }

        private void OnMouseExit()
        {
            List<Material> defaultMaterials = new() { _buildingMaterial };
            buildingMeshRenderer.SetSharedMaterials(defaultMaterials);
        }

        /// <summary>
        ///     Always call base from this method first
        /// </summary>
        /// <param name="tileBuildingGetsPlacedOn"></param>
        /// <param name="buildingGuid"></param>
        public virtual void OnBuild(GridTile tileBuildingGetsPlacedOn, GUID buildingGuid)
        {
            BuildingGuid = buildingGuid;
            _buildingMaterial = buildingMeshRenderer.material;
            _occupiedTile = tileBuildingGetsPlacedOn;
            _occupiedTile.IsOccupied = true;
            onBuildAnimation.GetCopy<SpriteAnimTemplate>().PlayAnimation(this);
        }
    }
}