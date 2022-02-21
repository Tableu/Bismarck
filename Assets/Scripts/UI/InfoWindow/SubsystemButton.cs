using System;
using Ships.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InfoWindow
{
    /// <summary>
    ///     Displays a subsystems icon in a button which is colored based on the state of the subsystem.
    ///     e.g. the button turns red when the subsystem is disabled.
    ///     When clicked the button sets the target subsystem of the ship.
    /// </summary>
    public class SubsystemButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Text _buttonText;
        public ShipInfo ShipInfo;
        public Subsystem Subsystem;
        public ButtonData ButtonData;
        public IDamageable Target;

        void Start()
        {
            if (ButtonData != null)
            {
                _buttonText.text = ButtonData.Name;
                _button.onClick.AddListener(OnClick);
            }
        }

        void Update()
        {
            RefreshButton();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void RefreshButton()
        {
            switch (Subsystem)
            {
                case Subsystem.None:
                    break;
                case Subsystem.Weapon:
                    //TODO Disable/Enable button case for Weapons
                    break;
                case Subsystem.Engine:
                    if (ShipInfo.SpeedMultiplier.CurrentValue <= 0)
                    {
                        SetButtonDamaged();
                    }
                    else
                    {
                        SetButtonNormal();
                    }

                    break;
            }
        }

        private void OnClick()
        {
            if (ShipInfo != null && Target != null)
            {
                ShipInfo.SelectedTarget = Target;
            }
        }

        private void SetButtonDamaged()
        {
            var colors = _button.colors;
            colors.normalColor = Color.red;
            _button.colors = colors;
        }

        private void SetButtonNormal()
        {
            var colors = _button.colors;
            colors.normalColor = Color.white;
            _button.colors = colors;
        }
    }
}