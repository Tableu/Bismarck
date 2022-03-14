using Ships.Components;
using UI.Map;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Ship
{
    /// <summary>
    ///     A clickable button that tracks a ships location.
    /// </summary>
    public class ShipIcon : BindableUIComponent<ShipStats>
    {
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        private ShipStats ship;

        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        public override void Bind(ShipStats bindingTarget)
        {
            if (bindingTarget != null)
            {
                target = bindingTarget.transform;
                ship = bindingTarget;
                image.sprite = bindingTarget.Data.MapIcon;
                bindingTarget.OnShipDestroyed += delegate
                {
                    if (this != null && gameObject != null)
                    {
                        Destroy(gameObject);
                    }
                };
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnClick()
        {
            InfoWindowManager.Instance.SpawnInfoWindow(ship);
        }
    }
}