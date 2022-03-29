using System.Collections.Generic;
using Ships.Components;
using Ships.DataManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipInfoPopup : MonoBehaviour
{
    // public ShipDictionary ShipDictionary;
    public GameObject Ship;
    public ShipData _shipData;

    [SerializeField] private GameObject weaponSlotPrefab;
    [SerializeField] private GameObject moduleSlotPrefab;
    [SerializeField] private GameObject weaponsGridLayout;
    [SerializeField] private GameObject modulesGridLayout;
    [SerializeField] private GameObject weaponsLabel;
    [SerializeField] private GameObject modulesLabel;
    [SerializeField] private GraphicRaycaster graphicRaycaster;

    public void Refresh(GameObject ship)
    {
        foreach (Transform child in weaponsGridLayout.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in modulesGridLayout.transform)
        {
            Destroy(child.gameObject);
        }

        Ship = ship;
        _shipData = ship.GetComponent<ShipStats>().Data;
        SpawnWeapons();
        bool weapons = weaponsGridLayout.transform.childCount != 0;
        bool modules = modulesGridLayout.transform.childCount != 0;
        weaponsLabel.SetActive(weapons);
        weaponsGridLayout.SetActive(weapons);
        modulesLabel.SetActive(modules);
        modulesGridLayout.SetActive(modules);
    }

    private void SpawnWeapons()
    {
        ShipTurrets turrets = _shipData.Visuals.GetComponent<ShipTurrets>();
        var weapons = _shipData.Weapons.GetEnumerator();
        var weaponSlotCount = turrets != null ? turrets.TurretPositions.Count : _shipData.Weapons.Count;

        while (weaponSlotCount != 0)
        {
            GameObject weaponSlot = Instantiate(weaponSlotPrefab, weaponsGridLayout.transform, false);
            weaponSlotCount--;
            if (weapons.MoveNext())
            {
                if (weapons.Current != null)
                {
                    weaponSlot.GetComponent<Image>().sprite =
                        weapons.Current.Turret.GetComponent<SpriteRenderer>().sprite;
                    DraggableItem draggableItem = weaponSlot.AddComponent<DraggableItem>();
                    //draggableItem.ItemName = weapons.Current.AttackName;
                    draggableItem.ItemReleased = new UnityEvent();
                    draggableItem.ItemSelected = new UnityEvent();
                    draggableItem.ItemReleased.AddListener(DropItem);
                }
                else
                {
                    weaponSlot.GetComponent<Image>().color = Color.blue;
                }
            }
        }
    }

    private void DropItem()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();
        List<RaycastResult> hits = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, hits);
        foreach (RaycastResult hit in hits)
        {
            switch (hit.gameObject.tag)
            {
                case "Store":

                    break;
                case "WeaponSlot":
                    break;
            }
        }
    }
}
