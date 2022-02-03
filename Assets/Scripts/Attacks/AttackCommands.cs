using System;
using System.Collections;
using UnityEngine;

public interface AttackCommand
{
    public IEnumerator DoAttack(GameObject attacker, Transform spawnPosition = null);
    public void StopAttack();
    public void SetTarget(GameObject target);
    public void SetParent(Transform parent);
}

[Serializable]
public abstract class AttackScriptableObject : ScriptableObject
{
    public GameObject Turret;
    public string AttackName;
    public int Cost;
    public abstract AttackCommand MakeAttack();
}
