using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipGrid : MonoBehaviour
{
    public ShipDictionary ShipDictionary;
    public ShipListScriptableObject ShipList;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject shipItemPrefab;
    [SerializeField] private GameObject shipItemGrid;
    [SerializeField] private GameObject shipItemPanel;
    void Start()
    {
        Dictionary<int,ShipData>.Enumerator enumerator = ShipDictionary.GetEnumerator();
        while (enumerator.MoveNext()){
            ShipData shipData = enumerator.Current.Value;
            if (shipData != null)
            {
                GameObject shipItem = Instantiate(shipItemPrefab, gameObject.transform, false);
                Image image = shipItem.GetComponent<Image>();
                SpriteRenderer spriteRenderer = shipData.ShipPrefab.GetComponent<SpriteRenderer>();
                if (image != null && spriteRenderer != null)
                {
                    image.sprite = spriteRenderer.sprite;
                }
                
                ShipGridItem shipItemScript = shipItem.GetComponent<ShipGridItem>();
                if (shipItemScript != null)
                {
                    shipItemScript.Ship = ShipList.GetShip(enumerator.Current.Key);
                    shipItemScript.ShipItemGrid = shipItemGrid;
                    shipItemScript.ShipItemPanel = shipItemPanel;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
