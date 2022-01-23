using System;
using Ships.Components;
using Ships.DataManagment;
using UnityEngine;

public class ShipHealth : MonoBehaviour, IDamageable, IInitializableComponent, ILoadableComponent
{
    public ShipList selectedShips;
    [SerializeField] private GameObject healthBarPrefab;
    private bool _healthDirty;
    private ShipStats _stats;

    public float Health => PercentHealth * _stats.MaxHealth;
    public float PercentHealth { get; private set; } = 1f;

    private void Start()
    {
        var healthBars = GameObject.Find("HealthBars");
        var healthBarGo = Instantiate(healthBarPrefab, healthBars.transform);
        var healthBar = healthBarGo.GetComponent<HealthBar>();
        healthBar.Bind(this);
    }

    private void Update()
    {
        if (_healthDirty)
        {
            _healthDirty = false;
            OnHealthChanged?.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (selectedShips != null) selectedShips.RemoveShip(gameObject);
    }

    public void TakeDamage(Damage dmg)
    {
        PercentHealth -= dmg.RawDamage / _stats.MaxHealth;
        _healthDirty = true;
        if (PercentHealth <= 0) Destroy(gameObject);
    }

    public bool DestroyProjectile(CollisionType type)
    {
        return true;
    }

    public void Initialize(ShipData data, ShipSpawner spawner)
    {
        _stats = GetComponent<ShipStats>();
    }

    public void Load(ShipSaveData saveData)
    {
        PercentHealth = saveData.healthPercentage;
    }

    [ContextMenu("Test Damage")]
    public void TestDamage()
    {
        TakeDamage(new Damage(Vector2.down, 1, CollisionType.energy));
    }

    public event Action OnHealthChanged;
}