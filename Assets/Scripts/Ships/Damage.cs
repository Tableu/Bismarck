using Ships.Components;
using UnityEngine;

public class Damage
{
    private DamageableComponentInfo _target;
    private float _rawDamage;
    private float _hitChance;

    public float RawDamage => _rawDamage;
    public float HitChance => _hitChance;

    public Damage(DamageableComponentInfo target, float rawDamage, float hitChance)
    {
        _target = target;
        _rawDamage = rawDamage;
        _hitChance = hitChance;
    }

    public bool Hit()
    {
        if (_target != null)
        {
            float hit = Random.Range(0f, 1f);
            if (hit > (1 - _hitChance) + _target.DodgeChance)
            {
                return true;
            }
        }

        return false;
    }

    public void ApplyDamage()
    {
        if (_target != null)
        {
            _target.TakeDamage(this);
        }
    }
}