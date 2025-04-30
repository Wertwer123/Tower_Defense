using System.Collections.Generic;
using Base;
using UnityEngine;

namespace Game
{
    public class GridMesh : MonoBehaviour
    {
        [SerializeField] private float lineThickness = 0.1f;
        [SerializeField, Min(10)] private int subsampleLineAmount = 40;
        [SerializeField] float groundOffset = 0.1f;
        [SerializeField] LayerMask layerMask; 
        [SerializeField] private MeshRenderer gridVisualizationMesh;
        [SerializeField] private MeshFilter gridVisualizationMeshFilter;
        
        public void GenerateGridMesh(TdGrid grid)
        {
            if (gridVisualizationMeshFilter == null) return;

            Mesh mesh = new();
            List<Vector3> vertices = new();
            List<int> triangles = new();

            int rows = grid.Rows;
            int columns = grid.Columns;

            //offset the end to the right  to close the grid bounds
            Vector3 columnLinesStart = transform.position;
            Vector3 columnLinesEnd = columnLinesStart + transform.forward * rows * grid.CellSize;

            Vector3 rowLinesStart = transform.position;
            Vector3 rowLinesEnd = rowLinesStart + transform.right * columns * grid.CellSize;

            Vector3 rowLinesCreationStep = (columnLinesEnd - columnLinesStart) / rows;
            Vector3 columnLinesCreationStep = (rowLinesEnd - rowLinesStart) / columns;

            GenerateLines(columns, columnLinesStart, columnLinesEnd, columnLinesCreationStep, vertices, triangles);
            GenerateLines(rows, rowLinesStart, rowLinesEnd, rowLinesCreationStep, vertices, triangles);
            
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            gridVisualizationMeshFilter.mesh = mesh;
        }

        private void GenerateLines(int amount, Vector3 linesStart, Vector3 linesEnd, Vector3 columnLinesCreationStep, List<Vector3> vertices, List<int> triangles)
        {
            Vector3 start = linesStart;
            Vector3 end = linesEnd;
            
            for (int x = 0; x <= amount; x++)
            {
                // Debug.DrawLine(columnLinesStart, columnLinesEnd, Color.green, 1);
                GenerateSubsampledLine(start, end, vertices, triangles);
                start += columnLinesCreationStep;
                end += columnLinesCreationStep;
            }
        }

        private void GenerateSubsampledLine(Vector3 startPoint, Vector3 endPoint, List<Vector3> vertices,
            List<int> triangles)
        {
            int vertexCount = vertices.Count;
            Vector3 direction = (endPoint - startPoint).normalized;
            Vector3 right = Vector3.Cross(direction, Vector3.up);
            Vector3 sampleStep = (endPoint - startPoint) / subsampleLineAmount;
            Vector3 previousSampleStep = startPoint;
            Vector3 currentSampleStep = startPoint + sampleStep;

            for (int i = 0; i < subsampleLineAmount; i++)
            {
                bool hitPrevious = Physics.Raycast(previousSampleStep, Vector3.down,
                    out RaycastHit hitInfoPrevious,
                    Mathf.Infinity, layerMask);
                bool hitCurrent = Physics.Raycast(currentSampleStep, Vector3.down,
                    out RaycastHit hitInfoCurrent,
                    Mathf.Infinity, layerMask);
                if (hitPrevious && hitCurrent)
                {
                    //generate equal to the left and right of the hit points to center the line
                    Vector3 pointCurrent = hitInfoCurrent.point + Vector3.up * groundOffset;
                    Vector3 pointPrevious = hitInfoPrevious.point + Vector3.up * groundOffset;

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
    }
}