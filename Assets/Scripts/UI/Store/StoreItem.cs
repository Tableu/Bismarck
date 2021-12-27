using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent StoreItemReleased;
    public UnityEvent UpdateShipName;
    public String shipName;
    public PlayerInputScriptableObject playerInput;
    private PlayerInputActions _playerInputActions;
    private Vector2 originalPos;
    private bool holding;

    private void Start()
    {
        gameObject.name = shipName;
        _playerInputActions = playerInput.PlayerInputActions;
        _playerInputActions.UI.LeftClick.canceled += DropItem;
    }

    private void Update()
    {
        if (holding)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            holding = true;
            gameObject.name = shipName;
            UpdateShipName.Invoke();
            originalPos = transform.position;
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
        transform.position = originalPos;
        holding = false;
    }
}