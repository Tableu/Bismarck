using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    private static StoreManager _instance;
    [SerializeField] private int money;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text sellText;
    [SerializeField] private Text repairText;

    public static StoreManager Instance
    {
        get { return _instance; }
    }
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    private void Start()
    {
        moneyText.text = money.ToString();
    }
    public bool Buy(int cost)
    {
        if (money - cost > 0)
        {
            money -= cost;
            moneyText.text = money.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Sell(int cost)
    {
        money += cost;
        moneyText.text = money.ToString();
    }

    public void Repair(int cost)
    {
        money -= cost;
        moneyText.text = money.ToString();
    }

    public void UpdateSellText(int cost)
    {
        sellText.text = cost.ToString();
    }

    public void UpdateRepairText(int cost)
    {
        repairText.text = cost.ToString();
    }
}
