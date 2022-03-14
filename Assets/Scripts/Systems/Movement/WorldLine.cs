using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems.Movement
{
    public class WorldLine : IDrawablePath
    {
        private int _currentManeuverId;
        private readonly Dictionary<int, Maneuver> _maneuvers = new Dictionary<int, Maneuver>();
        private Spline2D _spline = new Spline2D(Vector2.zero, Vector2.zero, Vector2.zero);
        public float CurrentTime { get; private set; } = 0;

        public IReadOnlyDictionary<int, Maneuver> Maneuvers => _maneuvers;
        public bool ClosedPath => false;
        public Vector2 Evaluate(float time) => _spline.Evaluate(time);
        public Vector2 EvaluateVelocity(float t) => _spline.EvaluateVelocity(t);
        public (float start, float end)[] IntervalsInBounds(Bounds bounds) => _spline.IntervalsInBounds(bounds);
        public float ClosestPointOnPath(Vector2 p) => _spline.ClosestPointOnSpline(p);

        public event Action OnPathChanged;

        public int AddManeuver(Maneuver maneuver)
        {
            _currentManeuverId++;
            _maneuvers.Add(_currentManeuverId, maneuver);
            RegeneratePath();
            return _currentManeuverId;
        }
        public void EditManeuver(int id, Maneuver maneuver)
        {
            _maneuvers[id] = maneuver;
            RegeneratePath();
        }
        public void RemoveManeuver(int id)
        {
            _maneuvers.Remove(id);
            RegeneratePath();
        }

        public void UpdateTime(float deltaTime)
        {
            CurrentTime += deltaTime;
            if (CurrentTime > 1e3)
            {
                // todo: discard/store very old data
                _spline.ResetZeroTime(CurrentTime);
                foreach (var key in _maneuvers.Keys)
                {
                    var maneuver = _maneuvers[key];
                    maneuver.startTime -= CurrentTime;
                    _maneuvers[key] = maneuver;
                }
                CurrentTime = 0;
            }
        }

        private void RegeneratePath()
        {
            _spline = new Spline2D(Vector2.zero, Vector2.zero, Vector2.zero);
            var values = _maneuvers.Values.ToList();

            values.Sort((x, y) => (int)Math.Round(x.startTime - y.startTime, MidpointRounding.AwayFromZero));

            for (int i = 0; i < values.Count; i++)
            {
                var maneuver = values[i];
                _spline.AddSegment(maneuver.startTime, maneuver.thrust);
                if (i != values.Count - 1 && values[i + 1].startTime <= values[i].startTime + values[i].duration)
                {
                    var value = values[i];
                    value.duration = values[i + 1].startTime - values[i].startTime;
                    values[i] = value;
                    continue;
                }
                _spline.AddSegment(maneuver.startTime + maneuver.duration, Vector2.zero);
            }
            OnPathChanged?.Invoke();
        }
    }
}
