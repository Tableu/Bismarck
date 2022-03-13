using System.Collections;
using Ships.Components;
using UnityEngine;

namespace Attacks
{
    public class AttackIcon : MonoBehaviour
    {
        public AttackProjectile AttackProjectile;
        public ShipStats Attacker;
        public DamageableComponent Target;
        [SerializeField] private SpriteRenderer SpriteRenderer;
        private Vector2 _direction;

        private void Start()
        {
            SpriteRenderer.enabled = false;
            GameObject Fire = Instantiate(AttackProjectile.AbilityData.FireAnimation,
                Attacker.Visuals.transform);
            AttackAnimation animation = Fire.GetComponent<AttackAnimation>();
            _direction = (Target.ShipStats.MapIcon.transform.position - Attacker.MapIcon.transform.position)
                .normalized;
            if (animation != null)
            {
                animation.Initialize(_direction, AttackProjectile.AbilityData.BaseInfoWindowSpeed,
                    AttackProjectile.AbilityData.InfoWindowSprite, StartTravel);
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
                    Vector2.MoveTowards(transform.position, target, AttackProjectile.AbilityData.BaseMapSpeed);
                yield return null;
            }

            if (AttackProjectile.Hit())
            {
                SpriteRenderer.enabled = false;
                GameObject hit = Instantiate(AttackProjectile.AbilityData.HitAnimation,
                    Target.ShipStats.Visuals.transform);
                AttackAnimation animation = hit != null ? hit.GetComponent<AttackAnimation>() : null;
                if (animation != null)
                {
                    animation.Initialize(_direction, AttackProjectile.AbilityData.BaseInfoWindowSpeed,
                        AttackProjectile.AbilityData.InfoWindowSprite, delegate
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
                GameObject miss = Instantiate(AttackProjectile.AbilityData.MissAnimation,
                    Target.ShipStats.Visuals.transform);
                AttackAnimation animation = miss != null ? miss.GetComponent<AttackAnimation>() : null;
                if (animation != null)
                {
                    animation.Initialize(_direction, AttackProjectile.AbilityData.BaseInfoWindowSpeed,
                        AttackProjectile.AbilityData.InfoWindowSprite, delegate { Destroy(gameObject); });
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