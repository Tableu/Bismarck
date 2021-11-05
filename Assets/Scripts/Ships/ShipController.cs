using UnityEngine;

public class ShipController : MonoBehaviour, IDamageable
{
    protected MovementController _movementController;
    protected AttackCommand _attackCommand;
    [SerializeField] protected AttackScriptableObject attackScriptableObject;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject target;
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool blocksMovement;
    [SerializeField] private bool move = true;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private float aggroRange;
    private HealthBar _healthBar;
    private MoveToTargetState _moveToTarget;
    private MoveToPositionState _moveToPosition;
    protected FSM StateMachine;
    public bool BlocksMovement => blocksMovement;

    private void Awake()
    {
        _movementController = new MovementController(gameObject, speed, rotationSpeed, layerMask);
    }

    protected void Start()
    {
        var moveForward = new MoveForwardState(this, _movementController);
        _moveToTarget = new MoveToTargetState(this, _movementController, target);
        _moveToPosition = new MoveToPositionState(this, _movementController, Vector2.zero);
        StateMachine = new FSM();
        StateMachine.SetState(moveForward);
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
    public void MoveToPosition(Vector2 position)
    {
        _moveToPosition.Position = position;
        StateMachine.SetState(_moveToPosition);
    }

    public void MoveToTarget(GameObject target)
    {
        _moveToTarget.Target = target;
        StateMachine.SetState(_moveToTarget);
    }
    protected bool HasReachedTarget()
    {
        if (target == null)
        {
            target = DetectionController.DetectShip(aggroRange, gameObject);
            if (target != null)
            {
                _moveToTarget.Target = target;
                _moveToTarget.OnEnter();
            }
        }
        if (target == null || Vector2.Distance(transform.position, target.transform.position) < .5f)
        {
            return true;
        }
        return false;
    }
}
