using Player;
using UnityEngine;

namespace Prefabs.UI
{
    public class ScaleOnZoom : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float minScale = 0.2f;
        [SerializeField] [Min(0.0f)] private float maxScale = 1.0f;
        [SerializeField] private CameraController cameraController;

        private void Start()
        {
            cameraController.OnZoom += OnZoom;
        }

        private void OnDestroy()
        {
            cameraController.OnZoom -= OnZoom;
        }

        private void OnZoom(float scrollPercentage)
        {
            Vector3 minScaleVec3 = new(minScale, minScale, minScale);
            Vector3 maxScaleVec3 = new(maxScale, maxScale, maxScale);

            transform.localScale = Vector3.Lerp(minScaleVec3, maxScaleVec3, scrollPercentage);
        }
    }
}