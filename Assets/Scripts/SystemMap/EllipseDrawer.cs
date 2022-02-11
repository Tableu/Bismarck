using UnityEngine;

namespace SystemMap
{
    /// <summary>
    ///     Draws an ellipse using a line renderer. Line width is rescaled so as to always have the same width on screen
    ///     regardless of zoom.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class EllipseDrawer: MonoBehaviour
    {
        // todo: make shader
        public float semiMajorAxis;
        public float semiMinorAxis;
        public int resolution;
        public Camera cam;

        private LineRenderer _lineRenderer;
        private float _baseMultiply;
        
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            Redraw();
            Matrix4x4 mat = cam.projectionMatrix;
            _baseMultiply = mat.m11;
        }

        private void Update()
        {
            Matrix4x4 mat = cam.projectionMatrix;
            _lineRenderer.widthMultiplier = _baseMultiply / mat.m11;
        }
        private void OnValidate()
        {
            if (resolution > 0)
            {
                _lineRenderer = GetComponent<LineRenderer>();
                Redraw();
            }
        }


        private void Redraw()
        {
            var inc = 2f * Mathf.PI / resolution;
            var points = new Vector3[resolution];

            for (int i = 0; i < resolution; i++)
            {
                points[i] = new Vector3(
                    semiMinorAxis*Mathf.Cos(inc*i), 
                    semiMajorAxis*Mathf.Sin(inc*i),
                    0);
            }
            _lineRenderer.positionCount = resolution;
            _lineRenderer.SetPositions(points);
            _lineRenderer.loop = true;
        }
    }
}
