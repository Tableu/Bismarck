using Ships.Components;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private Transform target;
    [SerializeField] private int playerHealth;

    [SerializeField] private float barDisplacement;
    private ShipHealth _shipHealth;

    // Start is called before the first frame update
    private void Start()
    {
        healthBar = GetComponent<Slider>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (target != null)
        {
            gameObject.transform.position = (Vector2)target.position + Vector2.down * barDisplacement;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_shipHealth != null)
        {
            _shipHealth.OnHealthChanged -= Redraw;
        }
    }

    public void Bind(ShipHealth bindingTarget)
    {
        healthBar.maxValue = 1f;
        healthBar.value = bindingTarget.PercentHealth;
        target = bindingTarget.transform;
        _shipHealth = bindingTarget;
        _shipHealth.OnHealthChanged += Redraw;
        transform.localPosition = Vector2.down * barDisplacement;
    }

    private void Redraw()
    {
        healthBar.value = _shipHealth.PercentHealth;
    }
}
