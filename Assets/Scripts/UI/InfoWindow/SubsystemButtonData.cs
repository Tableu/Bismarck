using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.InfoWindow
{
    [Serializable]
    public class ButtonData
    {
        public Sprite Icon;
        public Sprite Button;
        public string Name;
        public Subsystem Subsystem;
    }

    [CreateAssetMenu(fileName = "SubsystemButtonData", menuName = "UI Data/SubsystemButtonData")]
    public class SubsystemButtonData : ScriptableObject
    {
        [SerializeField] private List<ButtonData> _buttonData;

        public List<ButtonData> ButtonData => _buttonData;
    }
}