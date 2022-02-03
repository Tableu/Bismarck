using System.Collections.Generic;
using Ships.Components;
using Ships.DataManagement;
using Ships.Fleets;
using UnityEngine;

public class ShipLogic : MonoBehaviour
{
    [Header("Objects")] [SerializeField] protected GameObject target;

    [SerializeField] public List<Transform> turretPositions;

    [SerializeField] private string tag;
    protected List<AttackCommand> _attackCommands;
    protected ShipData _data;
    protected MoveForwardState _moveForward;
    protected MovementController _movementController;
    protected MoveToPositionState _moveToPosition;
    protected MoveToTargetState _moveToTarget;
    protected FleetManager _shipSpawner;
    private List<AttackScriptableObject> attackScriptableObjects;
    protected FSM StateMachine;
    public bool BlocksMovement { get; private set; }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        StateMachine.Tick();
    }

    public void Initialize(ShipData data, FleetManager spawner)
    {
        _shipSpawner = spawner;
        _data = data;
        BlocksMovement = data.BlocksMovement;
        var stats = GetComponent<ShipInfo>();
        Debug.Assert(stats != null, "Ship missing stats component");
        _movementController = new MovementController(gameObject, stats.SpeedMultiplier, 0, LayerMask.GetMask("EnemyShips"));
        _moveToTarget = new MoveToTargetState(this, _movementController, target);
        _moveToPosition = new MoveToPositionState(this, _movementController, Vector2.zero);
        _moveForward = new MoveForwardState(this, _movementController);
        StateMachine = new FSM();
        attackScriptableObjects = data.Weapons;
        _attackCommands = new List<AttackCommand>();
        if (!enabled)
        {
            return;
        }
        var turretPos = turretPositions.GetEnumerator();

        var parent = GameObject.FindWithTag(tag) ?? new GameObject
        {
            tag = tag
        };

        foreach (var attackScriptableObject in attackScriptableObjects)
        {
            if (!turretPos.MoveNext())
            {
                break;
            }

            var attackCommand = attackScriptableObject.MakeAttack();
            StartCoroutine(attackCommand.DoAttack(gameObject, turretPos.Current));
            attackCommand.SetParent(parent.transform);
            _attackCommands.Add(attackCommand);
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
        if (_moveToTarget.Target == null ||
            Vector2.Distance(transform.position, _moveToTarget.Target.transform.position) < _data.TargetRange)
        {
            return true;
        }

        return false;
    }

    protected bool HasReachedPosition()
    {
        DetectEnemy();
        if (Vector2.Distance(transform.position, _moveToPosition.Position) < .5f)
        {
            return true;
        }

        return false;
    }

    protected bool DetectEnemy()
    {
        if (_moveToTarget.Target != null)
        {
            return false;
        }

        var enemy = DetectionController.DetectShip(_data.SensorRange, gameObject);
        if (enemy != null)
        {
            _moveToTarget.Target = enemy;
            foreach (var command in _attackCommands)
            {
                command.SetTarget(enemy);
                StartCoroutine(command.DoAttack(gameObject));
            }

            return true;
        }

        return false;
    }
}
