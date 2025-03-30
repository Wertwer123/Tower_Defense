using UnityEngine;

namespace Interfaces
{
    /// <summary>
    /// this interface must be added with a EditorApplication.update call
    /// </summary>
    public interface ITransformChanged
    {
        public Transform Self { get;}
        public Vector3 OldPosition { get; set; }

        /// <summary>
        /// Just copy paste this from an allready existing implementation
        /// </summary>
        /// <returns></returns>
        bool HasTransformChanged();
        abstract void OnTransformChanged();
    }
}
