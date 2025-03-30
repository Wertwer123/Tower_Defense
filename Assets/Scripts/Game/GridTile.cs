using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [Serializable]
    public class GridTile
    {
        [SerializeField] private int tileIndex;
        [SerializeField] private bool isOccupied;
        [SerializeField] private bool canHostBaseBuildings = false;
        [SerializeField] private Vector2 position;

        public int TileIndex => tileIndex;
        public bool CanHostBaseBuildings => canHostBaseBuildings;

        public bool IsOccupied
        {
            get => isOccupied;
            set => isOccupied = value;
        }

        public Vector2 Position => position;
        
        public GridTile(int tileIndex, bool canHostBaseBuildings, Vector2 position)
        {
            this.tileIndex = tileIndex;
            this.isOccupied = false;
            this.position = position;
            this.canHostBaseBuildings = canHostBaseBuildings;
        }
    }
}
