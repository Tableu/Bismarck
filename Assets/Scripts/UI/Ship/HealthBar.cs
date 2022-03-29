using Ships.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Ship
{
    /// <summary>
    ///     A canvas healthbar that tracks a ships location
    /// </summary>
    public class HealthBar : BindableUIComponent<Hull>
    {
        [SerializeField] private Slider healthBar;
        [SerializeField] private int playerHealth;
        private Hull _hull;

        private void Start()
        {
            healthBar = GetComponent<Slider>();
        }

        private void OnDestroy()
        {
            if (_hull != null)
            {
                _hull.OnHealthChanged -= Redraw;
            }
        }

        public override void Bind(Hull bindingTarget)
        {
            if (bindingTarget != null)
            {
                healthBar.maxValue = 1f;
                healthBar.value = bindingTarget.PercentHealth;
                target = bindingTarget.transform;
                _hull = bindingTarget;
                _hull.OnHealthChanged += Redraw;
                _hull.OnDisabledChanged += delegate(bool disabled)
                {
                    if (disabled)
                    {
                        Destroy(gameObject);
                    }
                };
                transform.localPosition = displacement;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void Redraw()
        {
            healthBar.value = _hull.PercentHealth;
        }
    }
}