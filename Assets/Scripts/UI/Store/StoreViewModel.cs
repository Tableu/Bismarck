using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class StoreViewModel : MonoBehaviour, INotifyPropertyChanged
{
    private int money = 1000;
    private int repairCost;
    private int sellValue;
    private string shipToSpawn;
    [SerializeField] private Transform fleetParent;
    public ShipListScriptableObject selectedShips;
    public ShipDBScriptableObject shipDB;
    public PlayerInputScriptableObject playerInput;
    public ShipSpawner spawner;
    public ShipDictionary ships;

    [Binding]
    public int Money
    {
        get => money;
        set
        {
            if (money == value)
            {
                return;
            }

            money = value;
            OnPropertyChanged("Money");
        }
    }

    [Binding]
    public int RepairCost
    {
        get => repairCost;
        set
        {
            if (repairCost == value)
            {
                return;
            }
            repairCost = value;
            OnPropertyChanged("RepairCost");
        }
    }

    [Binding]
    public int SellValue
    {
        get => sellValue;
        set
        {
            if (sellValue == value)
            {
                return;
            }
            sellValue = value;
            OnPropertyChanged("SellValue");
        }
    }

    [Binding]
    public string ShipToSpawn
    {
        get => shipToSpawn;
        set
        {
            if (shipToSpawn == value)
            {
                return;
            }
            shipToSpawn = value;
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [Binding]
    public void SellShip()
    {
        if (selectedShips.Count > 0)
        {
            foreach (GameObject ship in selectedShips.ShipList)
            {
                ShipData data = ships.GetShip(ship.GetInstanceID());
                Money += data.Cost;
                Destroy(ship);
            }
            selectedShips.ClearList();
            UpdateRepairCostAndSellValue();
        }
        Debug.Log("SellShip");
    }

    [Binding]
    public void RepairShip()
    {
        if (selectedShips.Count > 0)
        {
            foreach (GameObject ship in selectedShips.ShipList)
            {
                ShipData data = ships.GetShip(ship.GetInstanceID());
                Money -= data.RepairCost;
                Destroy(ship);
            }
            selectedShips.ClearList();
            UpdateRepairCostAndSellValue();
        }
    }

    [Binding]
    public void BuyShip()
    {
        ShipData shipData = shipDB.GetShip(shipToSpawn);
        if (shipData != null && money - shipData.Cost >= 0)
        {
            Money -= shipData.Cost;
            Vector2 startingPos = Camera.main.ScreenToWorldPoint(playerInput.PlayerInputActions.Mouse.Point.ReadValue<Vector2>());
            spawner.SpawnShip(shipData, fleetParent, startingPos);
            UpdateRepairCostAndSellValue();
        }
    }

    public void UpdateRepairCostAndSellValue()
    {
        foreach (GameObject ship in selectedShips.ShipList)
        {
            ShipData data = ships.GetShip(ship.GetInstanceID());
            RepairCost += data.RepairCost;
            SellValue += data.SellValue;
        }
    }
}
