using System.Collections.Generic;
using Systems.Movement;
using UnityEngine;

namespace UI.Movement
{
    [RequireComponent(typeof(LineRenderer))]
    public class PathDrawer : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float segmentSize;
        private Bounds _currentBounds;
        private bool _dirty = true;
        private LineRenderer _lineRenderer;
        private IDrawablePath _path;
        private float _baseMultiply;


        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _path = GetComponent<Systems.Movement.MovementController>().Path;
            Matrix4x4 mat = cam.projectionMatrix;
            _baseMultiply = mat.m11;
        }

        private void LateUpdate()
        {
            Matrix4x4 mat = cam.projectionMatrix;
            _lineRenderer.widthMultiplier = _baseMultiply / mat.m11;
            var bounds = new Bounds
            {
                min = cam.ViewportToWorldPoint(new Vector3(-1f, -1f, -1)),
                max = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 1))
            };

            var tmp = bounds.min;
            tmp.z = -10;
            bounds.min = tmp;
            tmp = bounds.max;
            tmp.z = 10;
            bounds.max = tmp;

            if (!_currentBounds.Contains(bounds.min) || !_currentBounds.Contains(bounds.max) || _dirty)
            {
                bounds.min *= 1.1f;
                bounds.max *= 1.1f;
                _currentBounds = bounds;
                _dirty = false;
                DrawPath();
            }
        }

        private void DrawPath()
        {
            var points = new List<Vector3>();

            foreach (var interval in _path.IntervalsInBounds(_currentBounds))
            {
                float t = interval.start;
                while (t < interval.end)
                {
                    points.Add(_path.Evaluate(t));
                    t += segmentSize;
                }
            }
            _lineRenderer.positionCount = points.Count;
            _lineRenderer.SetPositions(points.ToArray());
        }
    }
}
