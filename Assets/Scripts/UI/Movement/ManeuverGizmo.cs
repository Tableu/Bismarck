using System;
using UnityEngine;

namespace UI.Movement
{
    public class ManeuverGizmo : MonoBehaviour
    {
        [SerializeField] private RectTransform arrow;
        private float _baseHeight;
        [NonSerialized] public ManeuverButton ManeuverButton;
        public Systems.Movement.MovementController movement;
        private float _mag = 1;
        private float _angle = 0;
        private void Awake()
        {
            _baseHeight = arrow.rect.height;
        }

        public void UpdateDir(float angle)
        {
            _angle = (angle - 0.5f) * 360f;
            arrow.transform.rotation = Quaternion.Euler(0, 0, _angle - 90);
            if (ManeuverButton != null)
            {
                movement.EditManeuver(ManeuverButton.maneuverId, new Vector2(_mag * Mathf.Cos(Mathf.Deg2Rad * _angle), _mag * Mathf.Sin(Mathf.Deg2Rad * _angle)));
            }
        }

        public void UpdateMag(float mag)
        {
            _mag = 10 * mag;
            var size = new Vector2
            {
                x = arrow.rect.width,
                y = _baseHeight * (0.9f * mag + 0.1f)
            };
            if (ManeuverButton != null)
            {
                movement.EditManeuver(ManeuverButton.maneuverId, new Vector2(_mag * Mathf.Cos(Mathf.Deg2Rad * _angle), _mag * Mathf.Sin(Mathf.Deg2Rad * _angle)));
            }
            arrow.sizeDelta = size;
        }
    }
}
