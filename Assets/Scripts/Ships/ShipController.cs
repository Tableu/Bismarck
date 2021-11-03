using UnityEngine;

public class ShipController : MonoBehaviour, IDamageable
{
    protected MovementController _movementController;
    protected AttackCommand _attackCommand;
    [SerializeField] protected AttackScriptableObject attackScriptableObject;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector2 moveTarget;
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool blocksMovement;
    [SerializeField] private bool move = true;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject healthBarPrefab;
    private HealthBar _healthBar;
    protected FSM StateMachine;
    public bool BlocksMovement => blocksMovement;

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
        _attackCommand = attackScriptableObject.MakeAttack();
        StartCoroutine(_attackCommand.DoAttack(gameObject));
        
        GameObject healthBars = GameObject.Find("HealthBars");
        GameObject healthBar = Instantiate(healthBarPrefab, healthBars.transform);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Init(transform,health, health, 2f);
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        StateMachine.Tick();
        _healthBar.SetHealth(health);

        if (move)
        {
            if (!_movementController.Move(moveTarget, speed))
            {
                move = false;
            }
        }
    }
    
    public void TakeDamage(Damage dmg)
    {
        health -= dmg.RawDamage;
        if (health == 0)
        {
            Death();
        }
    }

    public bool DestroyProjectile(CollisionType type)
    {
        return true;
    }

    protected void Death()
    {
        ShipManager.Instance.RemoveShip(gameObject);
        InputManager.Instance.DeselectShip(gameObject);
        _attackCommand.StopAttack();
        Destroy(_healthBar.gameObject);
        Destroy(gameObject);
    }

    public void Highlight()
    {
        var color = GetComponent<SpriteRenderer>().color;
        if (color.Equals(Color.white))
        {
            GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else
        { 
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
