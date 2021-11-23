using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSellButtonClick()
    {
        if (InputManager.Instance.selectedShips.Count > 0)
        {
            foreach (GameObject ship in InputManager.Instance.selectedShips)
            {
                var controller = ship.GetComponent<ShipController>();
                StoreManager.Instance.Sell(controller.Cost);
                Destroy(ship);
            }
            InputManager.Instance.selectedShips.Clear();
            StoreManager.Instance.UpdateSellText(0);
        }
    }
}
