using System;
using UnityEngine;

namespace Systems.Movement
{
    public class WorldLine : IDrawablePath
    {
        private Spline2D _spline = new Spline2D(Vector2.zero, Vector2.zero, Vector2.zero);
        private float _t0 = 0;
        public float CurrentTime => _t0;

        public void AddManeuver(Maneuver maneuver)
        {
            _spline.AddSegment(maneuver.startTime, maneuver.thrust);
            _spline.AddSegment(maneuver.startTime + maneuver.duration, Vector2.zero);
            OnPathChanged?.Invoke();
        }
        public void UpdateTime(float deltaTime)
        {
            _t0 += deltaTime;
        }

        public bool ClosedPath => false;
        public Vector2 Evaluate(float time)
        {
            return _spline.Evaluate(time);
        }
        public (float start, float end)[] IntervalsInBounds(Bounds bounds)
        {
            return _spline.IntervalsInBounds(bounds);
        }
        public event Action OnPathChanged;
    }
}
