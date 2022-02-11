using System;
using Ships.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ships.Fleets
{
    public class PlayerFleetController : MonoBehaviour
    {
        [SerializeField] private Camera activeCamera;

        private PlayerInput _input;
        private InputAction _select;
        private InputAction _pointerPos;
        private InputAction _cancel;

        public static event Action<ShipInfo> OnSelect;
        public ShipInfo SelectedShip { get; private set; }

        private void Awake()
        {
            _input = FindObjectOfType<PlayerInput>();
            Debug.Assert(_input != null, "Couldn't find PlayerInput in scene");
            _select = _input.actions["PrimarySelect"];
            _pointerPos = _input.actions["Pointer"];
            _cancel = _input.actions["Cancel"];
        }
        private void OnEnable()
        {
            _select.Enable();
            _select.performed += HandleSelect;
            _cancel.performed += HandleDeselect;
        }

        private void OnDisable()
        {
            _select.performed -= HandleSelect;
            _select.Disable();
        }

        private void HandleSelect(InputAction.CallbackContext context)
        {
            Vector2 mouseWorldPos = activeCamera.ScreenToWorldPoint(_pointerPos.ReadValue<Vector2>());
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity);
            if (hit)
            {
                Transform ship = hit.transform.parent;
                var shipInfo = ship?.GetComponent<ShipInfo>();
                if (shipInfo != null)
                {
                    SelectedShip = ship.GetComponent<ShipInfo>();
                    OnSelect?.Invoke(SelectedShip);
                    Debug.Log(ship.gameObject.name);
                }
            }
        }

        private void HandleDeselect(InputAction.CallbackContext context)
        {
            SelectedShip = null;
            OnSelect?.Invoke(SelectedShip);
            Debug.Log("Deselect");
        }
    }
}
