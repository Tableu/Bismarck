namespace Systems.Modifiers
{
    public abstract class StatModiferEffect : IEffect
    {
        public float additiveModifer;
        public float multiplicativeModifer;
        public override void Apply(ModifiableTarget target)
        {
            var stat = GetStat(target);
            stat.BaseModifier += additiveModifer;
            stat.MultiplicativeModifer += multiplicativeModifer;
        }

        public override void Remove(ModifiableTarget target)
        {
            var stat = GetStat(target);
            stat.BaseModifier -= additiveModifer;
            stat.MultiplicativeModifer -= multiplicativeModifer;
        }

        protected abstract ModifiableStat GetStat(ModifiableTarget ship);
    }
}
