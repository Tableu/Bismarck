using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class StoreViewModel : MonoBehaviour, INotifyPropertyChanged
{
    private string money;
    private string repairCost;
    private string sellValue;
    private string shipToSpawn;
    [SerializeField] private Transform fleetParent;
    public ShipListScriptableObject selectedShips;
    public ShipDBScriptableObject shipDB;
    public PlayerInputScriptableObject playerInput;
    public ShipSpawner spawner;
    public ShipDictionary ships;
    private int playerMoney;

    [Binding]
    public string Money
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
    public string RepairCost
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
    public string SellValue
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
                playerMoney += data.Cost;
                Destroy(ship);
            }
            selectedShips.ClearList();
            Money = playerMoney.ToString();
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
                playerMoney -= data.RepairCost;
                Destroy(ship);
            }
            selectedShips.ClearList();
            Money = playerMoney.ToString();
        }
    }

    [Binding]
    public void BuyShip()
    {
        ShipData shipData = shipDB.GetShip(shipToSpawn);
        if (shipData != null && playerMoney - shipData.Cost >= 0)
        {
            playerMoney -= shipData.Cost;
            Money = playerMoney.ToString();
            Vector2 startingPos = Camera.main.ScreenToWorldPoint(playerInput.PlayerInputActions.Mouse.Point.ReadValue<Vector2>());
            spawner.SpawnShip(shipData, fleetParent, startingPos);
        }
    }
}
