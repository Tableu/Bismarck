using UnityEngine;

namespace UI.ManeuverPlanning
{
    public class ManeuverGizmo : MonoBehaviour
    {
        [SerializeField] private RectTransform arrow;
        [SerializeField] private float maxThrust = 1;
        private float _baseHeight;
        private void Awake()
        {
            _baseHeight = arrow.rect.height;
        }

        public void UpdateDir(float angle)
        {
            arrow.transform.rotation = Quaternion.Euler(0, 0, (angle - 0.5f) * 360f);
        }

        public void UpdateMag(float mag)
        {
            var size = new Vector2
            {
                x = arrow.rect.width,
                y = _baseHeight * (1 + 0.1f * (2 * mag - 0.5f))
            };
            arrow.sizeDelta = size;
        }
    }
}
