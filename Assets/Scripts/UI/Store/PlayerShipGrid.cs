using Ships.Components;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipGrid : MonoBehaviour
{
    // public ShipDictionary ShipDictionary;
    public ShipList playerShipList;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject shipItemPrefab;
    [SerializeField] private GameObject shipItemGrid;
    [SerializeField] private GameObject shipItemPanel;

    private void Start()
    {
        playerShipList.OnListChanged += Redraw;
        Redraw();
    }

    private void OnDestroy()
    {
        playerShipList.OnListChanged -= Redraw;
    }

    private void Redraw()
    {
        foreach (Transform child in transform) Destroy(child.gameObject);

        foreach (var ship in playerShipList.Ships)
        {
            var shipData = ship.GetComponent<ShipInfo>()?.Data;
            Debug.Assert(shipData != null, "Ship missing Stats Component");
            if (shipData != null)
            {
                var shipItem = Instantiate(shipItemPrefab, gameObject.transform, false);
                var image = shipItem.GetComponent<Image>();
                var spriteRenderer = shipData.Visuals.GetComponent<SpriteRenderer>();
                if (image != null && spriteRenderer != null) image.sprite = spriteRenderer.sprite;

                var shipItemScript = shipItem.GetComponent<ShipGridItem>();
                if (shipItemScript != null)
                {
                    shipItemScript.Ship = ship;
                    shipItemScript.ShipItemGrid = shipItemGrid;
                    shipItemScript.ShipItemPanel = shipItemPanel;
                }
            }
        }
    }
}
