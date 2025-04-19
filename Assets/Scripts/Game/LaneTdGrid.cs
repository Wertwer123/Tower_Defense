using System;
using System.Collections.Generic;
using Base;
using Extensions;
using UnityEngine;

namespace Game
{
    public class LaneTdGrid : TdGrid
    {
        [SerializeField] private LaneDirection laneDirection = LaneDirection.LeftToRight;
        [SerializeField] private List<Lane> _lanes = new();


        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (!enableDebugLines) return;

            foreach (Lane lane in _lanes)
                GizmosExtensions.DrawArrow(0.5f, lane.LastTile.CellCenter, lane.FirstTile.Position, Color.red);
        }

        protected override void RegenerateGrid()
        {
            base.RegenerateGrid();
            CreateLanes();
        }

        private void CreateLanes()
        {
            _lanes.Clear();

            switch (laneDirection)
            {
                case LaneDirection.LeftToRight:
                {
                    for (int y = 0; y < rowCount; y++)
                    {
                        List<GridTile> laneTiles = new();

                        for (int x = laneCount - 1; x >= 0; x--) laneTiles.Add(allTiles[GetGridIndex(x, y)]);

                        _lanes.Add(new Lane(laneTiles));
                    }

                    break;
                }
                case LaneDirection.RightToLeft:
                {
                    for (int y = 0; y < rowCount; y++)
                    {
                        List<GridTile> laneTiles = new();

                        for (int x = 0; x < laneCount; x++) laneTiles.Add(allTiles[GetGridIndex(x, y)]);

                        _lanes.Add(new Lane(laneTiles));
                    }

                    break;
                }
                case LaneDirection.TopToBottom:
                {
                    for (int x = 0; x < laneCount; x++)
                    {
                        List<GridTile> laneTiles = new();

                        for (int y = 0; y < rowCount; y++) laneTiles.Add(allTiles[GetGridIndex(x, y)]);

                        _lanes.Add(new Lane(laneTiles));
                    }

                    break;
                }
                case LaneDirection.BottomToTop:
                {
                    for (int x = 0; x < laneCount; x++)
                    {
                        List<GridTile> laneTiles = new();

                        for (int y = rowCount - 1; y >= 0; y--) laneTiles.Add(allTiles[GetGridIndex(x, y)]);

                        _lanes.Add(new Lane(laneTiles));
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum LaneDirection
        {
            LeftToRight,
            RightToLeft,
            TopToBottom,
            BottomToTop
        }
    }
}