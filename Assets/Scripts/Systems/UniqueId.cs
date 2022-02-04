using System;
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
        public IdList parentList;

        [NonSerialized] private IdList _oldParentList;
        // OnValidate is called before OnEnable, meaning the Id can be added to the list twice
        // resulting in an error
        [NonSerialized] private bool _enabled = false;

        private void OnEnable()
        {
            if (parentList != null)
            {
                parentList.Add(this);
            }
            _enabled = true;
            _oldParentList = parentList;
        }
        private void OnDisable()
        {
            if (parentList != null)
            {
                parentList.Remove(this);
            }
            _enabled = false;
        }

        [ContextMenu("Update List")]
        private void OnValidate()
        {
            if (parentList != _oldParentList && _enabled)
            {
                if (_oldParentList != null)
                {
                    _oldParentList.Remove(this);
                }
                if (parentList != null)
                {
                    parentList.Add(this);
                }
            }
        }
    }
}
