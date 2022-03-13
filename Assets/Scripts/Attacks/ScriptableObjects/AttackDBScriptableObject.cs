using System;
using System.Collections.Generic;
using Systems.Abilities;
using UnityEngine;

namespace Attacks
{
    [CreateAssetMenu(fileName = "Attacks", menuName = "Attacks/AttackDB")]
    [Serializable]
    public class AttackDBScriptableObject : ScriptableObject
    {
        [SerializeField] private List<AbilityData> attackDB;

        public AbilityData GetAttack(string attackName)
        {
            return attackDB.Find(x => x.AttackName == attackName);
        }
    }
}