using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GridTile
    {
        [SerializeField] private bool isOccupied;
        [SerializeField] private bool canHostBaseBuildings;
        [SerializeField] private Vector2 tileCoordinates;
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 cellCenter;

        public GridTile(Vector2 tileCoordinates, bool canHostBaseBuildings, Vector3 position, Vector3 cellCenter)
        {
            this.tileCoordinates = tileCoordinates;
            isOccupied = false;
            this.position = position;
            this.cellCenter = cellCenter;
            this.canHostBaseBuildings = canHostBaseBuildings;
        }


        public Vector2 TileCoordinates => tileCoordinates;
        public bool CanHostBaseBuildings => canHostBaseBuildings;

        public bool IsOccupied
        {
            get => isOccupied;
            set => isOccupied = value;
        }

        public Vector3 Position => position;
        public Vector3 CellCenter => cellCenter;
    }
}