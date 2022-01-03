using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacks", menuName = "Attacks/AttackDB")] [Serializable]
public class AttackDBScriptableObject : ScriptableObject
{
    [SerializeField] private List<AttackScriptableObject> attackDB;

    public AttackScriptableObject GetAttack(string attackName)
    {
        return attackDB.Find(x => x.AttackName == attackName);
    }
}