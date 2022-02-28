using UnityEngine;

namespace SystemMap
{
    /// <summary>
    ///     A script that will rescale sprites so that they always appear the same size on screen, regardless of camera zoom.
    /// </summary>
    /// <remarks>
    ///     Useful for creating map icons that should remain visible for all zoom levels
    /// </remarks>
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapIcon : MonoBehaviour
    {
        // todo: make shader
        public Camera cam;

        private Vector3 baseScale;
        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (cam != null)
            {
                Matrix4x4 mat = cam.projectionMatrix;
                baseScale = transform.localScale * mat.m11 * 0.5f;
                Rescale();
            }
        }
        private void LateUpdate() => Rescale();

        private void Rescale()
        {
            Matrix4x4 mat = cam.projectionMatrix;
            var t = transform;
            var scale = baseScale;
            scale /= mat.m11 * 0.5f;
            t.localScale = scale;
        }
    }
}
