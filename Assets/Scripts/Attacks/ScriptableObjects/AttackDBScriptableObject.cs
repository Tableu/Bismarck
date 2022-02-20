using System;
using System.Collections.Generic;
using UnityEngine;

namespace Attacks
{
    [CreateAssetMenu(fileName = "Attacks", menuName = "Attacks/AttackDB")]
    [Serializable]
    public class AttackDBScriptableObject : ScriptableObject
    {
        [SerializeField] private List<AttackData> attackDB;

        public AttackData GetAttack(string attackName)
        {
            return attackDB.Find(x => x.AttackName == attackName);
        }
    }
}