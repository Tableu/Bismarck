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
    public int Speed;
    public float StopDistance;
    public float AggroRange;
    public LayerMask LayerMask;
    public bool BlocksMovement;
    public List<AttackScriptableObject> Weapons;
    public List<ScriptableObject> Modules;
    public List<ScriptableObject> Buffs;
    public Vector2 StartingPos;
    public GameObject ShipPrefab;
    public int RepairCost => (MaxHealth - Health) * 100;
    public int SellValue => Mathf.Max(Cost - RepairCost, 0);
}