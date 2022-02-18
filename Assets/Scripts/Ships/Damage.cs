using UnityEngine;

public struct Damage
{
    private IDamageable _target;
    private float _rawDamage;
    private float _hitChance;

    public float RawDamage => _rawDamage;

    public Damage(IDamageable target, float rawDamage, float hitChance)
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
            if (hit > (100 - _hitChance) + _target.DodgeChance)
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