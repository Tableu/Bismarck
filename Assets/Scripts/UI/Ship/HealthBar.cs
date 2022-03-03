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
    private Hull _hull;
    
    private void Start()
    {
        healthBar = GetComponent<Slider>();
    }
    
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
        if (_hull != null)
        {
            _hull.OnHealthChanged -= Redraw;
        }
    }

    public void Bind(Hull bindingTarget)
    {
        healthBar.maxValue = 1f;
        healthBar.value = bindingTarget.PercentHealth;
        target = bindingTarget.transform;
        _hull = bindingTarget;
        _hull.OnHealthChanged += Redraw;
        transform.localPosition = Vector2.down * barDisplacement;
    }

    private void Redraw()
    {
        healthBar.value = _hull.PercentHealth;
    }
}
