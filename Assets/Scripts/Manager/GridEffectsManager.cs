using System.Collections.Generic;
using Base;
using UnityEngine;
using Grid = Base.Grid;

namespace Manager
{
    public class GridEffectsManager : BaseSingleton<GridEffectsManager>
    {
        [SerializeField] private List<Grid> BuildingGrids = new List<Grid>();
        [SerializeField] Material gridGlobalMaterial;

        public void SetGridColor(Color color)
        {
            gridGlobalMaterial.SetColor("_GridLineColor", color);
        }
    }
}