using UnityEngine;
using UnityEngine.InputSystem;

public class RightClickInspect : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public PlayerInputScriptableObject playerInput;
    [SerializeField] private GameObject shipInfoPopup;
    private void Awake()
    {
        _playerInputActions = playerInput.PlayerInputActions;
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerInputActions.UI.RightClick.started += RightClick;
    }

    private void RightClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        
        var hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("PlayerShips"));
        if (hit)
        {
            shipInfoPopup.transform.position = mousePos;
            ShipInfoPopup infoPopup = shipInfoPopup.GetComponent<ShipInfoPopup>();
            if (infoPopup != null)
            {
                infoPopup.Refresh(hit.collider.gameObject);
            }

            shipInfoPopup.SetActive(true);
        }
        else
        {
            shipInfoPopup.SetActive(false);
        }
    }
}
