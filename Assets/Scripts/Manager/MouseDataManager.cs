using System.Collections.Generic;
using Base;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class MouseDataManager : BaseSingleton<MouseDataManager>
    {
        [SerializeField] Vector2 currentMousePositionWorld;
        [SerializeField] List<TdGrid> buildingGrids = new List<TdGrid>();
        [SerializeField] Camera playerCamera;

        private GridTile _currentlyHoveredTile = null;
        
        public GridTile CurrentlyHoveredTile => _currentlyHoveredTile;
        
        public void OnMouseMove(InputAction.CallbackContext context)
        {
            Vector2 mousePosition = context.ReadValue<Vector2>();
            Vector3 mousePositionNearClipPlaneAdded = new Vector3(mousePosition.x, mousePosition.y, playerCamera.nearClipPlane);

            currentMousePositionWorld = playerCamera.ScreenToWorldPoint(mousePositionNearClipPlaneAdded);
            _currentlyHoveredTile = GetCurrentlyHoveredTile(currentMousePositionWorld);
        }
        
        GridTile GetCurrentlyHoveredTile(Vector2 mousePosition)
        {
            GridTile hoveredTile = null;

            foreach (TdGrid buildingGrid in buildingGrids)
            {
                if (!buildingGrid.IsPositionInGrid(mousePosition))
                {
                    continue;
                }
                
                hoveredTile = buildingGrid.GetTileAtPosition(mousePosition);
                break;
            }
            
            return hoveredTile;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            Gizmos.color = Color.red;
            if (_currentlyHoveredTile == null)
            {
                return;
            }
            
            Gizmos.DrawCube(_currentlyHoveredTile.Position, Vector3.one / 2);
        }
    }
}
