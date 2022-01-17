using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityWeld.Binding;

[Binding]
public class StoreViewModel : MonoBehaviour, INotifyPropertyChanged
{
    private int money = 1000;
    private int repairCost;
    private int sellValue;
    private string _selectedItem;
    [SerializeField] private Transform fleetParent;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private ShipInfoPopup shipInfoPopup;
    public ShipListScriptableObject selectedShips;
    public ShipDBScriptableObject shipDB;
    public AttackDBScriptableObject attackDB;
    public PlayerInputScriptableObject playerInput;
    public ShipSpawner ShipSpawner;
    public ShipDictionary ShipDictionary;

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
    
    public string SelectedItem
    {
        get => _selectedItem;
        set => _selectedItem = value;
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
                ShipData data = ShipDictionary.GetShip(ship.GetInstanceID());
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
                ShipData data = ShipDictionary.GetShip(ship.GetInstanceID());
                Money -= data.RepairCost;
                data.Health = data.MaxHealth;
            }
            UpdateRepairCostAndSellValue();
        }
    }

    [Binding]
    public void BuyShip()
    {
        ShipData shipData = shipDB.GetShip(_selectedItem).MakeShipData();
        if (shipData != null && money - shipData.Cost >= 0)
        {
            Money -= shipData.Cost;
            Vector2 startingPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            GameObject ship = ShipSpawner.SpawnShip(shipData, fleetParent, startingPos);
            ShipLogic shipLogic = ship.GetComponent<ShipLogic>();
            if (shipLogic != null)
            {
                shipLogic.enabled = false;
            }
            UpdateRepairCostAndSellValue();
        }
    }

    [Binding]
    public void BuyWeapon()
    {
        AttackScriptableObject attack = attackDB.GetAttack(_selectedItem);
        if (attack != null && money - attack.Cost >= 0)
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Mouse.current.position.ReadValue();
            List<RaycastResult> hits = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, hits);

            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.CompareTag("Weapon"))
                {
                    int index = hit.gameObject.transform.GetSiblingIndex();
                    if (shipInfoPopup.Ship != null)
                    {
                        ShipData shipData = ShipDictionary.GetShip(shipInfoPopup.Ship.GetInstanceID());
                        shipData.Weapons[index] = attack;
                        Money -= attack.Cost;
                        ShipTurrets turrets = shipInfoPopup.Ship.GetComponent<ShipTurrets>();
                        if (turrets != null)
                        {
                            turrets.Refresh();
                        }

                        shipInfoPopup.Refresh(shipInfoPopup.Ship);
                    }
                    UpdateRepairCostAndSellValue();
                    break;
                }
            }
        }
    }

    public void UpdateRepairCostAndSellValue()
    {
        RepairCost = 0;
        SellValue = 0;
        foreach (GameObject ship in selectedShips.ShipList)
        {
            ShipData data = ShipDictionary.GetShip(ship.GetInstanceID());
            RepairCost += data.RepairCost;
            SellValue += data.SellValue;
        }
    }
}
