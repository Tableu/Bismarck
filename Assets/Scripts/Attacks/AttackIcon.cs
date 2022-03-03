using System;
using System.Collections;
using Ships.Components;
using UnityEngine;

namespace Attacks
{
    public class AttackIcon : MonoBehaviour
    {
        public AttackInfo AttackInfo;
        public DamageableComponentInfo Attacker;
        public DamageableComponentInfo Target;
        [SerializeField] private SpriteRenderer SpriteRenderer;
        private Vector2 _direction;

        private void Start()
        {
            SpriteRenderer.enabled = false;
            GameObject Fire = Instantiate(AttackInfo.AttackData.FireAnimation, Attacker.ShipInfo.Visuals.transform);
            AttackAnimation animation = Fire.GetComponent<AttackAnimation>();
            _direction = (Target.ShipInfo.MapIcon.transform.position - Attacker.ShipInfo.MapIcon.transform.position)
                .normalized;
            if (animation != null)
            {
                animation.OnAnimationFinish += StartTravel;
                animation.Direction = _direction;
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
            SpriteRenderer.enabled = true;
            var target = Target.transform.position;
            while (!transform.position.Equals(target))
            {
                transform.position = Vector2.MoveTowards(transform.position, target, 0.1f);
                yield return null;
            }

            if (AttackInfo.Hit())
            {
                SpriteRenderer.enabled = false;
                GameObject hit = Instantiate(AttackInfo.AttackData.HitAnimation, Target.ShipInfo.Visuals.transform);
                AttackAnimation animation = hit != null ? hit.GetComponent<AttackAnimation>() : null;
                if (animation != null)
                {
                    animation.OnAnimationFinish += delegate
                    {
                        AttackInfo.ApplyDamage();
                        Destroy(gameObject);
                    };
                    animation.Direction = _direction;
                    animation.transform.localPosition = -2 * _direction;
                }
                else
                {
                    AttackInfo.ApplyDamage();
                    Destroy(gameObject);
                }
            }
            else
            {
                SpriteRenderer.enabled = false;
                GameObject miss = Instantiate(AttackInfo.AttackData.MissAnimation, Target.ShipInfo.Visuals.transform);
                AttackAnimation animation = miss != null ? miss.GetComponent<AttackAnimation>() : null;
                if (animation != null)
                {
                    animation.OnAnimationFinish += delegate { Destroy(gameObject); };
                    animation.Direction = _direction;
                    animation.transform.localPosition = -2 * _direction;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}