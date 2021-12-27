using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private Transform target;
    [SerializeField] private int playerHealth;
    [SerializeField] private float barDisplacement;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            
            gameObject.transform.position = Camera.main.WorldToScreenPoint((Vector2)target.position + Vector2.down*barDisplacement);
        }
    }

    public void Init(Transform target, int maxHealth, int health, float displacement)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        barDisplacement = displacement;
        this.target = target;
    }

    public void SetHealth(int health)
    {
        healthBar.value = health;
    }
}
