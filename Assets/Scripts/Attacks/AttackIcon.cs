using System.Collections;
using Ships.Components;
using UnityEngine;

namespace Attacks
{
    public class AttackIcon : MonoBehaviour
    {
        public AttackProjectile AttackProjectile;
        public DamageableComponent Attacker;
        public DamageableComponent Target;
        [SerializeField] private SpriteRenderer SpriteRenderer;
        private Vector2 _direction;

        private void Start()
        {
            SpriteRenderer.enabled = false;
            GameObject Fire = Instantiate(AttackProjectile.AttackData.FireAnimation,
                Attacker.ShipStats.Visuals.transform);
            AttackAnimation animation = Fire.GetComponent<AttackAnimation>();
            _direction = (Target.ShipStats.MapIcon.transform.position - Attacker.ShipStats.MapIcon.transform.position)
                .normalized;
            if (animation != null)
            {
                animation.Initialize(_direction, AttackProjectile.AttackData.BaseInfoWindowSpeed,
                    AttackProjectile.AttackData.InfoWindowSprite, StartTravel);
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
                transform.position =
                    Vector2.MoveTowards(transform.position, target, AttackProjectile.AttackData.BaseMapSpeed);
                yield return null;
            }

            if (AttackProjectile.Hit())
            {
                SpriteRenderer.enabled = false;
                GameObject hit = Instantiate(AttackProjectile.AttackData.HitAnimation,
                    Target.ShipStats.Visuals.transform);
                AttackAnimation animation = hit != null ? hit.GetComponent<AttackAnimation>() : null;
                if (animation != null)
                {
                    animation.Initialize(_direction, AttackProjectile.AttackData.BaseInfoWindowSpeed,
                        AttackProjectile.AttackData.InfoWindowSprite, delegate
                        {
                            AttackProjectile.ApplyDamage();
                            Destroy(gameObject);
                        });
                    animation.transform.localPosition = -2 * _direction;
                }
                else
                {
                    AttackProjectile.ApplyDamage();
                    Destroy(gameObject);
                }
            }
            else
            {
                SpriteRenderer.enabled = false;
                GameObject miss = Instantiate(AttackProjectile.AttackData.MissAnimation,
                    Target.ShipStats.Visuals.transform);
                AttackAnimation animation = miss != null ? miss.GetComponent<AttackAnimation>() : null;
                if (animation != null)
                {
                    animation.Initialize(_direction, AttackProjectile.AttackData.BaseInfoWindowSpeed,
                        AttackProjectile.AttackData.InfoWindowSprite, delegate { Destroy(gameObject); });
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