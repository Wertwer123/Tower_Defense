using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Lane
    {
        [SerializeField] private List<GridTile> tiles;
        
        public GridTile LastTile => tiles.Last();
        public GridTile FirstTile => tiles.First();

        public Lane(List<GridTile> tiles)
        {
            this.tiles = tiles;
        }
    }
}