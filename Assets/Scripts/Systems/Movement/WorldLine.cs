using UnityEngine;

namespace Systems.Movement
{
    public class WorldLine
    {
        private Spline2D _spline = new Spline2D(Vector2.zero, Vector2.zero, Vector2.zero);
        private float _t0 = 0;

        public void AddManeuver(Maneuver maneuver)
        {
            _spline.AddSegment(maneuver.startTime, maneuver.thrust);
            _spline.AddSegment(maneuver.startTime + maneuver.duration, Vector2.zero);
        }
        public void UpdateTime(float deltaTime)
        {
            _t0 += deltaTime;
        }

        public Vector2 Evaluate(float time = 0)
        {
            return _spline.Evaluate(time + _t0);
        }
    }
}
