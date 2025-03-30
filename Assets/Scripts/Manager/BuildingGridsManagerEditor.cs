using System.Drawing;
using Interfaces;
using UnityEngine;
using Grid = Base.Grid;

namespace Manager
{
   public class BuildingGridsPositionManager : MonoBehaviour, ITransformChanged
   {
      [SerializeField] private Grid topGrid;
      [SerializeField] private Grid rightGrid;
      [SerializeField] private Grid bottomGrid;
      [SerializeField] private Grid leftGrid;
      [SerializeField] private Grid centerGrid;
   
      public Transform Self => transform;
      public Vector3 OldPosition { get; set; }
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
         //Reposition all the gridsaround the center grid
         centerGrid.transform.position = transform.position;
         Rect centerGridBounds = centerGrid.GridBounds;
         
         Vector2 topGridPosition = centerGridBounds.min + Vector2.up * centerGridBounds.height;
         Vector2 leftGridPosition = centerGrid.GridBounds.min + Vector2.left * leftGrid.GridBounds.width;
         Vector2 bottomGridPosition = centerGridBounds.position + Vector2.down * bottomGrid.GridBounds.height;
         Vector2 rightGridPosition = centerGridBounds.position + Vector2.right * centerGridBounds.width;

         topGrid.transform.position = topGridPosition;
         rightGrid.transform.position = rightGridPosition;
         bottomGrid.transform.position = bottomGridPosition;
         leftGrid.transform.position = leftGridPosition;
      }
   }
}
