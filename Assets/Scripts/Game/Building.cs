using System.Collections.Generic;
using ScriptableObjects.Buildings;
using UnityEngine;

namespace Game
{
    public abstract class Building : MonoBehaviour
    {
        protected GridTile OccupiedTile;

        /// <summary>
        /// Always call base from this method first
        /// </summary>
        /// <param name="tileBuildingGetsPlacedOn"></param>
        public virtual void OnBuild(GridTile tileBuildingGetsPlacedOn)
        {
            OccupiedTile = tileBuildingGetsPlacedOn;
            OccupiedTile.IsOccupied = true;
        }
    }
}