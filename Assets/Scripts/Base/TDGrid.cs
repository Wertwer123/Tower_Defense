using System.Collections.Generic;
using Game;
using Interfaces;
using PropertyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Base
{
    [RequireComponent(typeof(GridMesh))]
    public class TdGrid : MonoBehaviour, ITransformChanged
    {
        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))] [Min(1)]
        protected int columns = 3;
        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))] [Min(1)]
        protected int rows = 5;

        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))] [Min(0.1f)]
        protected float cellSize = 0.5f;

        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))]
        protected bool canHostBaseBuildings;

        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))]
        protected bool enableDebugLines;

        [SerializeField] private LayerMask terrainLayerMask;
        
        [SerializeField] protected BoxCollider gridBounds;
        [SerializeField] GridMesh gridMesh;
        
        [SerializeField] [HideInInspector] private Vector3 oldPosition;
        [SerializeField] [HideInInspector] protected float cellExtents;

        protected GridTile[,] AllTiles;


        private void Start()
        {
            CreateGrid();
        }

        protected virtual void OnDrawGizmos()
        {
            if (!enableDebugLines) return;

            Handles.color = Color.yellow;
            if (AllTiles == null) return;

            foreach (GridTile tile in AllTiles)
            {
                Handles.DrawWireDisc(tile.CellCenter, Vector3.up, cellExtents);
                Handles.color = Color.red;
                Handles.DrawWireDisc(tile.Position, Vector3.up, 0.1f);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(gridBounds.center, gridBounds.size);
        }

        public Transform Self => transform;

        public Vector3 OldPosition
        {
            get => oldPosition;
            set => oldPosition = value;
        }
        
        public int Columns => columns;
        public int Rows => rows;
        public float CellSize => cellSize;

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
            RegenerateGrid();
        }

        public void SetCellSize(float newCellSize)
        {
            cellSize = newCellSize;
        }
        public bool IsPositionInGrid(Vector2 position)
        {
            return gridBounds.bounds.Contains(position);
        }

        public GridTile GetTileAtPosition(Vector3 position)
        {
            int xOffset = Mathf.FloorToInt((position.x - transform.position.x) / cellSize);
            int yOffset = Mathf.FloorToInt((position.z - transform.position.z) / cellSize);

            if (xOffset < 0 || yOffset < 0 || xOffset >= columns || yOffset >= rows)
                return null;

            return AllTiles[xOffset, yOffset];
        }

        protected virtual void RegenerateGrid()
        {
            CreateGrid();
            CalculateGridBounds();
            gridMesh?.GenerateGridMesh(this);
        }

        private void CalculateGridBounds()
        {
            //collider bounds are relative
            Vector3 gridBoundsSize = new(columns * cellSize, 25, rows * cellSize);
            Vector3 gridCenter = new(columns * cellSize * 0.5f, -gridBoundsSize.y / 2, rows * cellSize * 0.5f);

            gridBounds.center = gridCenter;
            gridBounds.size = gridBoundsSize;
        }

        private void CreateGrid()
        {
            AllTiles = new GridTile[columns, rows];

            cellExtents = cellSize * 0.5f;

            Vector3 cellCenterOffset = transform.right * cellExtents + transform.forward * cellExtents;

            for (int x = 0; x < columns; x++)
            for (int y = 0; y < rows; y++)
            {
                //Pivot is on the lower left
                Vector3 cellOrigin = transform.position + transform.right * cellSize * x +
                                     transform.forward * cellSize * y;
                Physics.Raycast(
                    cellOrigin + cellCenterOffset,
                    Vector3.down,
                    out RaycastHit hitInfo, Mathf.Infinity, terrainLayerMask);

                GridTile newGridTile = new(new Vector2(x, y), canHostBaseBuildings, cellOrigin,
                    hitInfo.point);

                AllTiles[x, y] = newGridTile;
            }
        }
    }
}