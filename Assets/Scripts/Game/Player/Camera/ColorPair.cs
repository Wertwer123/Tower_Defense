using System;
using UnityEngine;

namespace Game.Player.Camera
{
    [Serializable]
    public class ColorPair
    {
        public Color colorKey;
        public Color colorValue;


        public ColorPair(Color colorKey, Color colorValue)
        {
            this.colorKey = colorKey;
            this.colorValue = colorValue;
        }
    }
}