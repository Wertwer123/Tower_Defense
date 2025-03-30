using System;
using System.Collections.Generic;
using Base;
using Interfaces;
using PropertyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Color = UnityEngine.Color;
using Grid = Base.Grid;

namespace Manager
{
   public class BuildingGridsManager : BaseSingleton<BuildingGridsManager>, ITransformChanged
   {
      [SerializeField] private Grid topGrid;
      [SerializeField] private Grid rightGrid;
      [SerializeField] private Grid bottomGrid;
      [SerializeField] private Grid leftGrid;
      [SerializeField] private Grid centerGrid;
      [SerializeField, OnValueChanged(nameof(SetManagedGridPositions)), Min(0)] float laneGridOffset = 0.2f;
      [SerializeField, OnValueChanged(nameof(SetManagedGridCellSizes)),Min(0)] private float cellSizeGrids = 0.5f;
      [SerializeField, OnValueChanged(nameof(SetManagedGridLineThickness)), Min(0)] private float gridLineThickness = 0.5f;
      [SerializeField, OnValueChanged(nameof(SetManagedGridLineAlpha)), Min(0)] private float gridLineAlpha = 0.5f;
      [SerializeField, ColorUsage(true,true)] private Color gridColor = Color.black;
      [SerializeField, OnValueChanged(nameof(SetManagedGridColors))] private bool setColorValue;
      [SerializeField] private MaterialInterface gridMaterial;
      
      public List<Base.Grid> grids => new List<Grid>(4)
      {
         topGrid,
         rightGrid,
         bottomGrid,
         leftGrid,
         centerGrid
      };
      
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
         centerGrid.SetCellSize(cellSizeGrids);
         topGrid.SetCellSize(cellSizeGrids);
         rightGrid.SetCellSize(cellSizeGrids);
         bottomGrid.SetCellSize(cellSizeGrids);
         leftGrid.SetCellSize(cellSizeGrids);
         
         SetManagedGridPositions();
      }
      
      void SetManagedGridColors()
      {
         gridMaterial.SetMaterialColor("_GridLineColor", gridColor);
         setColorValue = true;
      }

      void SetManagedGridLineThickness()
      {
         centerGrid.SetLineThickness(gridLineThickness);
         topGrid.SetLineThickness(gridLineThickness);
         rightGrid.SetLineThickness(gridLineThickness);
         bottomGrid.SetLineThickness(gridLineThickness);
         leftGrid.SetLineThickness(gridLineThickness);
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
         centerGrid.transform.position = transform.position;
         Rect centerGridBounds = centerGrid.GridBounds;
         
         Vector2 topGridPosition = (centerGridBounds.min + Vector2.up * centerGridBounds.height) + Vector2.up * laneGridOffset;
         Vector2 leftGridPosition = (centerGrid.GridBounds.min + Vector2.left * leftGrid.GridBounds.width) + Vector2.left * laneGridOffset;
         Vector2 bottomGridPosition = (centerGridBounds.position + Vector2.down * bottomGrid.GridBounds.height) + Vector2.down * laneGridOffset;
         Vector2 rightGridPosition = (centerGridBounds.position + Vector2.right * centerGridBounds.width) + Vector2.right * laneGridOffset;

         topGrid.transform.position = topGridPosition;
         rightGrid.transform.position = rightGridPosition;
         bottomGrid.transform.position = bottomGridPosition;
         leftGrid.transform.position = leftGridPosition;
      }
      
      public void OnTransformChanged()
      {
         SetManagedGridPositions();
      }
      
   }
}
