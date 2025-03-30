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
    }
}