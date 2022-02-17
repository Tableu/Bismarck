using System;
using System.Collections;
using Ships.Components;
using UnityEngine;

public interface AttackCommand
{
    public bool DoAttack(GameObject attacker, Transform spawnPosition = null);
    public void SetTarget(ShipInfo target);
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
