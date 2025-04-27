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

        public static Color GetClosestColorToPixelColor(Color color, List<ColorPair> colorPairs)
        {
            Color closestColor = Color.clear;

            float smallestDiff = float.MaxValue;

            for (int i = 0; i < colorPairs.Count; i++)
            {
                ColorPair miniMapColor = colorPairs[i];
                Color keyColor = miniMapColor.colorKey;

                float rDiff = Mathf.Pow(color.r - keyColor.r, 2);
                float gDiff = Mathf.Pow(color.g - keyColor.g, 2);
                float bDiff = Mathf.Pow(color.b - keyColor.b, 2);

                float wholeDiff = Mathf.Sqrt(rDiff + gDiff + bDiff);

                if (wholeDiff < smallestDiff)
                {
                    smallestDiff = wholeDiff;
                    closestColor = miniMapColor.colorValue;
                }
            }

            return closestColor;
        }
    }
}