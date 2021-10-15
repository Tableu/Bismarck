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
        if (!_movement.FrontClear(1))
        {
            _movement.SetDirection(-_movement.GetDirection());
        }
        _movement.Move(_ship.transform.position+(Vector3.right*10*_movement.GetDirection()),_speed);
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
    private readonly GameObject _target;
    private float _speed;
    private float _rotationSpeed;

    public MoveToTargetState(ShipController ship, MovementController movement, GameObject target)
    {
        _ship = ship;
        _movement = movement;
        _target = target;
    }
    public void Tick()
    {
        _movement.Move(_target.transform.position,_speed);
        _movement.RotateTowards(_target.transform,_rotationSpeed);
    }

    public void OnEnter()
    {
        _speed = _movement.BaseSpeed;
        _rotationSpeed = _movement.RotationSpeed;
    }

    public void OnExit()
    {
        
    }
}
