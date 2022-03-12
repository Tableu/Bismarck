using Ships.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InfoWindow
{
    /// <summary>
    ///     Refreshes InfoWindow based on a ShipInfo instance. Enables/Disables the InfoWindow camera and canvas elements
    ///     accordingly.
    /// </summary>
    public class InfoWindow : MonoBehaviour
    {
        public Camera Camera;
        public ShipStats Player;
        [SerializeField] private SubsystemButtonManager subsystemButtonManager;
        [SerializeField] private ShipStats shipStats;
        [SerializeField] private Button closeButton;
        [SerializeField] private Slider healthBar;
        private Hull _hull;

        public void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(CloseWindow);
            }

            if (shipStats == null)
            {
                gameObject.SetActive(false);
            }

            subsystemButtonManager.Player = Player;
        }

        public void Start()
        {
            if (shipStats != null)
            {
                Refresh(shipStats);
            }
        }

        private void OnDestroy()
        {
            if (_hull != null)
            {
                _hull.OnHealthChanged -= Redraw;
            }

            if (shipStats != null)
            {
                shipStats.OnShipDestroyed -= CloseWindow;
            }
        }

        public void Refresh(ShipStats newTarget = null)
        {
            if (newTarget != null)
            {
                if (shipStats != null)
                {
                    shipStats.OnShipDestroyed -= CloseWindow;
                    if (_hull != null)
                    {
                        _hull.OnHealthChanged -= Redraw;
                    }
                }

                shipStats = newTarget;
                shipStats.OnShipDestroyed += CloseWindow;
                _hull = shipStats.GetComponent<Hull>();
                if (_hull != null)
                {
                    healthBar.gameObject.SetActive(true);
                    _hull.OnHealthChanged += Redraw;
                }
                else
                {
                    healthBar.gameObject.SetActive(false);
                }
            }

            if (Camera != null && shipStats != null)
            {
                var shipPos = shipStats.Visuals.transform.position;
                Camera.transform.position = new Vector3(shipPos.x, shipPos.y, Camera.transform.position.z);

                subsystemButtonManager.Player = Player;
                subsystemButtonManager.Refresh(shipStats);
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void CloseWindow()
        {
            if (Camera != null)
            {
                Destroy(Camera.gameObject);
            }

            if (this != null && gameObject != null)
            {
                Destroy(gameObject);
            }
        }

        private void Redraw()
        {
            healthBar.value = _hull.PercentHealth;
        }

#if UNITY_EDITOR
        [Header("Debug")] [SerializeField] private ShipStats testStats;

        [ContextMenu("Refresh")]
        private void DebugRefresh()
        {
            Refresh(testStats);
        }
#endif
    }
}