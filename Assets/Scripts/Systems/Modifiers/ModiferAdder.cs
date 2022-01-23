using UnityEngine;

namespace Systems.Modifiers
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
