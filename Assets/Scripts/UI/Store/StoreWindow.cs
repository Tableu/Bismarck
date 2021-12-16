using UnityEngine;
using UnityEngine.UI;

public class StoreWindow : MonoBehaviour
{
    [SerializeField] private ShipSpawner spawner;
    [SerializeField] private int money;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text sellText;
    [SerializeField] private Text repairText;
    private ISpawner _spawner;

    private void Start()
    {
        moneyText.text = money.ToString();
        _spawner = spawner.MakeSpawner(gameObject.transform);
    }
    public bool Buy(int cost)
    {
        if (money - cost >= 0)
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
