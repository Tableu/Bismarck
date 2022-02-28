using System;
using System.Collections;
using Attacks;
using Ships.Components;
using UnityEngine;

namespace Attacks
{
    public class ProjectileAttackAnimation : InfoWindowAttackAnimation
    {
        public IEnumerator Fire(DamageableComponentInfo attacker, Action callback)
        {
            throw new NotImplementedException();
        }

        public IEnumerator Hit(DamageableComponentInfo target, Action callback)
        {
            throw new NotImplementedException();
        }

        public IEnumerator Miss(DamageableComponentInfo target, Action callback)
        {
            throw new NotImplementedException();
        }
    }
}