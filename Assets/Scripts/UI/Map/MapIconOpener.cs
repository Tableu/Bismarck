using Ships.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Map
{
    /// <summary>
    ///     Opens the relevant info window and camera, when a ships MapIcon is clicked. Refreshes info window on spawn
    /// </summary>
    public class MapIconOpener : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject enemyInfoWindow;
        [SerializeField] private GameObject enemyCamera;
        [SerializeField] private GameObject playerInfoWindow;
        [SerializeField] private GameObject playerCamera;

        [Header("Scene References")] [SerializeField]
        private GameObject canvas;

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
                    if (ship != null && ship != player)
                    {
                        SpawnInfoWindow(ship);
                    }
                }
            }
        }

        private void SpawnInfoWindow(ShipInfo shipInfo)
        {
            GameObject infoWindow;
            GameObject camera;
            if (shipInfo == player)
            {
                infoWindow = Instantiate(playerInfoWindow, canvas.transform);
                camera = Instantiate(playerCamera, infoWindowCameras.transform);
            }
            else
            {
                infoWindow = Instantiate(enemyInfoWindow, canvas.transform);
                camera = Instantiate(enemyCamera, infoWindowCameras.transform);
            }

            InfoWindow.InfoWindow infoWindowScript = infoWindow.GetComponent<InfoWindow.InfoWindow>();
            infoWindowScript.Camera = camera.GetComponent<Camera>();
            infoWindowScript.Player = player;
            infoWindowScript.Refresh(shipInfo);
        }
    }
}