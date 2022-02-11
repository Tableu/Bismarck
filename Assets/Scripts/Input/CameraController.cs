using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    /// <summary>
    ///     Manages panning & zooming the camera based on user input
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private PlayerInputScriptableObject input;
        [SerializeField] private Vector2 minBounds;
        [SerializeField] private Vector2 maxBounds;
        [SerializeField] private float minOrthoSize;
        [SerializeField] private float maxOrthoSize;
        [SerializeField] private float zoomSpeed;
        private Camera _camera;
        private Vector3 _camOrigin;
        private Vector2 _dragOrigin;
        private bool _panning = false;
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            input.PlayerInputActions.UI.RightClick.performed += HandlePan;
            input.PlayerInputActions.UI.RightClick.canceled += HandlePan;
            input.PlayerInputActions.UI.Zoom.performed += HandleZoom;

            Debug.Assert(minBounds.x < maxBounds.x, "minBounds.x < maxBounds.x");
            Debug.Assert(minBounds.y < maxBounds.y, "minBounds.y < maxBounds.y");
            Debug.Assert(minOrthoSize < maxOrthoSize, "minOrthoSize < maxOrthoSize");
        }

        private void Update()
        {
            if (_panning)
            {
                Vector3 delta = _dragOrigin - input.PlayerInputActions.UI.Pan.ReadValue<Vector2>();
                var projectionMatrix = _camera.projectionMatrix;
                // Scale movement so mouse world pos does not change
                delta.x /= _camera.pixelWidth * 0.5f * projectionMatrix.m00;
                delta.y /= _camera.pixelHeight * 0.5f * projectionMatrix.m11;
                var pos = _camOrigin + delta;
                pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.y);
                pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
                transform.position = pos;
            }
        }

        private void OnDestroy()
        {
            input.PlayerInputActions.UI.RightClick.performed -= HandlePan;
            input.PlayerInputActions.UI.RightClick.canceled -= HandlePan;
            input.PlayerInputActions.UI.Zoom.performed -= HandleZoom;
        }
        private void HandleZoom(InputAction.CallbackContext context)
        {
            // Save old mouse world pos so we can move the camera such that the mouse world pos stays the same
            var oldPos = _camera.ScreenToWorldPoint(input.PlayerInputActions.UI.Pan.ReadValue<Vector2>());
            // Zoom in by adjusting orthographicSize
            var scroll = context.ReadValue<Vector2>().y / 120f;
            var orthographicSize = _camera.orthographicSize;
            orthographicSize -= scroll * zoomSpeed;
            orthographicSize = Mathf.Clamp(orthographicSize, minOrthoSize, maxOrthoSize);
            _camera.orthographicSize = orthographicSize;
            // Transform the camera to keep the mouse world pos static
            var newPos = _camera.ScreenToWorldPoint(input.PlayerInputActions.UI.Pan.ReadValue<Vector2>());
            _camera.transform.position += oldPos - newPos;
        }

        private void HandlePan(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _panning = true;
                _dragOrigin = input.PlayerInputActions.UI.Pan.ReadValue<Vector2>();
                _camOrigin = transform.position;
            }
            if (context.canceled)
            {
                _panning = false;
            }
        }
    }
}
