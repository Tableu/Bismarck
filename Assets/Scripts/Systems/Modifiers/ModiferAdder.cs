using Systems.Modifiers;
using UnityEngine;

namespace Demo
{
    public class ModiferAdder : MonoBehaviour
    {
        public ModifiableTarget Target;
        public ModifierData Modifier;

        public void AddSelectedModifer()
        {
            Modifier.AttachNewModifer(Target);
        }
    }
}