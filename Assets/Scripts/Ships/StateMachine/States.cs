using UnityEngine;

public class MoveForwardState : IState
{
    private readonly MovementController _movement;
    private readonly ShipLogic _shipBattle;
    private float _speed;

    public MoveForwardState(ShipLogic shipBattle, MovementController movement)
    {
        _shipBattle = shipBattle;
        _movement = movement;
    }

    public void Tick()
    {
        if (_movement.DirectionClear(_shipBattle.transform.forward, 1))
        {
            _movement.Move(_shipBattle.transform.position + (Vector3.right * 10 * _movement.GetDirection()));
        }
    }

    public void OnEnter()
    {
        _speed = _movement.Speed;
    }

    public void OnExit()
    {
    }
}

public class MoveToTargetState : IState
{
    private readonly MovementController _movement;
    private readonly ShipLogic _shipBattle;
    private float _rotationSpeed;
    private float _speed;

    public MoveToTargetState(ShipLogic shipBattle, MovementController movement, GameObject target)
    {
        _shipBattle = shipBattle;
        _movement = movement;
        Target = target;
    }

    public GameObject Target { get; set; }

    public void Tick()
    {
        if (Target == null)
            return;
        if (_movement.DirectionClear(Target.transform.position - _shipBattle.transform.position, 1))
        {
            _movement.Move(Target.transform.position);
        }
        //_movement.RotateTowards(_target.transform,_rotationSpeed);
    }

    public void OnEnter()
    {
        if (Target == null)
            return;
        _speed = _movement.Speed;
        Vector3 diff = Target.transform.position - _shipBattle.transform.position;
        _movement.SetDirection((int) Mathf.Sign(diff.x));
        //_rotationSpeed = _movement.RotationSpeed;
    }

    public void OnExit()
    {
    }
}

public class MoveToPositionState : IState
{
    private readonly MovementController _movement;
    private readonly ShipLogic _shipBattle;
    private float _rotationSpeed;
    private float _speed;

    public MoveToPositionState(ShipLogic shipBattle, MovementController movement, Vector2 position)
    {
        _shipBattle = shipBattle;
        _movement = movement;
        Position = position;
    }

    public Vector2 Position { get; set; }

    public void Tick()
    {
        if (_movement.DirectionClear(Position - (Vector2) _shipBattle.transform.position, 1))
        {
            _movement.Move(Position);
        }
        //_movement.RotateTowards(_target.transform,_rotationSpeed);
    }

    public void OnEnter()
    {
        if (Position == null)
            return;
        _speed = _movement.Speed;
        Vector3 diff = Position - (Vector2) _shipBattle.transform.position;
        _movement.SetDirection((int) Mathf.Sign(diff.x));
        //_rotationSpeed = _movement.RotationSpeed;
    }

    public void OnExit()
    {
    }
}