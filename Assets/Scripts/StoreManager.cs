using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    private static StoreManager _instance;
    [SerializeField] private int money;
    [SerializeField] private Text moneyText;

    public static StoreManager Instance
    {
        get { return _instance; }
    }
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        _instance = this;
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

    public bool Sell(int cost)
    {
        money += cost;
        moneyText.text = money.ToString();
        return true;
    }
}
