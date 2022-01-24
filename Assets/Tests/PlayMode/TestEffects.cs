using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Systems.Modifiers;
using Tests.Mocks;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    [TestFixture]
    public class TestEffects
    {
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0.2f, 0.25f)]
        public void TestStatModiferTemporallyModifiesValues(float additive, float multiplicative)
        {
            var modifiableStat = new ModifiableStat(1);
            var effect = ScriptableObject.CreateInstance<MockStatModEffect>();
            effect.Stat = modifiableStat;
            effect.additiveModifer = additive;
            effect.multiplicativeModifer = multiplicative;

            Assert.AreEqual(1f, modifiableStat.CurrentValue);
            effect.Apply(null);
            Assert.AreEqual((1 + additive) * (1 + multiplicative), modifiableStat.CurrentValue);
            effect.Remove(null);
            Assert.AreEqual(1f, modifiableStat.CurrentValue);
        }

        private static float[] periodValues = { 0.1f, 1f };

        [UnityTest]
        public IEnumerator TestPeriodicModiferTickIsCalledAtCorrectFrequency([ValueSource(nameof(periodValues))] float period)
        {

            var testModifierData = ScriptableObject.CreateInstance<ModifierData>();
            var effect = ScriptableObject.CreateInstance<MockPeriodicEffect>();
            var testGameObject = new GameObject();
            var testTarget = testGameObject.AddComponent<ModifiableTarget>();
            testModifierData.Effects = new List<IEffect> { effect };

            int? tickCount = 0;
            effect.Action = () => ++tickCount;
            effect.tickPeriod = period;
            Assert.AreEqual(0, tickCount.Value);
            testModifierData.Duration = 5 * period;
            testModifierData.HasDuration = true;

            testModifierData.AttachNewModifer(testTarget);
            yield return new WaitForSeconds(5 * period);
            Assert.AreEqual(5, tickCount.Value);
            yield return new WaitForSeconds(5 * period);
            Assert.AreEqual(5, tickCount.Value);
            Assert.AreEqual(1, effect.ApplyCount);
            Assert.AreEqual(1, effect.RemoveCount);
        }
    }
}
