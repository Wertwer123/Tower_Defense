using UnityEngine;

namespace Extensions
{
    public static class GizmosExtensions
    {
        public static void DrawArrow(float arrowSize, Vector3 from, Vector3 to, Color color)
        {
            Gizmos.color = color;
            
            Vector3 direction = to - from;
            Vector3 arrowTip = to;
            
            //represents on line going from the tip
            Vector3 arrowWing = -direction.normalized * arrowSize;
            Vector3 arrowWingRotatedLeft = Quaternion.AngleAxis(45, Vector3.forward) * arrowWing;
            Vector3 arrowWingRotatedRight = Quaternion.AngleAxis(-45, Vector3.forward) * arrowWing;
            
            Gizmos.DrawLine(from, arrowTip);
            Gizmos.DrawLine(arrowTip, arrowTip + arrowWingRotatedLeft);
            Gizmos.DrawLine(arrowTip, arrowTip + arrowWingRotatedRight);
        }
    }
}