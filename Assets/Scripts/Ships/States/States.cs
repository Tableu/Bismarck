using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardState : IState
{
    private readonly ShipController _ship;
    private readonly MovementController _movement;
    private float _speed;

    public MoveForwardState(ShipController ship, MovementController movement)
    {
        _ship = ship;
        _movement = movement;
    }
    public void Tick()
    {
        if (_movement.DirectionClear(_ship.transform.forward,1))
        {
            _movement.Move(_ship.transform.position+(Vector3.right*10*_movement.GetDirection()),_speed);
        }
    }

    public void OnEnter()
    {
        _speed = _movement.BaseSpeed;
    }

    public void OnExit()
    {
        
    }
}

public class MoveToTargetState : IState
{
    private readonly ShipController _ship;
    private readonly MovementController _movement;
    public GameObject Target { get; set; }
    private float _speed;
    private float _rotationSpeed;

    public MoveToTargetState(ShipController ship, MovementController movement, GameObject target)
    {
        _ship = ship;
        _movement = movement;
        Target = target;
    }
    public void Tick()
    {
        if (_movement.DirectionClear(Target.transform.position - _ship.transform.position,1))
        {
            _movement.Move(Target.transform.position,_speed);
        }
        //_movement.RotateTowards(_target.transform,_rotationSpeed);
    }

    public void OnEnter()
    {
        _speed = _movement.BaseSpeed;
        Vector3 diff = Target.transform.position - _ship.transform.position;
        _movement.SetDirection((int)Mathf.Sign(diff.x));
        //_rotationSpeed = _movement.RotationSpeed;
    }

    public void OnExit()
    {
        
    }
}
