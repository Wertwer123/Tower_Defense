using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    public sealed class CameraController : MonoBehaviour
    {
        //Capable of edge paning camera shakes focusing a viewpoint
        //zooming
        [SerializeField] [Min(0.0f)] private float cameraSpeed = 5f;
        [SerializeField] [Min(0.0f)] private float cameraZoomSpeed = 5f;
        [SerializeField] [Min(0.1f)] private float minZoom = 1;
        [SerializeField] [Min(0.0f)] private float maxZoom = 5;
        [SerializeField] [Range(0, 100)] private int edgePanThreshold = 50;
        [SerializeField] private Camera playerCamera;
        private Vector2 _currentMovementVector = Vector2.zero;

        private float _currentZoom;

        private void Start()
        {
            Utilities.LockMouseCursor();
            _currentZoom = playerCamera.orthographicSize;
        }

        public void Update()
        {
            MoveCamera();
            ZoomCamera();
        }

        /// <summary>
        ///     Takes in the percentage of zoom as a parameter
        /// </summary>
        public event Action<float> OnZoom;

        private void MoveCamera()
        {
            transform.Translate(_currentMovementVector);
        }

        private void ZoomCamera()
        {
            _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);
            playerCamera.orthographicSize = Mathf.Lerp(playerCamera.orthographicSize, _currentZoom,
                cameraZoomSpeed * Time.deltaTime);

            OnZoom?.Invoke(playerCamera.orthographicSize / maxZoom);
        }

        public void Zoom(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 scrollValue = context.ReadValue<Vector2>();
                int zoomValue = scrollValue.y > 0 ? -1 : 1;
                _currentZoom += zoomValue;
            }
        }

        public void EdgePan(InputAction.CallbackContext context)
        {
            Vector2 mousePos = context.ReadValue<Vector2>();

            float mouseX = mousePos.x;
            float mouseY = mousePos.y;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            //If the mouse it out of screenbounds
            if (mouseX > screenWidth - edgePanThreshold || mouseX < 0 + edgePanThreshold
                                                        || mouseY > screenHeight - edgePanThreshold ||
                                                        mouseY < 0 + edgePanThreshold)
            {
                //calculate the direction from center screen pos to mouse pos
                Vector2 screenCenter = new(screenWidth * 0.5f, screenHeight * 0.5f);
                Vector3 directionToMouse = mousePos - screenCenter;

                _currentMovementVector = directionToMouse.normalized * cameraSpeed * Time.deltaTime;
            }
            else
            {
                _currentMovementVector = Vector2.zero;
            }
        }
    }
}