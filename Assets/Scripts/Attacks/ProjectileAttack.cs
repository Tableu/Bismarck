using Ships.Components;
using Systems.Modifiers;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Attacks/Projectile", order = 0)]
public class ProjectileAttack : AttackScriptableObject
{
    public GameObject mapIcon;
    public float fireDelay;

    public override AttackCommand MakeAttack(ShipInfo shipInfo)
    {
        return new Attack(shipInfo, mapIcon, fireDelay, HitChance, Damage);
    }

    private class Attack : AttackCommand
    {
        private ShipInfo _shipInfo;
        private float _fireDelay;
        private float _fireTime;
        private float _damage;
        private Transform _parent;
        private GameObject _mapIcon;

        public ModifiableStat HitChanceMultiplier { get; } = new ModifiableStat(0);

        public Attack(ShipInfo shipInfo, GameObject mapIcon, float fireDelay, float hitChance, float damage)
        {
            _shipInfo = shipInfo;
            _mapIcon = mapIcon;
            _fireDelay = fireDelay;
            _fireTime = Time.deltaTime;
            _damage = damage;
            HitChanceMultiplier.UpdateBaseValue(hitChance);
        }

        public void SetParent(Transform parent)
        {
            _parent = parent;
        }

        public bool DoAttack(IDamageable target)
        {
            if (target != null)
            {
                if (Time.deltaTime - _fireTime > _fireDelay)
                {
                    _fireTime = Time.deltaTime;
                    SpawnProjectile(target);
                    return true;
                }
            }

            return false;
        }

        private void SpawnProjectile(IDamageable target)
        {
            Damage damage = new Damage(target, _damage, HitChanceMultiplier);
            GameObject mapIcon = Instantiate(_mapIcon, _shipInfo.transform.position, Quaternion.identity);
            AttackIcon attackIcon = mapIcon.GetComponent<AttackIcon>();
            attackIcon.Damage = damage;
            attackIcon.Attacker = _shipInfo;
            attackIcon.Target = target.ShipInfo;    
        }
    }
}
