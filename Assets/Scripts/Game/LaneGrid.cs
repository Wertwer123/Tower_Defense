using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using PropertyAttributes;
using Extensions;
using Grid = Base.Grid;

namespace Game
{
   public class LaneGrid : Grid
   {
      enum LaneDirection
      {
         LeftToRight,
         RightToLeft,
         TopToBottom,
         BottomToTop
      }
      
      [SerializeField] LaneDirection laneDirection = LaneDirection.LeftToRight;
      [SerializeField] private List<Lane> _lanes = new();
      
      protected override void RegenerateGrid()
      {
         base.RegenerateGrid();
         CreateLanes();
      }
      
      void CreateLanes()
      {
         _lanes.Clear();
         
         switch (laneDirection)
         {
            case LaneDirection.LeftToRight:
            {
               for (int y = 0; y < rowCount; y++)
               {
                  List<GridTile> laneTiles = new List<GridTile>();
                  
                  for (int x = laneCount - 1; x >= 0; x--)
                  {
                     laneTiles.Add(allTiles[GetGridIndex(x, y)]);   
                  }
                  
                  _lanes.Add(new Lane(laneTiles));
               }
               break;
            }
            case LaneDirection.RightToLeft:
            {
               for (int y = 0; y < rowCount; y++)
               {
                  List<GridTile> laneTiles = new List<GridTile>();
                  
                  for (int x = 0; x < laneCount; x++)
                  {
                     laneTiles.Add(allTiles[GetGridIndex(x, y)]);   
                  }
                  
                  _lanes.Add(new Lane(laneTiles));
               }
               break;
            }
            case LaneDirection.TopToBottom:
            {
               for (int x = 0; x < laneCount; x++)
               {
                  List<GridTile> laneTiles = new List<GridTile>();
                  
                  for (int y = 0; y < rowCount; y++)
                  {
                     laneTiles.Add(allTiles[GetGridIndex(x, y)]);   
                  }
                  
                  _lanes.Add(new Lane(laneTiles));
               }
               break;
            }
            case LaneDirection.BottomToTop:
            {
               for (int x = 0; x < laneCount; x++)
               {
                  List<GridTile> laneTiles = new List<GridTile>();
                  
                  for (int y = rowCount - 1; y >= 0; y--)
                  {
                     laneTiles.Add(allTiles[GetGridIndex(x, y)]);   
                  }
                  
                  _lanes.Add(new Lane(laneTiles));
               }
               break;
            }
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
      
      

      protected override void OnDrawGizmos()
      {
         base.OnDrawGizmos();

         if (!enableDebugLines)
         {
            return;
         }
         
         foreach (Lane lane in _lanes)
         {
            GizmosExtensions.DrawArrow(0.5f ,lane.LastTile.Position, lane.FirstTile.Position, Color.red);
         }
      }
   }
}
