using UnityEngine;

namespace Ships
{
    /// <summary>
    /// A class that provides a uniform interface for modifying statistics.
    /// </summary>
    /// <remarks>
    ///  The stat value is constrained to be a non-negative.
    ///  Implicitly casts to a floating point number repressing the current stat value.
    /// </remarks>
    public class ModifiableStat
    {
        public ModifiableStat(float baseValue = 0)
        {
            BaseValue = baseValue;
        }

        /// <summary>
        /// The base value of this statistic with no modifiers applied
        /// </summary>
        public float BaseValue { get; private set; }

        /// <summary>
        /// An additive modifer to the base stat (applied before additive modifiers)
        /// </summary>
        /// <remarks>
        /// A value of 0 indicates no scaling.
        /// </remarks>
        public float BaseModifier { get; set; }

        /// <summary>
        /// A percent multiplicative modifer used to scale the base stat. Intended to be used for difficulty scaling.
        /// </summary>
        /// <remarks>
        /// A value of 0 indicates no scaling.
        /// </remarks>
        public float ScalingModifer { get; set; }

        /// <summary>
        /// A percent multiplicative modifer used to scale the base stat.
        /// </summary>
        /// <remarks>
        /// A value of 0 indicates no scaling.
        /// </remarks>
        public float MultiplicativeModifer { get; set; }

        /// <summary>
        /// The current value of this stat with all modifiers applied.
        /// </summary>
        /// <remarks>
        /// Constraints the current value to be non-negative to prevent potential bugs like dealing negative damage.
        /// </remarks>
        public float CurrentValue =>
            Mathf.Max((BaseValue + BaseModifier) * (1 + ScalingModifer) + (1 + MultiplicativeModifer), 0);

        public void UpdateBaseValue(float value)
        {
            BaseValue = value;
        }

        public static implicit operator float(ModifiableStat stat) => stat.CurrentValue;
    }
}