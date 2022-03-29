using System;
using UnityEngine;

namespace Attacks
{
    public abstract class AttackAnimation : MonoBehaviour
    {
        public event Action OnAnimationFinish;
        public float Speed;
        public Vector2 Direction;
        public float MaxDistance;
        public SpriteRenderer SpriteRenderer;

        public abstract void Initialize(Vector2 direction, float speed, Sprite sprite, Action callback);

        protected void RaiseAnimationFinish()
        {
            if (this != null && OnAnimationFinish != null)
            {
                OnAnimationFinish.Invoke();
            }
        }

        public abstract void Finish();
    }
}