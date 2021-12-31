using System.Collections;
using UnityEngine;

public interface AttackCommand
{
    public IEnumerator DoAttack(GameObject attacker, GameObject turret = null);
    public void StopAttack();
    public void SetTarget(GameObject target);
}

public abstract class AttackScriptableObject : ScriptableObject
{
    public GameObject Turret;
    public abstract AttackCommand MakeAttack();
}