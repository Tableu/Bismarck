using System.Collections.Generic;
using UnityEngine;

public class ShipLogic : MonoBehaviour
{
    protected MovementController _movementController;
    protected List<AttackCommand> _attackCommands;
    public ShipSpawner ShipSpawner;
    private List<AttackScriptableObject> attackScriptableObjects;
    private bool blocksMovement;
    [Header("Objects")]
    [SerializeField] protected GameObject target;
    [SerializeField] public List<Transform> turretPositions;

    protected MoveToTargetState _moveToTarget;
    protected MoveToPositionState _moveToPosition;
    protected MoveForwardState _moveForward;
    protected FSM StateMachine;
    public bool BlocksMovement => blocksMovement;

    protected void Start()
    {
        var shipData = ShipSpawner.ShipDictionary.GetShip(gameObject.GetInstanceID());
        blocksMovement = shipData.BlocksMovement;
        _movementController = new MovementController(gameObject, shipData.Speed, 0, ShipSpawner.LayerMask);
        _moveToTarget = new MoveToTargetState(this, _movementController, target);
        _moveToPosition = new MoveToPositionState(this, _movementController, Vector2.zero);
        _moveForward = new MoveForwardState(this, _movementController);
        StateMachine = new FSM();
        attackScriptableObjects = shipData.Weapons;
        _attackCommands = new List<AttackCommand>();
        List<Transform>.Enumerator turretPos = turretPositions.GetEnumerator();
        foreach (AttackScriptableObject attackScriptableObject in attackScriptableObjects)
        {
            if (!turretPos.MoveNext())
            {
                break;
            }
            AttackCommand attackCommand = attackScriptableObject.MakeAttack();
            StartCoroutine(attackCommand.DoAttack(gameObject, turretPos.Current));
            attackCommand.SetParent(ShipSpawner.ProjectileParent);
            _attackCommands.Add(attackCommand);
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        StateMachine.Tick();
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
        var shipData = ShipSpawner.ShipDictionary.GetShip(gameObject.GetInstanceID());
        if (_moveToTarget.Target == null || Vector2.Distance(transform.position, _moveToTarget.Target.transform.position) < shipData.StopDistance)
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
        var shipData = ShipSpawner.ShipDictionary.GetShip(gameObject.GetInstanceID());
        if (_moveToTarget.Target != null)
        {
            return false;
        }
        GameObject enemy = DetectionController.DetectShip(shipData.AggroRange, gameObject);
        if (enemy != null)
        {
            _moveToTarget.Target = enemy;
            foreach (AttackCommand command in _attackCommands)
            {
                command.SetTarget(enemy);
                StartCoroutine(command.DoAttack(gameObject));
            }
            return true;
        }
        return false;
    }
}
