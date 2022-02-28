using System;
using System.Collections;
using Ships.Components;
using UnityEngine;

namespace Attacks
{
    public class AttackIcon : MonoBehaviour
    {
        public Damage Damage;
        public DamageableComponentInfo Attacker;
        public DamageableComponentInfo Target;
        public InfoWindowAttackAnimation AttackAnimation;

        private void Start()
        {
            if (AttackAnimation != null)
            {
                StartCoroutine(AttackAnimation.Fire(Attacker, StartTravel));
            }
            else
            {
                StartTravel();
            }
        }

        private void StartTravel()
        {
            StartCoroutine(Travel());
        }

        private IEnumerator Travel()
        {
            var target = Target.transform.position;
            while (!transform.position.Equals(target))
            {
                transform.position = Vector2.MoveTowards(transform.position, target, 0.1f);
                yield return null;
            }

            if (Damage.Hit())
            {
                if (AttackAnimation != null)
                {
                    StartCoroutine(AttackAnimation.Hit(Target, delegate
                    {
                        Damage.ApplyDamage();
                        Destroy(gameObject);
                    }));
                }
                else
                {
                    Damage.ApplyDamage();
                    Destroy(gameObject);
                }
            }
            else
            {
                if (AttackAnimation != null)
                {
                    StartCoroutine(AttackAnimation.Miss(Target, delegate { Destroy(gameObject); }));
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    
    public interface InfoWindowAttackAnimation
    {
        public IEnumerator Fire(DamageableComponentInfo attacker, Action callback);
        public IEnumerator Hit(DamageableComponentInfo target, Action callback);
        public IEnumerator Miss(DamageableComponentInfo target, Action callback);
    }
}