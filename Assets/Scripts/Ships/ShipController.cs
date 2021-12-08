using System.Collections.Generic;
using UnityEngine;
public class ShipController : MonoBehaviour, IDamageable
{
    protected MovementController _movementController;
    protected List<AttackCommand> _attackCommands = new List<AttackCommand>();
    [SerializeField] protected List<AttackScriptableObject> attackScriptableObjects;
    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private int cost;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] protected float aggroRange;
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool blocksMovement;
    [SerializeField] private LayerMask layerMask;
    [Header("Objects")]
    [SerializeField] public GameObject target;
    [SerializeField] private GameObject healthBarPrefab;
    private HealthBar _healthBar;
    private Vector2? _fleetScreenPos = null;
    protected MoveToTargetState _moveToTarget;
    protected MoveToPositionState _moveToPosition;
    protected MoveForwardState _moveForward;
    protected FSM StateMachine;
    public bool BlocksMovement => blocksMovement;
    public int Cost => cost;

    #region Shop Functions
    public Vector2 positionOffset
    {
        set;
        get;
    }

    public int RepairCost()
    {
        return (maxHealth-health)*100;
    }

    public int SellCost()
    {
        return cost - RepairCost();
    }

    public void Repair()
    {
        health = maxHealth;
        _healthBar.SetHealth(health);
    }
    #endregion

    private void Awake()
    {
        _movementController = new MovementController(gameObject, speed, rotationSpeed, layerMask, stopDistance);
    }

    protected void Start()
    {
        _moveToTarget = new MoveToTargetState(this, _movementController, target);
        _moveToPosition = new MoveToPositionState(this, _movementController, Vector2.zero);
        _moveForward = new MoveForwardState(this, _movementController);
        StateMachine = new FSM();
        
        ShipManager.Instance.AddShip(gameObject);
        foreach (AttackScriptableObject attackScriptableObject in attackScriptableObjects)
        {
            _attackCommands.Add(attackScriptableObject.MakeAttack());
        }
        GameObject healthBars = GameObject.Find("HealthBars");
        GameObject healthBar = Instantiate(healthBarPrefab, healthBars.transform);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Init(transform, maxHealth, health, 2f);
        
        SaveFleetScreenPosition();
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

    private void OnDestroy()
    {
        if(_healthBar != null && _healthBar.gameObject != null)
        {
            Destroy(_healthBar.gameObject);
        }
        ShipManager.Instance.RemoveShip(gameObject);
        InputManager.Instance.DeselectShip(gameObject);
    }

    protected void Death()
    {
        Destroy(gameObject);
    }

    public void Highlight()
    {
        GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    public void DeHighlight()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
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
            foreach (AttackCommand command in _attackCommands)
            {
                command.SetTarget(enemy);
            }
            return true;
        }
        return false;
    }

    public void SaveFleetScreenPosition()
    {
        _fleetScreenPos = transform.position;
        foreach (AttackCommand command in _attackCommands)
        {
            StartCoroutine(command.DoAttack(gameObject));
        }
        DetectEnemy();
    }

    public void SetFleetScreenPosition()
    {
        if(_fleetScreenPos != null)
            transform.position = _fleetScreenPos.Value;
    }
}
