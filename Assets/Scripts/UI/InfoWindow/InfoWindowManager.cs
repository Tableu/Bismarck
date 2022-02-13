using Ships.Components;
using UnityEngine;

/// <summary>
/// Instantiates InfoWindow and attaches them to each ShipInfo in its children
/// </summary>
public class InfoWindowManager : MonoBehaviour
{
    [SerializeField] private GameObject infoWindowCameraPrefab;

    void Start()
    {
        ShipInfo[] shipInfos = GetComponentsInChildren<ShipInfo>();
        if (shipInfos != null && shipInfos.Length > 0)
        {
            foreach (ShipInfo shipInfo in shipInfos)
            {
                if (shipInfo != null)
                {
                    SpawnInfoWindow(shipInfo.gameObject);
                }
            }
        }
    }

    public void SpawnInfoWindow(GameObject ship)
    {
        GameObject infoWindow = Instantiate(infoWindowCameraPrefab, ship.transform);
        infoWindow.SetActive(false);
    }
}