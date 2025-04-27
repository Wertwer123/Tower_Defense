using System.Collections.Generic;
using Game;
using Interfaces;
using PropertyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Base
{
    public class TdGrid : MonoBehaviour, ITransformChanged
    {
        [FormerlySerializedAs("laneCount")] [SerializeField] [OnValueChanged(nameof(RegenerateGrid))] [Min(1)]
        protected int columns = 3;

        [FormerlySerializedAs("rowCount")] [SerializeField] [OnValueChanged(nameof(RegenerateGrid))] [Min(1)]
        protected int rows = 5;

        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))] [Min(0.1f)]
        protected float cellSize = 0.5f;

        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))]
        protected bool canHostBaseBuildings;

        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))]
        protected bool enableDebugLines;

        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))]
        protected float lineThickness = 0.1f;

        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))]
        protected float meshGroundOffset = 0.1f;

        [SerializeField] [OnValueChanged(nameof(RegenerateGrid))]
        protected int subsampleGridLineAmount = 20;


        [SerializeField] private LayerMask terrainLayerMask;
        [SerializeField] private MeshRenderer gridVisualizationMesh;
        [SerializeField] private MeshFilter gridVisualizationMeshFilter;
        [SerializeField] protected BoxCollider gridBounds;

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

        public void SetLineThickness(float newLineThickness)
        {
            lineThickness = newLineThickness;
            GenerateGridMesh();
            Debug.Log("regenerating grid mesh");
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
            GenerateGridMesh();
        }

        protected void GenerateGridMesh()
        {
            if (gridVisualizationMeshFilter == null) return;

            Mesh mesh = new();
            List<Vector3> vertices = new();
            List<int> triangles = new();

            //offset the end to the right  to close the grid bounds
            Vector3 columnLinesStart = transform.position;
            Vector3 columnLinesEnd = columnLinesStart + transform.forward * rows * cellSize;

            Vector3 rowLinesStart = transform.position;
            Vector3 rowLinesEnd = rowLinesStart + transform.right * columns * cellSize;

            Vector3 rowLinesCreationStep = (columnLinesEnd - columnLinesStart) / rows;
            Vector3 columnLinesCreationStep = (rowLinesEnd - rowLinesStart) / columns;

            for (int x = 0; x <= columns; x++)
            {
                // Debug.DrawLine(columnLinesStart, columnLinesEnd, Color.green, 1);
                GenerateSubsampledLine(columnLinesStart, columnLinesEnd, vertices, triangles);
                columnLinesStart += columnLinesCreationStep;
                columnLinesEnd += columnLinesCreationStep;
            }

            for (int y = 0; y <= rows; y++)
            {
                GenerateSubsampledLine(rowLinesStart, rowLinesEnd, vertices, triangles);
                rowLinesStart += rowLinesCreationStep;
                rowLinesEnd += rowLinesCreationStep;
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            gridVisualizationMeshFilter.mesh = mesh;
        }

        private void GenerateSubsampledLine(Vector3 startPoint, Vector3 endPoint, List<Vector3> vertices,
            List<int> triangles)
        {
            int vertexCount = vertices.Count;
            Vector3 direction = (endPoint - startPoint).normalized;
            Vector3 right = Vector3.Cross(direction, Vector3.up);
            Vector3 sampleStep = (endPoint - startPoint) / subsampleGridLineAmount;
            Vector3 previousSampleStep = startPoint;
            Vector3 currentSampleStep = startPoint + sampleStep;

            for (int i = 0; i < subsampleGridLineAmount; i++)
            {
                bool hitPrevious = Physics.Raycast(previousSampleStep, Vector3.down,
                    out RaycastHit hitInfoPrevious,
                    Mathf.Infinity, terrainLayerMask);
                bool hitCurrent = Physics.Raycast(currentSampleStep, Vector3.down,
                    out RaycastHit hitInfoCurrent,
                    Mathf.Infinity, terrainLayerMask);
                if (hitPrevious && hitCurrent)
                {
                    //generate equal to the left and right of the hit points to center the line
                    Vector3 pointCurrent = hitInfoCurrent.point + Vector3.up * meshGroundOffset;
                    Vector3 pointPrevious = hitInfoPrevious.point + Vector3.up * meshGroundOffset;

                    Vector3 vertex1 = pointPrevious - right * lineThickness * 0.5f;
                    Vector3 vertex2 = pointPrevious + right * lineThickness * 0.5f;
                    Vector3 vertex3 = pointCurrent + right * lineThickness * 0.5f;
                    Vector3 vertex4 = pointCurrent - right * lineThickness * 0.5f;

                    vertices.Add(transform.InverseTransformPoint(vertex1));
                    vertices.Add(transform.InverseTransformPoint(vertex2));
                    vertices.Add(transform.InverseTransformPoint(vertex3));
                    vertices.Add(transform.InverseTransformPoint(vertex4));

                    triangles.Add(vertexCount);
                    triangles.Add(vertexCount + 1);
                    triangles.Add(vertexCount + 2);
                    triangles.Add(vertexCount + 2);
                    triangles.Add(vertexCount + 3);
                    triangles.Add(vertexCount);

                    vertexCount += 4;
                }

                previousSampleStep = currentSampleStep;
                currentSampleStep += sampleStep;
            }
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