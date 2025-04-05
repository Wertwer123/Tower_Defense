using System;
using System.Collections.Generic;
using Base;
using UnityEngine;

namespace Manager
{
    public class GridEffectsManager : BaseSingleton<GridEffectsManager>
    {
        [SerializeField] private List<TdGrid> BuildingGrids = new List<TdGrid>();
        [SerializeField] Material gridGlobalMaterial;
        
        private static readonly int LineAlpha = Shader.PropertyToID("_LineAlpha");
        private static readonly int GridLineColor = Shader.PropertyToID("_GridLineColor");

        private void Start()
        {
            SetGridAlpha(0.0f);
        }

        public void SetGridColor(Color color)
        {
            gridGlobalMaterial.SetColor(GridLineColor, color);
        }

        public void SetGridAlpha(float alpha)
        {
            gridGlobalMaterial.SetFloat(LineAlpha, alpha);
        }
    }
}