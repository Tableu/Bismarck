using UnityEngine;

public class ShipController : MonoBehaviour, IDamageable
{
    protected MovementController _movementController;
    protected AttackCommand _attackCommand;
    [SerializeField] protected AttackScriptableObject attackScriptableObject;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] public GameObject target;
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool blocksMovement;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] protected float aggroRange;
    private HealthBar _healthBar;
    protected MoveToTargetState _moveToTarget;
    protected MoveToPositionState _moveToPosition;
    protected MoveForwardState _moveForward;
    protected FSM StateMachine;
    public bool BlocksMovement => blocksMovement;

    private void Awake()
    {
        _movementController = new MovementController(gameObject, speed, rotationSpeed, layerMask);
    }

    protected void Start()
    {
        _moveToTarget = new MoveToTargetState(this, _movementController, target);
        _moveToPosition = new MoveToPositionState(this, _movementController, Vector2.zero);
        _moveForward = new MoveForwardState(this, _movementController);
        StateMachine = new FSM();
        
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
        if (_moveToTarget.Target == null || Vector2.Distance(transform.position, _moveToTarget.Target.transform.position) < .5f)
        {
            return true;
        }
        return false;
    }

    protected bool HasReachedPosition()
    {
        if (Vector2.Distance(transform.position, _moveToPosition.Position) < .5f)
        {
            return true;
        }
        return false;
    }

    protected bool DetectEnemy()
    {
        GameObject enemy = DetectionController.DetectShip(aggroRange, gameObject);
        if (enemy != null)
        {
            _moveToTarget.Target = enemy;
            return true;
        }
        return false;
    }
}
