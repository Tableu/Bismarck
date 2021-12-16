using UnityEngine;

public class RepairButton : MonoBehaviour
{
    // Start is called before the first frame update
    public StoreWindow store;
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
                var controller = ship.GetComponent<ShipStoreController>();
                store.Repair(controller.RepairCost());
                controller.Repair();
            }
            InputManager.Instance.selectedShips.Clear();
            store.UpdateRepairText(0);
            store.UpdateSellText(0);
        }    
    }
}
