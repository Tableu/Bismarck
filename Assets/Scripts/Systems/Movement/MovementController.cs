using UnityEngine;

namespace Systems.Movement
{
    public class MovementController : MonoBehaviour
    {
        public IDrawablePath DrawablePath { get; private set; }

        private WorldLine _worldLine = new WorldLine();

        private void Awake()
        {
            _worldLine.AddManeuver(new Maneuver
            {
                duration = 5,
                thrust = new Vector2(10, 1),
                startTime = 2
            });
            _worldLine.AddManeuver(new Maneuver
            {
                duration = 10,
                thrust = new Vector2(10, -5),
                startTime = 10
            });
        }

        private void Update()
        {
            _worldLine.UpdateTime(Time.deltaTime);
            transform.position = _worldLine.Evaluate();
        }
    }
}
