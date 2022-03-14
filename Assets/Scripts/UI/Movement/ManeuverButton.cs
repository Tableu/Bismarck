using Systems.Movement;
using UnityEngine;

namespace UI.Movement
{
    [RequireComponent(typeof(WorldSpaceUI))]
    public class ManeuverButton : MonoBehaviour
    {
        public WorldLine Line;
        public int maneuverId;

        private void OnDestroy()
        {
            var c = GetComponent<WorldSpaceUI>();
            Destroy(c.target.gameObject);
        }
    }
}
