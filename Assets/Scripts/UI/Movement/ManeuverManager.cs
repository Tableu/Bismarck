using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Movement
{
    public class ManeuverManager : MonoBehaviour
    {
        [SerializeField] private Systems.Movement.MovementController movementController;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private RectTransform canvas;
        [SerializeField] private Camera cam;
        [SerializeField] private RectTransform tracer;
        [SerializeField] private float minMouseDist = 50;

        private void Update()
        {
            var mouseScreenPos = Mouse.current.position.ReadValue();
            var mousePos = cam.ScreenToWorldPoint(mouseScreenPos);
            var pointOnLine = movementController.Path.ClosestPointOnPath(mousePos);
            Vector2 screenPoint = cam.WorldToScreenPoint(movementController.Path.Evaluate(pointOnLine));
            if ((mouseScreenPos - screenPoint).magnitude < minMouseDist)
            {
                tracer.position = screenPoint;
                tracer.gameObject.SetActive(true);
            }
            else
            {
                tracer.gameObject.SetActive(false);
            }
        }
        private void AddManeuverButton(Vector2 pos)
        {
            var button = Instantiate(buttonPrefab, pos, Quaternion.identity, canvas).GetComponent<WorldSpaceUI>();
            var target = new GameObject("Maneuver Target")
            {
                transform =
                {
                    position = pos,
                    parent = transform
                }
            };
            button.primaryCamera = cam;
            button.target = target.transform;
        }

    }
}
