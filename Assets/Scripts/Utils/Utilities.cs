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
    }
}