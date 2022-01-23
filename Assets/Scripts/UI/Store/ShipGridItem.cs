using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipGridItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Ship;
    public Button Button;
    public GameObject ShipItemGrid;

    public GameObject ShipItemPanel;

    // Start is called before the first frame update
    void Start()
    {
        Button.onClick.AddListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (Ship == null)
            return;
        ShipUI shipUI = Ship.GetComponent<ShipUI>();
        if (shipUI != null)
        {
            shipUI.SelectShip();
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (Ship == null)
            return;
        ShipUI shipUI = Ship.GetComponent<ShipUI>();
        if (shipUI != null)
        {
            shipUI.DeselectShip();
        }
    }

    private void OnClick()
    {
        ShipItemGrid.SetActive(false);
        ViewShipPanel viewShipPanel = ShipItemPanel.GetComponent<ViewShipPanel>();
        if (viewShipPanel != null)
        {
            viewShipPanel.Ship = Ship;
        }

        ShipItemPanel.SetActive(true);
    }
}