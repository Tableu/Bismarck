using System.Collections.Generic;
using System.ComponentModel;
using Attacks;
using Ships.Components;
using Ships.Fleets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityWeld.Binding;

[Binding]
public class StoreViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField] private FleetManager shipSpawner;
    [SerializeField] private Transform fleetParent;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private ShipInfoPopup shipInfoPopup;
    public ShipList selectedShips;
    public ShipDBScriptableObject shipDB;
    public AttackDBScriptableObject attackDB;
    public PlayerInputScriptableObject playerInput;
    private int money = 1000;
    private int repairCost;
    private int sellValue;

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

    public string SelectedItem { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

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
            foreach (var ship in selectedShips.Ships)
            {
                var data = ship.GetComponent<ShipInfo>().Data;
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
            foreach (var ship in selectedShips.Ships)
            {
                Money -= ComputeRepairCost(ship);
                var health = ship.GetComponent<Hull>();
                health.Repair();
            }

            UpdateRepairCostAndSellValue();
        }
    }

    [Binding]
    public void BuyShip()
    {
        var shipData = shipDB.GetShip(SelectedItem);
        Debug.Assert(shipData != null, "Failed to find ship in database");
        if (shipData != null && money - shipData.Cost >= 0)
        {
            Money -= shipData.Cost;
            Vector2 startingPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var ship = shipSpawner.SpawnShip(shipData, startingPos);
            var shipLogic = ship.GetComponent<ShipLogic>();
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
        var attack = attackDB.GetAttack(SelectedItem);
        if (attack != null && money - attack.Cost >= 0)
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Mouse.current.position.ReadValue();
            var hits = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, hits);

            foreach (var hit in hits)
            {
                if (hit.gameObject.CompareTag("Weapon"))
                {
                    var index = hit.gameObject.transform.GetSiblingIndex();
                    if (shipInfoPopup.Ship != null)
                    {
                        var shipData = shipInfoPopup.Ship.GetComponent<ShipInfo>().Data;
                        shipData.Weapons[index] = attack;
                        Money -= attack.Cost;
                        var turrets = shipInfoPopup.Ship.GetComponent<ShipTurrets>();
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
        foreach (var ship in selectedShips.Ships)
        {
            RepairCost += ComputeRepairCost(ship);
            SellValue += ComputeSellValue(ship);
        }
    }

    private static int ComputeRepairCost(GameObject ship)
    {
        var data = ship.GetComponent<ShipInfo>().Data;
        var health = ship.GetComponent<Hull>();
        return (int)(data.Cost * (1 - health.PercentHealth));
    }

    private static int ComputeSellValue(GameObject ship)
    {
        var data = ship.GetComponent<ShipInfo>().Data;
        var health = ship.GetComponent<Hull>();
        return (int)(data.Cost * health.PercentHealth);
    }
}
