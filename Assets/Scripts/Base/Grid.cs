using System.Collections.Generic;
using Extensions;
using Game;
using Interfaces;
using PropertyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Base
{
    public class Grid : MonoBehaviour, ITransformChanged
    {
        [SerializeField, OnValueChanged(nameof(RegenerateGrid)), Min(1)] protected int laneCount = 3;
        [SerializeField, OnValueChanged(nameof(RegenerateGrid)), Min(1)] protected int rowCount = 5;
        [SerializeField, OnValueChanged(nameof(RegenerateGrid)), Min(0.1f), ] protected float cellSize = 0.5f;
        [SerializeField, OnValueChanged(nameof(RegenerateGrid))] protected bool canHostBaseBuildings = false;
        [SerializeField, OnValueChanged(nameof(RegenerateGrid))] protected bool enableDebugLines = false;
        [SerializeField, OnValueChanged(nameof(RegenerateGrid))] protected float lineThickness = 0.1f;
        [SerializeField] private MeshRenderer gridVisualizationMesh;
        [SerializeField] private MeshFilter gridVisualizationMeshFilter;
        
        [SerializeField, HideInInspector] float cellExtents;
        [SerializeField, HideInInspector] private Vector3 oldPosition;
        [SerializeField, HideInInspector] protected Rect gridBounds;
        [SerializeField, HideInInspector] protected List<GridTile> allTiles = new List<GridTile>();
    
        public Transform Self => transform;
        public Vector3 OldPosition { get => oldPosition; set => oldPosition = value; }
        public Rect GridBounds => gridBounds;

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
        
        public bool IsPositionInGrid(Vector2 position)
        {
            return gridBounds.Contains(position);
        }
        public GridTile GetTileAtPosition(Vector2 position)
        {
            int xOffset = Mathf.FloorToInt((position.x - transform.position.x) / cellSize);
            int yOffset = Mathf.FloorToInt((position.y - transform.position.y) / cellSize);

            if (xOffset < 0 || yOffset < 0 || xOffset >= laneCount || yOffset >= allTiles.Count / laneCount)
            {
                return null;
            }
         
            return  allTiles[GetGridIndex(xOffset, yOffset)];
        }

        protected int GetGridIndex(int x, int y)
        {
            return y * laneCount + x;
        }
        
        protected virtual void RegenerateGrid()
        {
            CreateGrid();
            CalculateGridBounds();
            GenerateGridMesh();
        }
        
        protected virtual void OnDrawGizmos()
        {
            if (!enableDebugLines)
            {
                return;
            }
            
            Gizmos.color = Color.yellow;
            foreach (var tile in allTiles)
            {
                Gizmos.DrawWireCube(tile.Position, new Vector3(cellSize, cellSize, 0));
            }
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(gridBounds.center, gridBounds.size);
        }

        protected void GenerateGridMesh()
        {
            if (gridVisualizationMeshFilter == null)
            {
                return;
            }
            
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new();
            List<int> triangles = new();
            
            //offset the end to the right  to close the grid bounds
            Vector2 horizontalLinesStart = Vector2.zero;
            Vector2 horizontalLinesEnd = (Vector2.right *  laneCount * cellSize) + (Vector2.right * lineThickness);
            
            Vector2 verticalLinesStart = Vector2.zero;
            Vector2 verticalLinesEnd = Vector3.up *  rowCount * cellSize;
            
            CreateGridLines(true,rowCount + 1, vertices, triangles, horizontalLinesStart, horizontalLinesEnd, Vector2.up);
            CreateGridLines(false,laneCount + 1, vertices, triangles, verticalLinesStart, verticalLinesEnd, Vector2.right);
            
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            
            gridVisualizationMeshFilter.mesh = mesh;
        }

        private void CreateGridLines(
            bool reverseTriangleOrder,
            int numLines,
            List<Vector3> vertices,
            List<int> triangles,
            Vector2 linesStart, 
            Vector2 linesEnd,
            Vector2 lineCreationDirection)
        {
            for (int i = 0; i < numLines; i++)
            {
                int verticeCount = vertices.Count;
                
                triangles.Add(verticeCount + 0);
                triangles.Add(verticeCount + (reverseTriangleOrder ? 3 : 1));
                triangles.Add(verticeCount + 2);
                triangles.Add(verticeCount + 2);
                triangles.Add(verticeCount + (reverseTriangleOrder ? 1 : 3));
                triangles.Add(verticeCount + 0);
                
                //For testing insert a rectangle from start to end 
                vertices.Add(linesStart);  
                vertices.Add(linesEnd);
                vertices.Add(linesEnd + lineCreationDirection * lineThickness);
                vertices.Add(linesStart + lineCreationDirection * lineThickness);
                
                linesStart += lineCreationDirection * cellSize;
                linesEnd += lineCreationDirection * cellSize;
                Debug.Log(linesStart);
                Debug.Log(linesEnd);
            }
        }

        private void CalculateGridBounds()
        {
            Vector3 gridExtents = new Vector3(laneCount * cellSize, rowCount * cellSize);
            Vector3 gridCenter = transform.position + new Vector3(laneCount * cellSize * 0.5f, rowCount * cellSize * 0.5f, 0);
        
            gridBounds.center = gridCenter;
            gridBounds.width = gridExtents.x;
            gridBounds.height = gridExtents.y;
        }
    
        private void CreateGrid()
        {
            allTiles.Clear();
         
            int gridSize = rowCount * laneCount;
            cellExtents = cellSize * 0.5f;
         
            //shift half a cell from the transform position to make the origin the lower left corner
            Vector2 gridOrigin = new Vector2(
                transform.position.x + cellExtents,
                transform.position.y + cellExtents);
         
            Vector2 laneTilePosition = gridOrigin;
        
            for (int i = 1; i < gridSize + 1; i++)
            {
                //if we reached the end of one lane switch up by one row
                GridTile newGridTile = new GridTile(i - 1, canHostBaseBuildings, laneTilePosition);
                
                allTiles.Add(newGridTile);
            
                laneTilePosition.x += cellSize;
                
                if (i % laneCount == 0)
                {
                    laneTilePosition.y += cellSize;
                    laneTilePosition.x = transform.position.x + cellExtents;
                }
            }
        }
    }
}
