using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class StoreItem : MonoBehaviour
{
    public UnityEvent StoreItemReleased;
    public String shipName;
    public PlayerInputScriptableObject playerInput;
    private PlayerInputActions _playerInputActions;
    private Vector2 originalPos;
    private bool holding;

    private void Start()
    {
        gameObject.name = shipName;
        _playerInputActions = playerInput.PlayerInputActions;
        _playerInputActions.UI.LeftClick.performed += HoldAndDragItem;
        _playerInputActions.UI.LeftClick.canceled += DropItem;
    }
    public void OnClick()
    {
        originalPos = transform.position;
        holding = true;
    }

    private void HoldAndDragItem(InputAction.CallbackContext callbackContext)
    {
        if (!holding)
            return;
        gameObject.transform.position = _playerInputActions.Mouse.Point.ReadValue<Vector2>();
    }

    private void DropItem(InputAction.CallbackContext callbackContext)
    {
        if (!holding)
            return;
        
        List<RaycastHit2D> results = new List<RaycastHit2D>();
        Physics2D.BoxCast(Camera.main.ScreenToWorldPoint(transform.position), 
            GetComponent<BoxCollider2D>().size,0,Vector2.zero, playerInput.PlayerFilter, results);
        if (results.Count <= 0)
        {
            StoreItemReleased.Invoke();
        }
        transform.position = originalPos;
        holding = false;
    }
}