using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShopItem : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int cost;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    private HealthBar _healthBar;
    public int Cost => cost;
    public Vector2 positionOffset
    {
        set;
        get;
    }

    public int RepairCost()
    {
        return (maxHealth-health)*100;
    }

    public int SellCost()
    {
        return cost - RepairCost();
    }

    public void Repair()
    {
        health = maxHealth;
        _healthBar.SetHealth(health);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Highlight()
    {
        GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    public void DeHighlight()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
