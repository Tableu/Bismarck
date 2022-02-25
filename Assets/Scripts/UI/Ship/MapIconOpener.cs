using Ships.Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapIconOpener : MonoBehaviour
{
    [SerializeField] private GameObject infoWindowPrefab;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject infoWindowCameras;
    [SerializeField] private ShipInfo player;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),
                Vector2.zero,
                Mathf.Infinity,
                LayerMask.GetMask("Map"));
            if (hit)
            {
                var ship = hit.transform.parent.gameObject.GetComponent<ShipInfo>();
                if (ship != null)
                {
                    SpawnInfoWindow(ship);
                }
            }
        }
    }

    private void SpawnInfoWindow(ShipInfo shipInfo)
    {
        GameObject infoWindow = Instantiate(infoWindowPrefab, canvas.transform);
        GameObject camera = Instantiate(cameraPrefab, infoWindowCameras.transform);
        InfoWindow infoWindowScript = infoWindow.GetComponent<InfoWindow>();
        infoWindowScript.Camera = camera.GetComponent<Camera>();
        infoWindowScript.Player = player;
        infoWindowScript.Refresh(shipInfo);
    }
}