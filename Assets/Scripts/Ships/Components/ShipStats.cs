using Ships.DataManagement;
using Systems.Modifiers;
using UnityEngine;

namespace Ships.Components
{
    [AddComponentMenu("")]
    public class ShipStats : MonoBehaviour, IInitializableComponent
    {
        public ShipData Data { get; private set; }
        public ModifiableStat MaxHealth { get; private set; } = new ModifiableStat();
        public ModifiableStat DamageMultiplier { get; private set; } = new ModifiableStat();
        public ModifiableStat SpeedMultiplier { get; private set; } = new ModifiableStat();


        public void Initialize(ShipData data, ShipSpawner spawner)
        {
            MaxHealth.UpdateBaseValue(data.BaseHealth);
            DamageMultiplier.UpdateBaseValue(data.BaseDamageMultiplier);
            SpeedMultiplier.UpdateBaseValue(data.BaseSpeedMultiplier);
            Data = data;
        }
    }
}
