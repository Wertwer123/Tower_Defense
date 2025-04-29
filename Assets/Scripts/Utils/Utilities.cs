using System.Collections.Generic;
using Game.Player.Camera;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

namespace Utils
{
    public static class Utilities
    {
        public static void LockMouseCursor()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        public static Vector3 GetPositionOnCircleByAngle(float angle, float radius, Vector3 centerOfCircle)
        {
            float adjustedAngle = -angle + 90; // Negate for clockwise, add 90 to shift 0 degrees to the top

            // Calculate the position on the circle
            float x = centerOfCircle.x + radius * Mathf.Cos(Mathf.Deg2Rad * adjustedAngle);
            float y = centerOfCircle.y + radius * Mathf.Sin(Mathf.Deg2Rad * adjustedAngle);

            return new Vector3(x, y, 0);
        }
        
        /// <summary>
        /// This returns the normalized distance between colors as the colors in unity are already normalized lol
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        public static float GetDistanceBetweenColors(Color color1, Color color2)
        {
            float rDiff = Mathf.Pow(color1.r - color2.r, 2);
            float gDiff = Mathf.Pow(color1.g - color2.g, 2);
            float bDiff = Mathf.Pow(color1.b - color2.b, 2);
            
            return Mathf.Sqrt(rDiff + gDiff + bDiff);
        }

        public static ColorPair GetClosestColorToPixelColor(Color color, List<ColorPair> colorPairs)
        {
            ColorPair closestColor = null;

            float smallestDiff = float.MaxValue;

            for (int i = 0; i < colorPairs.Count; i++)
            {
                ColorPair miniMapColor = colorPairs[i];
                Color keyColor = miniMapColor.colorKey;

                float distance = GetDistanceBetweenColors(color, keyColor);

                if (distance < smallestDiff)
                {
                    smallestDiff = distance;
                    closestColor = miniMapColor;
                }
            }

            return closestColor;
        }
    }
}