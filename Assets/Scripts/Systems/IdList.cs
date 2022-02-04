using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    /// <summary>
    ///     A scriptable object used store lists of ID objects. Automatically sets the parentList property of the UUID object
    ///     to the list when it is added to the list via the inspector.
    /// </summary>
    /// <remarks>
    ///     This system is intended to be used to save references to scriptable objects when generating save files.
    ///     It is a slightly nicer solution than serializing references to Scriptable Objects since it gives us complete control
    ///     over the data & results in more readable save files.
    /// </remarks>
    [CreateAssetMenu(fileName = "New ID List", menuName = "ID List", order = 0)]
    public class IdList : ScriptableObject
    {
        [NonSerialized] private Dictionary<string, UniqueId> _idMap = new Dictionary<string, UniqueId>();

        public IReadOnlyDictionary<string, UniqueId> IDMap => _idMap;
        public void Add(UniqueId id)
        {
            Debug.Assert(!_idMap.ContainsKey(id.id), $"IdList already contains key {id.id}");
            _idMap[id.id] = id;
        }

        public void Remove(UniqueId id)
        {
            _idMap.Remove(id.id);
        }
    }
}
