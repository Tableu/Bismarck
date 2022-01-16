using UnityEngine;
using UnityEngine.UI;

public class ViewShipPanel : MonoBehaviour
{
    public GameObject Ship;
    public Image ShipImage;
    private void OnEnable()
    {
        ShipImage.sprite = Ship.GetComponent<SpriteRenderer>().sprite;
    }
}
