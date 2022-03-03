using System;
using UnityEngine;

public abstract class AttackAnimation : MonoBehaviour
{
    public event Action OnAnimationFinish;
    public float Speed;
    public Vector2 Direction;
    public float MaxDistance;
    public SpriteRenderer SpriteRenderer;

    protected void RaiseAnimationFinish()
    {
        if (this != null && OnAnimationFinish != null)
        {
            OnAnimationFinish.Invoke();
        }
    }

    public abstract void Finish();
}