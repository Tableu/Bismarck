using Systems.Movement;
using UnityEngine;

namespace UI.Movement
{
    [RequireComponent(typeof(WorldSpaceUI))]
    public class ManeuverButton : MonoBehaviour
    {
        public WorldLine Line;
        public int maneuverId;
        public ManeuverGizmo gizmo;

        public void OnClick()
        {
            gizmo.gameObject.SetActive(true);
            gizmo.ManeuverButton = this;
        }

        private void OnDestroy()
        {
            var c = GetComponent<WorldSpaceUI>();
            if (c != null && c.target != null)
            {
                Destroy(c.target.gameObject);
            }
            if (gizmo != null && gizmo.ManeuverButton == this)
            {
                gizmo.ManeuverButton = null;
            }
        }
    }
}
