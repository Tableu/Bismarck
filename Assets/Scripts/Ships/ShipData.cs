using System.Collections.Generic;
using UnityEngine;

public class ShipData : ScriptableObject
{
    public int health;
    public int maxHealth;
    public List<AttackScriptableObject> weapons;
    public List<ScriptableObject> modules;
    public List<ScriptableObject> buffs;
    public Vector2 startingPos;
    public GameObject shipPrefab;
}