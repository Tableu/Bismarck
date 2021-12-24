using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Selection box code: https://gamedevacademy.org/rts-unity-tutorial/
public class ShipSelector : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public ShipListScriptableObject selectedShips;
    public ShipListScriptableObject playerShips;
    public PlayerInputScriptableObject playerInput;
    public UnityEvent SelectedShipsEvent;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private bool isStore;
    [SerializeField]private Vector2 _startPos;
    [SerializeField]private Vector2 _mousePos;
    [SerializeField]private Vector2 _projectedMousePos;
    private bool _drawSelectionBox = false;
    private bool _dragShips = false;
    private CinemachineBrain _brain;
    private CinemachineVirtualCamera _virtualCamera;
    private static ContactFilter2D PlayerFilter;
    
    private void Awake()
    {
        _playerInputActions = playerInput.PlayerInputActions;
        PlayerFilter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("PlayerShips"),
            useLayerMask = true
        };
    }
    private void OnEnable()
    {
        _playerInputActions.Enable();
        _brain = FindObjectOfType<CinemachineBrain>();
        _virtualCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    public void EnableCombatInput()
    {
        _playerInputActions.UI.Disable();
        _playerInputActions.Combat.Enable();
        Time.timeScale = 1f;
    }
    public void EnableStoreInput()
    {
        _playerInputActions.Combat.Disable();
        _playerInputActions.UI.Enable();
        Time.timeScale = 0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerInputActions.Combat.Pause.started += Pause;
        _playerInputActions.Combat.LeftClick.started += CombatLeftClick;
        _playerInputActions.Combat.LeftClick.performed += CombatLeftClick;
        _playerInputActions.Combat.LeftClick.canceled += CombatLeftClick;

        _playerInputActions.Combat.RightClick.started += CombatRightClick;
        _playerInputActions.Combat.RightClick.performed += CombatRightClick;
        _playerInputActions.Combat.RightClick.canceled += CombatRightClick;
        
        _playerInputActions.UI.LeftClick.started += StoreLeftClick;
        _playerInputActions.UI.LeftClick.performed += StoreLeftClick;
        _playerInputActions.UI.LeftClick.canceled += StoreLeftClick;
        
        _playerInputActions.UI.RightClick.started += StoreRightClick;
        _playerInputActions.UI.RightClick.performed += StoreRightClick;
        _playerInputActions.UI.RightClick.canceled += StoreRightClick;

        if (isStore)
        {
            EnableStoreInput();
        }
        else
        {
            EnableCombatInput();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = Mouse.current.position.ReadValue();
        _projectedMousePos = Camera.main.ScreenToWorldPoint(_mousePos);

        if (_dragShips)
        {
            foreach(GameObject ship in selectedShips.ShipList)
            {
                if (ship == null)
                    continue;
                ShipUI shipUI = ship.GetComponent<ShipUI>();
                if (shipUI != null) 
                { 
                    shipUI.OnDrag(null);
                }
            }
        }else if (_drawSelectionBox)
        {
            UpdateSelectionBox();
        }
    }

    private void UpdateSelectionBox()
    {
        if(!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);
        float width = _mousePos.x - _startPos.x;
        float height = _mousePos.y - _startPos.y;
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.position = _startPos + new Vector2(width / 2, height / 2);
    }

    private void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);
        Vector3 min = Camera.main.ScreenToWorldPoint(selectionBox.position - (Vector3)(selectionBox.sizeDelta / 2));
        Vector3 max = Camera.main.ScreenToWorldPoint(selectionBox.position + (Vector3)(selectionBox.sizeDelta / 2));
        
        foreach(GameObject ship in playerShips.ShipList)
        {
            if (ship == null)
                continue;
            Vector3 position = ship.transform.position;
            if (position.x < max.x && position.x > min.x && 
                position.y < max.y && position.y > min.y)
            {
                ShipUI shipUI = ship.GetComponent<ShipUI>();
                if (shipUI != null)
                {
                    shipUI.SelectShip();
                }
            }
        }

        if (selectedShips.Count > 0)
        {
            SelectedShipsEvent.Invoke();
        }
    }
    private void CombatLeftClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                DeSelectShips();
                _drawSelectionBox = true;
                _startPos = _mousePos;
                break;
            case InputActionPhase.Canceled:
                if (_drawSelectionBox)
                {
                    _drawSelectionBox = false;
                    ReleaseSelectionBox();
                }
                break;
        }
    }

    private void CombatRightClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if(selectedShips.Count > 0)
                    MoveSelectedShips(_mousePos);
                break;
        }
    }
    
    private void StoreLeftClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _startPos = _mousePos;
                if (UIRaycast())
                    return;

                if (selectedShips.Count > 0)
                {
                    foreach (GameObject ship in selectedShips.ShipList)
                    {
                        if (ship != null)
                        {
                            ShipUI shipUI = ship.GetComponent<ShipUI>();
                            if (shipUI != null)
                            {
                                shipUI.RefreshPositionOffset();
                            }
                        }
                    }
                    _dragShips = true;
                }else
                {
                    _drawSelectionBox = true;
                }
                break;
            case InputActionPhase.Canceled:
                DeSelectShips();
                _dragShips = false;
                if (_drawSelectionBox)
                {
                    _drawSelectionBox = false;
                    ReleaseSelectionBox();
                }
                break;
        }
    }
    private void StoreRightClick(InputAction.CallbackContext context)
    {
        
    }

    private bool UIRaycast()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = _mousePos;
        List<RaycastResult> hits = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, hits);
        if (hits.Count > 0)
            return true;
        return false;
    }
    public void DeSelectShips()
    {
        foreach(GameObject ship in selectedShips.ShipList)
        {
            if (ship != null)
            {
                ShipUI shipUI = ship.GetComponent<ShipUI>();
                if (shipUI != null)
                {
                    shipUI.DeselectShip();
                }
            }
        }
        selectedShips.ClearList();
        SelectedShipsEvent.Invoke();
    }

    public void DeselectShip(GameObject ship)
    {
        if (ship != null)
        {
            ShipUI shipUI = ship.GetComponent<ShipUI>();
            if (shipUI != null)
            {
                shipUI.DeselectShip();
            }

        }
    }
    public void MoveSelectedShips(Vector2 position)
    {
        foreach(GameObject ship in selectedShips.ShipList)
        {
            if (ship != null)
            {
                ship.GetComponent<ShipLogic>().MoveToPosition(Camera.main.ScreenToWorldPoint(position));
            }
        }
        _drawSelectionBox = false;
    }
    private void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("Pause");
        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
