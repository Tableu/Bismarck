using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShipData
{
    public string ShipName;
    public int Cost;
    public int Health;
    public int MaxHealth;
    public float Speed;
    public float StopDistance;
    public float AggroRange;
    public bool BlocksMovement;
    public List<AttackScriptableObject> Weapons;
    public List<ScriptableObject> Modules;
    public List<ScriptableObject> Buffs;
    public Vector2 StartingPos;
    public int ShipDirection;
    public GameObject ShipPrefab;
    public int RepairCost => (MaxHealth - Health) * 100;
    public int SellValue => Mathf.Max(Cost - RepairCost, 0);

    public ShipData Copy()
    {
        ShipData clone = MemberwiseClone() as ShipData;
        clone.Weapons = new List<AttackScriptableObject>(Weapons);
        clone.Modules = new List<ScriptableObject>(Modules);
        clone.Buffs = new List<ScriptableObject>(Buffs);
        clone.StartingPos = new Vector2(StartingPos.x, StartingPos.y);
        return clone;
    }
}