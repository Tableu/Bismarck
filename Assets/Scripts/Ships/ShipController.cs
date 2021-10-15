using UnityEngine;
using UnityEngine.EventSystems;

public class ShipController : MonoBehaviour, IDamageable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    protected MovementController _movementController;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private bool move = true;
    [SerializeField] private bool attack = false;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float fireDelay;
    [SerializeField] private float fireTimer;
    [SerializeField] private Vector2 moveTarget;
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private LayerMask layerMask;
    protected FSM StateMachine;
    private void Awake()
    {
        _movementController = new MovementController(gameObject, speed, rotationSpeed, layerMask);
    }

    protected void Start()
    {
        var moving = new MoveForwardState(this, _movementController);
        StateMachine = new FSM();
        StateMachine.SetState(moving);
        ShipManager.Instance.AddShip(gameObject);
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        StateMachine.Tick();
        if (attack)
        {
            if (Mathf.Abs(Time.time-fireTimer) >= fireDelay)
            {
                fireTimer = Time.time;
                SpawnProjectile();
            }
        }
    }

    protected void SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.localPosition, projectilePrefab.transform.rotation);
        projectile.GetComponent<Projectile>().direction = (int)transform.localScale.x;
        var rotation = transform.rotation.eulerAngles;
        projectile.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z+projectile.transform.rotation.eulerAngles.z);
    }
    public void TakeDamage(Damage dmg)
    {
        health -= dmg.RawDamage;
        if (health == 0)
        {
            Death();
        }
    }

    protected void Death()
    {
        ShipManager.Instance.RemoveShip(gameObject);
        InputManager.Instance.DeselectShip(gameObject);
        Destroy(gameObject);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Hello - Mouse Enter");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPlayer)
        {
            var color = GetComponent<SpriteRenderer>().color;
            if (color.Equals(Color.white))
            {
                GetComponent<SpriteRenderer>().color = Color.cyan;
                InputManager.Instance.SelectShip(gameObject);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Hello - Mouse Exit");
    }
}
