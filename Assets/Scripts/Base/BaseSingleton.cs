using UnityEngine;

namespace Base
{
    public class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (!Application.isPlaying)
                {
                    return null;
                }
                if (instance == null)
                {
                    instance = FindFirstObjectByType(typeof(T)) as T;
                }
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
                
                return instance;
            }
        }
        
        protected virtual void Awake()
        {
            if (instance != this && instance != null)
            {
                Destroy(this);
            }
        }
    }
}