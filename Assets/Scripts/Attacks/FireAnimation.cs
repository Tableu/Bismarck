using System;
using UnityEngine;

public class FireAnimation : AttackAnimation
{
    private float _distance;

    private void Start()
    {
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Direction);
        SpriteRenderer.transform.rotation = rotation;
    }

    private void Update()
    {
        transform.Translate(Direction * Speed);
        _distance += Speed;
        if (_distance > MaxDistance)
        {
            Finish();
            Destroy(gameObject);
        }
    }

    public override void Finish()
    {
        RaiseAnimationFinish();
    }
}