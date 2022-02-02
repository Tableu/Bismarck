using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems
{
    /// <summary>
    ///     A scriptable object used store lists of ID objects. Automatically sets the parentList property of the UUID object
    ///     to the list when it is added to the list via the inspector.
    /// </summary>
    [CreateAssetMenu(fileName = "New ID List", menuName = "ID List", order = 0)]
    public class IdList : ScriptableObject
    {
        public List<UniqueId> uniqueObjects;

        /// <summary>
        /// Called when values are changed in the inspector, also called by UniqueId on validate function.
        /// Verifies that all IDs are unique and prints an error if they are not.
        /// </summary>
        [ContextMenu("Validate IDs")]
        public void OnValidate()
        {
            var duplicateIds = new List<string>();
            foreach (var uniqueObject in uniqueObjects)
            {
                if (uniqueObject.parentList != null && uniqueObject.parentList != this)
                {
                    Debug.LogWarning($"Attempting to add {uniqueObject.id} to multiple lists ({name} and {uniqueObject.parentList.name})");
                }
                var idCount = uniqueObjects.Count(o => o.id == uniqueObject.id);
                if (!duplicateIds.Contains(uniqueObject.id) && idCount != 1)
                {
                    Debug.LogError($"Found {idCount} occurrences of \"{uniqueObject.id}\" in {name}");
                    duplicateIds.Add(uniqueObject.id);
                }
                uniqueObject.parentList = this;
            }
        }

        /// <summary>
        /// Gets a scriptable object from is id.
        /// </summary>
        /// <param name="id">The scriptable object uuid</param>
        /// <returns>A scriptable object with the given id or null if no such object could be found</returns>
        public UniqueId FindById(string id)
        {
            if (id is null) return null;
            return uniqueObjects.FirstOrDefault(scriptableObject => scriptableObject.id == id);
        }
    }
}
