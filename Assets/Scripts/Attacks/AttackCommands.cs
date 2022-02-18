using System;
using Ships.Components;
using Systems.Modifiers;
using UnityEngine;

public interface AttackCommand
{
    public ModifiableStat HitChanceMultiplier { get; }
    public bool DoAttack(IDamageable target);
    public void SetParent(Transform parent);
}

[Serializable]
public abstract class AttackScriptableObject : ScriptableObject
{
    public GameObject Turret;
    public string AttackName;
    public int Cost;
    public float HitChance;
    public int Damage;

    public abstract AttackCommand MakeAttack(ShipInfo shipInfo);
}