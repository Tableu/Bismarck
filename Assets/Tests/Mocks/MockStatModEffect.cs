using Systems.Modifiers;

namespace Tests.Mocks
{
    public class MockStatModEffect : StatModiferEffect
    {

        public ModifiableStat Stat;
        protected override ModifiableStat GetStat(ModifiableTarget ship)
        {
            return Stat;
        }
    }
}
