using System;
using System.Collections.Generic;
using Base;
using Game;
using Interfaces;
using PropertyAttributes;
using UnityEngine;
using Utils;
using Color = UnityEngine.Color;

namespace Manager
{
   public class BuildingGridsManager : BaseSingleton<BuildingGridsManager>, ITransformChanged
   {
      [SerializeField] private TdGrid topTdGrid;
      [SerializeField] private TdGrid rightTdGrid;
      [SerializeField] private TdGrid bottomTdGrid;
      [SerializeField] private TdGrid leftTdGrid;
      [SerializeField] private TdGrid centerTdGrid;
      [SerializeField, OnValueChanged(nameof(SetManagedGridPositions)), Min(0)] float laneGridOffset = 0.2f;
      [SerializeField, OnValueChanged(nameof(SetManagedGridCellSizes)),Min(0)] private float cellSizeGrids = 0.5f;
      [SerializeField, OnValueChanged(nameof(SetManagedGridLineThickness)), Min(0)] private float gridLineThickness = 0.5f;
      [SerializeField, OnValueChanged(nameof(SetManagedGridLineAlpha)), Min(0)] private float gridLineAlpha = 0.5f;
      [SerializeField, ColorUsage(true,true)] private Color gridColor = Color.black;
      [SerializeField, OnValueChanged(nameof(SetManagedGridColors))] private bool setColorValue;
      [SerializeField] private MaterialInterface gridMaterial;
      
      public List<Base.TdGrid> grids => new List<TdGrid>(4)
      {
         topTdGrid,
         rightTdGrid,
         bottomTdGrid,
         leftTdGrid,
         centerTdGrid
      };

      private void OnDestroy()
      {
         SetManagedGridColors();
      }

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
      
      void SetManagedGridCellSizes()
      {
         centerTdGrid.SetCellSize(cellSizeGrids);
         topTdGrid.SetCellSize(cellSizeGrids);
         rightTdGrid.SetCellSize(cellSizeGrids);
         bottomTdGrid.SetCellSize(cellSizeGrids);
         leftTdGrid.SetCellSize(cellSizeGrids);
         
         SetManagedGridPositions();
      }
      
      void SetManagedGridColors()
      {
         gridMaterial.SetMaterialColor("_GridLineColor", gridColor);
         setColorValue = true;
      }

      void SetManagedGridLineThickness()
      {
         centerTdGrid.SetLineThickness(gridLineThickness);
         topTdGrid.SetLineThickness(gridLineThickness);
         rightTdGrid.SetLineThickness(gridLineThickness);
         bottomTdGrid.SetLineThickness(gridLineThickness);
         leftTdGrid.SetLineThickness(gridLineThickness);
      }

      void SetManagedGridLineAlpha()
      {
         gridMaterial.SetFloat("_LineAlpha", gridLineAlpha);
      }

      /// <summary>
      /// Reposition all the grids around the center grid
      /// </summary>
      /// <param name="centerGridBounds"></param>
      void SetManagedGridPositions()
      {
         centerTdGrid.transform.position = transform.position;
         Rect centerGridBounds = centerTdGrid.GridBounds;
         
         Vector2 topGridPosition = (centerGridBounds.min + Vector2.up * centerGridBounds.height) + Vector2.up * laneGridOffset;
         Vector2 leftGridPosition = (centerTdGrid.GridBounds.min + Vector2.left * leftTdGrid.GridBounds.width) + Vector2.left * laneGridOffset;
         Vector2 bottomGridPosition = (centerGridBounds.position + Vector2.down * bottomTdGrid.GridBounds.height) + Vector2.down * laneGridOffset;
         Vector2 rightGridPosition = (centerGridBounds.position + Vector2.right * centerGridBounds.width) + Vector2.right * laneGridOffset;

         topTdGrid.transform.position = topGridPosition;
         rightTdGrid.transform.position = rightGridPosition;
         bottomTdGrid.transform.position = bottomGridPosition;
         leftTdGrid.transform.position = leftGridPosition;
      }
      
      public void OnTransformChanged()
      {
         SetManagedGridPositions();
      }
      
   }
}
