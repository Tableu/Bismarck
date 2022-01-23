using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems
{
    /// <summary>
    ///     A scriptable object used store lists of UUID objects. Automatically sets the parentList property of the UUID object
    ///     to the list when it is added to the list via the inspector.
    /// </summary>
    [CreateAssetMenu(fileName = "New UUID List", menuName = "UUID List", order = 0)]
    public class UUIDList : ScriptableObject
    {
        public List<UuidScriptableObject> uniqueObjects;

        private void OnValidate()
        {
            foreach (var uniqueObject in uniqueObjects)
            {
                if (uniqueObject.parentList != null && uniqueObject.parentList != this)
                    Debug.LogWarning(
                        $"Attempting to add {uniqueObject.uuid} to multiple lists ({name} and {uniqueObject.parentList.name})");
                uniqueObject.parentList = this;
            }
        }

        /// <summary>
        /// Gets a scriptable object from is uuid.
        /// </summary>
        /// <param name="id">The scriptable object uuid</param>
        /// <returns>A scriptable object with the given id or null if no such object could be found</returns>
        public UuidScriptableObject FindByUUID(string id)
        {
            if (id is null) return null;
            return uniqueObjects.FirstOrDefault(scriptableObject => scriptableObject.uuid == id);
        }
    }
}