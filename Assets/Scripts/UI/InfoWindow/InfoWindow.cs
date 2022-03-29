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
        [SerializeField] private ShipStats target;
        [SerializeField] private Button closeButton;
        [SerializeField] private Slider healthBar;
        private Hull _hull;

        public void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(CloseWindow);
            }

            if (target == null)
            {
                gameObject.SetActive(false);
            }

            subsystemButtonManager.Player = Player;
        }

        public void Start()
        {
            if (target != null)
            {
                Refresh(target);
            }
        }

        private void OnDestroy()
        {
            if (_hull != null)
            {
                _hull.OnHealthChanged -= Redraw;
            }

            if (target != null)
            {
                target.OnShipDestroyed -= CloseWindow;
            }
        }

        public void Refresh(ShipStats newTarget = null)
        {
            if (newTarget != null)
            {
                if (target != null)
                {
                    target.OnShipDestroyed -= CloseWindow;
                    if (_hull != null)
                    {
                        _hull.OnHealthChanged -= Redraw;
                    }
                }

                target = newTarget;
                target.OnShipDestroyed += CloseWindow;
                _hull = target.GetComponent<Hull>();
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

            if (Camera != null && target != null)
            {
                var shipPos = target.Visuals.transform.position;
                Camera.transform.position = new Vector3(shipPos.x, shipPos.y, Camera.transform.position.z);

                subsystemButtonManager.Player = Player;
                subsystemButtonManager.Refresh(target);
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void CloseWindow()
        {
            if (Player != null && Player.TargetingHelper.Target != null &&
                Player.TargetingHelper.Target.ShipStats == target)
            {
                Player.TargetingHelper.SetTarget(null);
            }

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