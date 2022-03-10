using UnityEngine;

namespace Systems.Movement
{
    public class MovementController : MonoBehaviour
    {
        private WorldLine _worldLine = new WorldLine();
        public IDrawablePath Path => _worldLine;

        public void ScheduleManeuver(Maneuver maneuver)
        {
            _worldLine.AddManeuver(maneuver);
        }

        private void Awake()
        {
            _worldLine.AddManeuver(new Maneuver
            {
                duration = 5,
                thrust = new Vector2(1, .1f),
                startTime = 2
            });
            _worldLine.AddManeuver(new Maneuver
            {
                duration = 10,
                thrust = new Vector2(1, -.5f),
                startTime = 10
            });
        }

        private void Update()
        {
            _worldLine.UpdateTime(Time.deltaTime);
            transform.position = _worldLine.Evaluate(_worldLine.CurrentTime);
        }
    }
}
