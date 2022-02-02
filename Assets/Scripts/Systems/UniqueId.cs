using Editor;
using UnityEngine;

namespace Systems
{
    /// <summary>
    ///     A class that can be used to generate unique and persistent ids for a scriptable object. A UuidScriptableObject must
    ///     belong to exactly one UUIDList to function correctly.
    /// </summary>
    /// <remarks>
    ///     Used to save persistent references to scriptable objects when saving the game.
    /// </remarks>
    public class UniqueId : ScriptableObject
    {
        [Header("Object ID")]
        public string id;
        [ReadOnlyField] public IdList parentList;

        private void OnValidate()
        {
            if (parentList != null)
            {
                parentList.OnValidate();
            }
        }
    }
}
