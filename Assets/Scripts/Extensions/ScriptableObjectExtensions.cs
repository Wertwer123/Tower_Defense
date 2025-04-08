using UnityEngine;

namespace Extensions
{
    public static class ScriptableObjectExtensions
    {
        public static T GetCopy<T>(this ScriptableObject src) where T : ScriptableObject
        {
            var instance = Object.Instantiate(src);
            src = null;
            return (T)instance;
        }
    }
}