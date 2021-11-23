using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRepairButtonClick()
    {
        if (InputManager.Instance.selectedShips.Count > 0)
        {
            foreach (GameObject ship in InputManager.Instance.selectedShips)
            {
                var controller = ship.GetComponent<ShipController>();
                StoreManager.Instance.Repair(controller.RepairCost());
                controller.Repair();
            }
            InputManager.Instance.selectedShips.Clear();
            StoreManager.Instance.UpdateRepairText(0);
            StoreManager.Instance.UpdateSellText(0);
        }    
    }
}
