using System.Collections.Generic;
using Base;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class MouseDataManager : BaseSingleton<MouseDataManager>
    {
        [SerializeField] private Vector2 currentMousePositionWorld;
        [SerializeField] private List<TdGrid> buildingGrids = new();
        [SerializeField] private Camera playerCamera;

        private Vector2 _currentMousePositionScreen;

        public GridTile CurrentlyHoveredTile { get; private set; }
        public Vector2 CurrentMousePositionScreen => _currentMousePositionScreen;
        public Vector2 CurrentMousePositionWorld => currentMousePositionWorld;

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            Gizmos.color = Color.red;
            if (CurrentlyHoveredTile == null) return;

            Gizmos.DrawCube(CurrentlyHoveredTile.CellCenter, Vector3.one / 2);
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            _currentMousePositionScreen = context.ReadValue<Vector2>();
            Vector3 mousePositionNearClipPlaneAdded =
                new(_currentMousePositionScreen.x, _currentMousePositionScreen.y, playerCamera.nearClipPlane);

            currentMousePositionWorld = playerCamera.ScreenToWorldPoint(mousePositionNearClipPlaneAdded);
            CurrentlyHoveredTile = GetCurrentlyHoveredTile(currentMousePositionWorld);
        }

        private GridTile GetCurrentlyHoveredTile(Vector2 mousePosition)
        {
            GridTile hoveredTile = null;

            foreach (TdGrid buildingGrid in buildingGrids)
            {
                if (!buildingGrid.IsPositionInGrid(mousePosition)) continue;

                hoveredTile = buildingGrid.GetTileAtPosition(mousePosition);
                break;
            }

            return hoveredTile;
        }
    }
}