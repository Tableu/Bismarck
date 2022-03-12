using Ships.Components;
using UnityEngine;

namespace Attacks
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Attacks/Projectile", order = 0)]
    public class ProjectileAttack : AttackData
    {
        public override Attack MakeAttack(DamageableComponent component)
        {
            return new Attack(this, component);
        }
    }
}