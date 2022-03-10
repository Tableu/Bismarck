using UnityEngine;

namespace UI
{

    /// <summary>
    ///     Binds a UI element to a world space transform
    /// </summary>
    public class WorldSpaceUI : MonoBehaviour
    {
        public Transform target;
        [SerializeField] public Camera primaryCamera;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            var screenPoint = primaryCamera.WorldToScreenPoint(target.position);
            _rectTransform.position = screenPoint;
        }
    }
}
