using System.Collections.Generic;
using System.Linq;
using Systems.Movement;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] private PlayerInputScriptableObject input;
        private List<GameObject> _maneuverButtons = new List<GameObject>();

        private float _currentT;
        private Vector2 _currentPos;
        private void Awake()
        {
            input.PlayerInputActions.UI.LeftClick.performed += Select;
            movementController.Path.OnPathChanged += OnOnPathChanged;
        }
        private void OnOnPathChanged()
        {
            foreach (var button in _maneuverButtons.Where(button => button != null))
            {
                Destroy(button);
            }
            _maneuverButtons.Clear();
            RedrawManeuvers();
        }

        private void RedrawManeuvers()
        {

            foreach (var pair in movementController.Path.Maneuvers)
            {
                var id = pair.Key;
                var maneuver = pair.Value;

                var button = Instantiate(buttonPrefab, _currentPos, Quaternion.identity, canvas).GetComponent<WorldSpaceUI>();
                var target = new GameObject("Maneuver Target")
                {
                    transform =
                    {
                        position = movementController.Path.Evaluate(maneuver.startTime),
                        parent = transform
                    }
                };
                button.primaryCamera = cam;
                button.target = target.transform;
                button.GetComponent<ManeuverButton>().maneuverId = id;
                button.GetComponent<ManeuverButton>().Line = movementController.Path;
                _maneuverButtons.Add(button.gameObject);
            }
        }
        private void OnDestroy()
        {
            input.PlayerInputActions.UI.LeftClick.performed -= Select;
            movementController.Path.OnPathChanged -= OnOnPathChanged;
        }
        private void Select(InputAction.CallbackContext obj)
        {
            if (tracer.gameObject.activeSelf)
            {
                var mouseScreenPos = Mouse.current.position.ReadValue();
                var results = new List<RaycastResult>();
                var eventData = new PointerEventData(EventSystem.current)
                {
                    position = mouseScreenPos
                };
                EventSystem.current.RaycastAll(eventData, results);
                if (results.Count == 0)
                {
                    movementController.ScheduleManeuver(new Maneuver
                    {
                        startTime = _currentT,
                        duration = 1,
                        thrust = Vector2.zero
                    });
                    RedrawManeuvers();
                }
            }
        }
        private void Update()
        {
            var mouseScreenPos = Mouse.current.position.ReadValue();
            var mousePos = cam.ScreenToWorldPoint(mouseScreenPos);
            _currentT = movementController.Path.ClosestPointOnPath(mousePos);
            _currentPos = cam.WorldToScreenPoint(movementController.Path.Evaluate(_currentT));
            if (_currentT > movementController.Path.CurrentTime && (mouseScreenPos - _currentPos).magnitude < minMouseDist)
            {
                tracer.position = _currentPos;
                tracer.gameObject.SetActive(true);
                // pause game while planning maneuvers
                Time.timeScale = 0;
            }
            else
            {
                tracer.gameObject.SetActive(false);
                // resume the game, may need to adjust to support different rates
                Time.timeScale = 1;
            }
        }
    }
}
