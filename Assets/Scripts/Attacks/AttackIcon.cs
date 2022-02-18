using System;
using System.Collections;
using Ships.Components;
using UnityEngine;

public class AttackIcon : MonoBehaviour
{
    public Damage Damage;
    public ShipInfo Attacker;
    public ShipInfo Target;
    public InfoWindowAttackAnimation AttackAnimation;

    private void Start()
    {
        StartCoroutine(AttackAnimation.Fire(Attacker, StartTravel));
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
            Vector2.MoveTowards(transform.position, target, 1);
            yield return null;
        }

        if (Damage.Hit())
        {
            StartCoroutine(AttackAnimation.Hit(Target, delegate
            {
                Damage.ApplyDamage();
                Destroy(gameObject);
            }));
        }
        else
        {
            StartCoroutine(AttackAnimation.Miss(Target, delegate { Destroy(gameObject); }));
        }
    }
}

public interface InfoWindowAttackAnimation
{
    public IEnumerator Fire(ShipInfo attacker, Action callback);
    public IEnumerator Hit(ShipInfo target, Action callback);
    public IEnumerator Miss(ShipInfo target, Action callback);
}