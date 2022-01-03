using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class StoreItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent StoreItemSelected;
    public UnityEvent StoreItemReleased;
    public string ItemName;
    [SerializeField] private StoreViewModel store;
    private Vector2 originalPos;
    private bool holding;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (holding)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            holding = true;
            store.SelectedItem = ItemName;
            originalPos = transform.position;
            StoreItemSelected.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            StoreItemReleased.Invoke();
            transform.position = new Vector3(originalPos.x, originalPos.y, 0);
            holding = false;
        }
    }

    private void DropItem(InputAction.CallbackContext callbackContext)
    {
        if (!holding)
            return;
        
        /*List<RaycastHit2D> results = new List<RaycastHit2D>();
        
        Physics2D.BoxCast(Camera.main.ScreenToWorldPoint(transform.position), 
            GetComponent<BoxCollider2D>().size,0,Vector2.zero, playerInput.PlayerFilter, results);
        if (results.Count <= 0)
        {
            gameObject.name = shipName;
            StoreItemReleased.Invoke();
        }*/
        StoreItemReleased.Invoke();
        transform.position = new Vector3(originalPos.x, originalPos.y, 0);
        holding = false;
    }
}