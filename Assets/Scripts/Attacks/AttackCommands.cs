using System.Collections;
using UnityEngine;

public interface AttackCommand
{
    public IEnumerator DoAttack(GameObject attacker);
    public void StopAttack();
    public void SetTarget(GameObject target);
}

public abstract class AttackScriptableObject : ScriptableObject
{
    public abstract AttackCommand MakeAttack();
}