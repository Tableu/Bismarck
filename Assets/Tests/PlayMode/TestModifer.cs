using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Systems.Effects;
using Systems.Modifiers;
using Tests.Mocks;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    [TestFixture]
    public class TestModifer
    {
        [SetUp]
        public void Init()
        {
            TestModifierData = ScriptableObject.CreateInstance<ModifierData>();
            TestEffectGroup = ScriptableObject.CreateInstance<EffectGroup>();
            TestMockEffect = ScriptableObject.CreateInstance<MockEffect>();
            TestGameObject = new GameObject();
            TestTarget = TestGameObject.AddComponent<ModifiableTarget>();

            TestEffectGroup.Effects = new List<IEffect> {TestMockEffect};
            TestModifierData.EffectGroups = new List<EffectGroup> {TestEffectGroup};
        }

        public ModifierData TestModifierData;
        public EffectGroup TestEffectGroup;
        public MockEffect TestMockEffect;
        public GameObject TestGameObject;
        private ModifiableTarget TestTarget;

        [UnityTest]
        public IEnumerator TestModiferIsRemovedAfterDuration()
        {
            TestModifierData.Duration = 5;
            TestModifierData.HasDuration = true;

            TestModifierData.AttachNewModifer(TestTarget);

            Assert.AreEqual(TestMockEffect.ApplyCount, 1);
            Assert.AreEqual(TestMockEffect.RemoveCount, 0);

            yield return new WaitForSeconds(5.1f);

            Assert.AreEqual(TestMockEffect.ApplyCount, 1);
            Assert.AreEqual(TestMockEffect.RemoveCount, 1);
        }

        [UnityTest]
        public IEnumerator TestModiferWithRemovedBeforeDurationThrowsNoErrors()
        {
            TestModifierData.Duration = 5;
            TestModifierData.HasDuration = true;

            var modifer = TestModifierData.AttachNewModifer(TestTarget);

            Assert.AreEqual(TestMockEffect.ApplyCount, 1);
            Assert.AreEqual(TestMockEffect.RemoveCount, 0);

            yield return new WaitForSeconds(4.1f);
            modifer.Remove();

            Assert.AreEqual(TestMockEffect.ApplyCount, 1);
            Assert.AreEqual(TestMockEffect.RemoveCount, 1);

            LogAssert.NoUnexpectedReceived();
            yield return new WaitForSeconds(2f);

            Assert.AreEqual(TestMockEffect.ApplyCount, 1);
            Assert.AreEqual(TestMockEffect.RemoveCount, 1);
        }

        [UnityTest]
        public IEnumerator TestModiferWithDisabledDurationIsNotRemoved()
        {
            TestModifierData.Duration = 1;
            TestModifierData.HasDuration = false;

            TestModifierData.AttachNewModifer(TestTarget);

            Assert.AreEqual(TestMockEffect.ApplyCount, 1);
            Assert.AreEqual(TestMockEffect.RemoveCount, 0);

            yield return new WaitForSeconds(5.1f);

            Assert.AreEqual(TestMockEffect.ApplyCount, 1);
            Assert.AreEqual(TestMockEffect.RemoveCount, 0);
        }

        [UnityTest]
        public IEnumerator TestModiferDisablingRemovesEffects()
        {
            var mod = TestModifierData.AttachNewModifer(TestTarget);
            Assert.AreEqual(TestMockEffect.ApplyCount, 1);
            Assert.AreEqual(TestMockEffect.RemoveCount, 0);
            yield return null;
            mod.Enabled = false;
            Assert.AreEqual(TestMockEffect.ApplyCount, 1);
            Assert.AreEqual(TestMockEffect.RemoveCount, 1);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestModiferDisabledWhenConditionFalse()
        {
            var conditionRule = ScriptableObject.CreateInstance<MockConditionRule>();
            var condition = conditionRule.Condition;
            condition.IsTrue = true;
            TestModifierData.Condition = conditionRule;
            var mod = TestModifierData.AttachNewModifer(TestTarget);

            Assert.IsTrue(mod.Active);
            condition.IsTrue = false;
            Assert.IsFalse(mod.Active);

            Assert.AreEqual(TestMockEffect.ApplyCount, 1);
            Assert.AreEqual(TestMockEffect.RemoveCount, 1);

            condition.IsTrue = true;
            Assert.AreEqual(TestMockEffect.ApplyCount, 2);
            Assert.AreEqual(TestMockEffect.RemoveCount, 1);

            condition.IsTrue = false;
            Assert.AreEqual(TestMockEffect.ApplyCount, 2);
            Assert.AreEqual(TestMockEffect.RemoveCount, 2);
            mod.Remove();
            Assert.AreEqual(TestMockEffect.ApplyCount, 2);
            Assert.AreEqual(TestMockEffect.RemoveCount, 2);
            yield return null;
        }
    }
}