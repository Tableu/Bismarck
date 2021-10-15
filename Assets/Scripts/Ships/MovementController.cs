using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController
{
    private readonly BoxCollider2D _boxCollider;
    private readonly Transform _transform;
    
    private bool _inputLocked;
    private int _spriteForward = 1;
    private LayerMask _layerMask;
    public MovementController(GameObject go, float baseSpeed, float rotationSpeed, LayerMask layerMask)
    {
        _transform = go.transform;
        _boxCollider = go.GetComponent<BoxCollider2D>();
        BaseSpeed = baseSpeed;
        RotationSpeed = rotationSpeed;
        _layerMask = layerMask;
    }
    public Vector2 Position => _transform.position;
    public float BaseSpeed { get;}
    public float RotationSpeed { get; }
    public void Move(Vector2 target, float speed)
    {
        if (Vector2.Distance(_transform.position, target) > .1f)
        {
            _transform.position = Vector2.MoveTowards(_transform.position, target, speed);
        }
    }
    //https://answers.unity.com/questions/650460/rotating-a-2d-sprite-to-face-a-target-on-a-single.html
    public void RotateTowards(Transform target, float speed)
    {
        Vector3 vectorToTarget = target.position - _transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, q, Time.deltaTime * speed);
    }
    
    public bool FrontClear(float distance)
    {
        var hit = new RaycastHit2D[1];
        return _boxCollider.Raycast(new Vector2(GetDirection(), 0), hit, distance,
            _layerMask) <= 0;
    }
    public int GetDirection()
    {
        return (int) Mathf.Sign(_transform.localScale.x * _spriteForward);
    }

    public void SetDirection(int dir)
    {
        var localScale = _transform.localScale;
        localScale = new Vector3(Mathf.Abs(localScale.x) * dir * _spriteForward, localScale.y, localScale.z);
        _transform.localScale = localScale;
    }
}