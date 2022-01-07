using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    public Vector2 StartingPos;
    public GameObject ShipPrefab;
    public int RepairCost => (MaxHealth - Health) * 100;
    public int SellValue => Mathf.Max(Cost - RepairCost, 0);
    public ShipData Copy()
    {
        ShipData clone = MemberwiseClone() as ShipData;
        clone.Weapons = new List<AttackScriptableObject>(Weapons);
        clone.StartingPos = new Vector2(StartingPos.x, StartingPos.y);
        return clone;
    }
}

[Serializable]
public class ShipSaveData
{
    public string ShipName;
    public int Cost;
    public int Health;
    public int MaxHealth;
    public float Speed;
    public float StopDistance;
    public float AggroRange;
    public bool BlocksMovement;
    public List<string> Weapons;
    public float[] StartingPos = new float[2];

    public void Init(ShipData shipData)
    {
        ShipName = shipData.ShipName;
        Cost = shipData.Cost;
        Health = shipData.Health;
        MaxHealth = shipData.MaxHealth;
        Speed = shipData.Speed;
        StopDistance = shipData.StopDistance;
        AggroRange = shipData.AggroRange;
        BlocksMovement = shipData.BlocksMovement;
        Weapons = shipData.Weapons.Select(o => o.AttackName).ToList();
        StartingPos[0] = shipData.StartingPos.x;
        StartingPos[1] = shipData.StartingPos.y;
    }

    public ShipData Convert(AttackDBScriptableObject attackDB, ShipDBScriptableObject shipDB)
    {
        ShipData shipData = new ShipData();
        shipData.ShipName = ShipName;
        shipData.Cost = Cost;
        shipData.Health = Health;
        shipData.MaxHealth = MaxHealth;
        shipData.Speed = Speed;
        shipData.StopDistance = StopDistance;
        shipData.AggroRange = AggroRange;
        shipData.BlocksMovement = BlocksMovement;
        
        shipData.Weapons = new List<AttackScriptableObject>();
        foreach (string weapon in Weapons)
        {
            AttackScriptableObject attack = attackDB.GetAttack(weapon);
            if (attack != null)
            {
                shipData.Weapons.Add(attack);
            }
        }

        shipData.StartingPos = new Vector2(StartingPos[0], StartingPos[1]);

        ShipDataScriptableObject shipDataScriptableObject = shipDB.GetShip(ShipName);
        if (shipDataScriptableObject != null)
        {
            shipData.ShipPrefab = shipDataScriptableObject.ShipData.ShipPrefab;
        }

        return shipData;
    }
}