using Ships.Components;
using Systems.Modifiers;
using UnityEngine;

namespace Effects
{
    [CreateAssetMenu(fileName = "New Speed Modifer", menuName = "Effects/SpeedModifer", order = 0)]
    public class SpeedModiferEffect : StatModiferEffect
    {

        protected override ModifiableStat GetStat(ModifiableTarget ship)
        {
            var stats = ship.GetComponent<ShipStats>();
            return stats.SpeedMultiplier;
        }
    }
}
