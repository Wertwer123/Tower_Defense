using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GridTile
    {
        [SerializeField] private int tileIndex;
        [SerializeField] private bool isOccupied;
        [SerializeField] private bool canHostBaseBuildings;
        [SerializeField] private Vector2 position;
        [SerializeField] private Vector2 cellCenter;

        public GridTile(int tileIndex, bool canHostBaseBuildings, Vector2 position, Vector2 cellCenter)
        {
            this.tileIndex = tileIndex;
            isOccupied = false;
            this.position = position;
            this.cellCenter = cellCenter;
            this.canHostBaseBuildings = canHostBaseBuildings;
        }


        public int TileIndex => tileIndex;
        public bool CanHostBaseBuildings => canHostBaseBuildings;

        public bool IsOccupied
        {
            get => isOccupied;
            set => isOccupied = value;
        }

        public Vector2 Position => position;
        public Vector2 CellCenter => cellCenter;
    }
}