using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipInfoPopup : MonoBehaviour
{
    public ShipDictionary ShipDictionary;
    public GameObject Ship;
    public ShipData _shipData;
    
    [SerializeField] private GameObject weaponSlotPrefab;
    [SerializeField] private GameObject moduleSlotPrefab;
    [SerializeField] private GameObject weaponsGridLayout;
    [SerializeField] private GameObject modulesGridLayout;
    [SerializeField] private GameObject weaponsLabel;
    [SerializeField] private GameObject modulesLabel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        _shipData = ShipDictionary.GetShip(Ship.GetInstanceID());
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
        ShipTurrets turrets = _shipData.ShipPrefab.GetComponent<ShipTurrets>(); 
        List<AttackScriptableObject>.Enumerator weapons = _shipData.Weapons.GetEnumerator();
        int weaponSlotCount = turrets != null ? turrets.turretPositions.Count : _shipData.Weapons.Count;

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
                }
            }
        }
    }
}
