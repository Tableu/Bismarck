using UnityEngine;
using UnityEngine.InputSystem;

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
        public GameObject tracer;
        public float semiMajorAxis;
        public float semiMinorAxis;
        public int resolution;
        public Camera cam;

        private LineRenderer _lineRenderer;
        private float _baseMultiply;
        private Spline2D spline;
        
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
            var mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 pointOnLine = spline.ClosestPointOnSpline(mousePos);
            tracer.transform.position = pointOnLine;
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
            const float inc = 0.1f;
            float t = 0;
            var points = new Vector3[resolution];
            spline = new Spline2D(Vector2.zero, Vector2.left, Vector2.zero);
            spline.AddSegment(2, new Vector2(1, 1));
            spline.AddSegment(5, new Vector2(1, 0));
            spline.AddSegment(6, new Vector2(0, 1));

            for (int i = 0; i < resolution; i++)
            {
                t += inc;
                points[i] = spline.Evaluate(t);
            }
            _lineRenderer.positionCount = resolution;
            _lineRenderer.SetPositions(points);
            _lineRenderer.loop = false;
        }
    }
}
