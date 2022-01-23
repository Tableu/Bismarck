using UnityEngine;
using Random = System.Random;

namespace Systems
{
    /// <summary>
    ///     A class that can be used to generate unique and persistent ids for a scriptable object. A UuidScriptableObject must
    ///     belong to exactly one UUIDList to function correctly.
    /// </summary>
    /// <remarks>
    ///     Used to save persistent references to scriptable objects when saving the game.
    /// </remarks>
    public class UuidScriptableObject : ScriptableObject
    {
        private const int UuidLen = 4;
        private const string UuidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public string uuid;
        public UUIDList parentList;

        /// <summary>
        ///     Can be called in the editor to generate/regenerate a UUID
        /// </summary>
        [ContextMenu("Regenerate UUID")]
        public void RegenerateUUID()
        {
            uuid = null;
            while (parentList.FindByUUID(uuid) == null)
            {
                uuid = "";
                var rng = new Random();
                for (var i = 0; i < UuidLen; i++)
                {
                    var id = rng.Next(UuidChars.Length);
                    uuid += UuidChars[id];
                }
            }
        }
    }
}