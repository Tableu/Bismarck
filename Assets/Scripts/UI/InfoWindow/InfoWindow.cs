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
        public ShipInfo Player;
        [SerializeField] private SubsystemButtonManager subsystemButtonManager;
        [SerializeField] private ShipInfo shipInfo;
        [SerializeField] private Button closeButton;
        [SerializeField] private Slider healthBar;
        private Hull _hull;

        public void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(CloseWindow);
            }

            if (shipInfo == null)
            {
                gameObject.SetActive(false);
            }

            subsystemButtonManager.Player = Player;
        }

        public void Start()
        {
            if (shipInfo != null)
            {
                Refresh(shipInfo);
            }
        }

        private void OnDestroy()
        {
            if (_hull != null)
            {
                _hull.OnHealthChanged -= Redraw;
            }

            if (shipInfo != null)
            {
                shipInfo.OnShipDestroyed -= CloseWindow;
            }
        }

        public void Refresh(ShipInfo newTarget = null)
        {
            if (newTarget != null)
            {
                if (shipInfo != null)
                {
                    shipInfo.OnShipDestroyed -= CloseWindow;
                    if (_hull != null)
                    {
                        _hull.OnHealthChanged -= Redraw;
                    }
                }

                shipInfo = newTarget;
                shipInfo.OnShipDestroyed += CloseWindow;
                _hull = shipInfo.GetComponent<Hull>();
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

            if (Camera != null && shipInfo != null)
            {
                var shipPos = shipInfo.Visuals.transform.position;
                Camera.transform.position = new Vector3(shipPos.x, shipPos.y, Camera.transform.position.z);

                subsystemButtonManager.Player = Player;
                subsystemButtonManager.Refresh(shipInfo);
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
        [Header("Debug")] [SerializeField] private ShipInfo testInfo;

        [ContextMenu("Refresh")]
        private void DebugRefresh()
        {
            Refresh(testInfo);
        }
#endif
    }
}