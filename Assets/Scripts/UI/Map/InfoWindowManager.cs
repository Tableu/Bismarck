using Ships.Components;
using UnityEngine;

namespace UI.Map
{
    /// <summary>
    ///     Opens the relevant info window and camera, when a ships MapIcon is clicked. Refreshes info window on spawn
    /// </summary>
    public class InfoWindowManager : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject enemyInfoWindow;
        [SerializeField] private GameObject enemyCamera;
        [SerializeField] private GameObject playerInfoWindow;
        [SerializeField] private GameObject playerCamera;

        [Header("Scene References")] [SerializeField]
        private GameObject canvas;

        [SerializeField] private GameObject infoWindowCameras;
        [SerializeField] private ShipStats player;

        private static InfoWindowManager _instance;

        public static InfoWindowManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InfoWindowManager>();
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject();
                        _instance = gameObject.AddComponent<InfoWindowManager>();
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null || _instance == this)
            {
                _instance = this;
            }
            else
            {
                Debug.LogError("InfoWindowManager destroyed");
                Destroy(gameObject);
            }
        }

        public void SpawnInfoWindow(ShipStats shipStats)
        {
            GameObject infoWindow;
            GameObject camera;
            if (shipStats == player)
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
            infoWindowScript.Refresh(shipStats);
        }
    }
}