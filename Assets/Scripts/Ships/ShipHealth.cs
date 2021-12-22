using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ShipHealth : MonoBehaviour,IDamageable, INotifyPropertyChanged
{
    public ShipListScriptableObject selectedShips;
    public ShipDictionary shipDict;
    [SerializeField] private GameObject healthBarPrefab;
    private int maxHealth;
    private int health;
    private HealthBar _healthBar;

    [Binding]
    public int Health
    {
        get => health;
        set
        {
            if (health == value)
            {
                return;
            }
            health = value;
            OnPropertyChanged("Health");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    private void Start()
    {
        GameObject healthBars = GameObject.Find("HealthBars");
        GameObject healthBar = Instantiate(healthBarPrefab, healthBars.transform);
        _healthBar = healthBar.GetComponent<HealthBar>();
        ShipData shipData = shipDict.GetShip(gameObject.GetInstanceID());
        maxHealth = shipData.MaxHealth;
        health = shipData.Health;
        _healthBar.Init(transform, shipData.MaxHealth, shipData.Health, 2f);
    }

    private void FixedUpdate()
    {
        _healthBar.SetHealth(health);
    }
    
    public void TakeDamage(Damage dmg)
    {
        health -= dmg.RawDamage;
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        if(_healthBar != null && _healthBar.gameObject != null)
        {
            Destroy(_healthBar.gameObject);
        }
        selectedShips.RemoveShip(gameObject);
        shipDict.RemoveShip(gameObject.GetInstanceID());
    }
    
    public bool DestroyProjectile(CollisionType type)
    {
        return true;
    }
}
