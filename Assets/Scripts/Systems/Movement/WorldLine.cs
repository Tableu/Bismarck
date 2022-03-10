using System;
using UnityEngine;

namespace Systems.Movement
{
    public class WorldLine : IDrawablePath
    {
        private Spline2D _spline = new Spline2D(Vector2.zero, Vector2.zero, Vector2.zero);
        public float CurrentTime { get; private set; } = 0;

        public bool ClosedPath => false;
        public Vector2 Evaluate(float time) => _spline.Evaluate(time);
        public (float start, float end)[] IntervalsInBounds(Bounds bounds) => _spline.IntervalsInBounds(bounds);
        public float ClosestPointOnPath(Vector2 p) => _spline.ClosestPointOnSpline(p);

        public event Action OnPathChanged;

        public void AddManeuver(Maneuver maneuver)
        {
            _spline.AddSegment(maneuver.startTime, maneuver.thrust);
            _spline.AddSegment(maneuver.startTime + maneuver.duration, Vector2.zero);
            OnPathChanged?.Invoke();
        }
        public void UpdateTime(float deltaTime)
        {
            CurrentTime += deltaTime;
            if (CurrentTime > 1e3)
            {
                _spline.ResetZeroTime(CurrentTime);
                CurrentTime = 0;
            }
        }
    }
}
