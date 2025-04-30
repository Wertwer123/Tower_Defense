using System.Collections.Generic;
using Base;
using Interfaces;
using PropertyAttributes;
using UnityEngine;
using Utils;
using Color = UnityEngine.Color;

namespace Manager
{
    public class BuildingGridsManager : BaseSingleton<BuildingGridsManager>, ITransformChanged
    {
        [SerializeField] private TdGrid topTdGrid;
        [SerializeField] private TdGrid rightTdGrid;
        [SerializeField] private TdGrid bottomTdGrid;
        [SerializeField] private TdGrid leftTdGrid;
        [SerializeField] private TdGrid centerTdGrid;

        [SerializeField] [OnValueChanged(nameof(SetManagedGridPositions))] [Min(0)]
        private float laneGridOffset = 0.2f;

        [SerializeField] [OnValueChanged(nameof(SetManagedGridCellSizes))] [Min(0)]
        private float cellSizeGrids = 0.5f;

        [SerializeField] [OnValueChanged(nameof(SetManagedGridLineAlpha))] [Min(0)]
        private float gridLineAlpha = 0.5f;

        [SerializeField] [ColorUsage(true, true)]
        private Color gridColor = Color.black;

        [SerializeField] [OnValueChanged(nameof(SetManagedGridColors))]
        private bool setColorValue;

        [SerializeField] private MaterialInterface gridMaterial;

        public List<TdGrid> grids => new(4)
        {
            topTdGrid,
            rightTdGrid,
            bottomTdGrid,
            leftTdGrid,
            centerTdGrid
        };

        private void OnDestroy()
        {
            SetManagedGridColors();
        }

        public Transform Self => transform;
        public Vector3 OldPosition { get; set; }

        public bool HasTransformChanged()
        {
            if (Self.position != OldPosition)
            {
                OldPosition = Self.position;
                return true;
            }

            return false;
        }

        public void OnTransformChanged()
        {
            SetManagedGridPositions();
        }

        private void SetManagedGridCellSizes()
        {
            centerTdGrid.SetCellSize(cellSizeGrids);
            topTdGrid.SetCellSize(cellSizeGrids);
            rightTdGrid.SetCellSize(cellSizeGrids);
            bottomTdGrid.SetCellSize(cellSizeGrids);
            leftTdGrid.SetCellSize(cellSizeGrids);

            SetManagedGridPositions();
        }

        private void SetManagedGridColors()
        {
            gridMaterial.SetMaterialColor("_GridLineColor", gridColor);
            setColorValue = true;
        }

        private void SetManagedGridLineAlpha()
        {
            gridMaterial.SetFloat("_LineAlpha", gridLineAlpha);
        }

        /// <summary>
        ///     Reposition all the grids around the center grid
        /// </summary>
        /// <param name="centerGridBounds"></param>
        private void SetManagedGridPositions()
        {
            // centerTdGrid.transform.position = transform.position;
            // Bounds centerGridBounds = centerTdGrid.GridBounds;
            //
            // Vector2 topGridPosition = centerGridBounds.min + Vector3.forward * centerGridBounds.extents.y +
            //                           Vector3.forward * laneGridOffset;
            // Vector2 leftGridPosition = centerTdGrid.GridBounds.min + Vector3.left * leftTdGrid.GridBounds.extents.y +
            //                            Vector3.left * laneGridOffset;
            // Vector2 bottomGridPosition = centerGridBounds.min - Vector3.forward * bottomTdGrid.GridBounds.extents.z -
            //                              Vector3.forward * laneGridOffset;
            // Vector2 rightGridPosition = centerGridBounds.min + Vector3.right * centerGridBounds.extents.x +
            //                             Vector3.right * laneGridOffset;
            //
            // topTdGrid.transform.position = topGridPosition;
            // rightTdGrid.transform.position = rightGridPosition;
            // bottomTdGrid.transform.position = bottomGridPosition;
            // leftTdGrid.transform.position = leftGridPosition;
        }
    }
}