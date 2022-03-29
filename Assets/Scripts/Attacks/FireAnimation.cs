using System;
using UnityEngine;

namespace Attacks
{
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

        public override void Initialize(Vector2 direction, float speed, Sprite sprite, Action callback)
        {
            Direction = direction;
            Speed = speed;
            SpriteRenderer.sprite = sprite;
            OnAnimationFinish += callback;
        }

        public override void Finish()
        {
            RaiseAnimationFinish();
        }
    }
}