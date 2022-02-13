using Ships.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InfoWindow
{
    public class SubsystemButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Text _buttonText;
        public ShipInfo ShipInfo;
        public Subsystem Subsystem;
        public ButtonData ButtonData;

        void Start()
        {
            if (ButtonData != null)
            {
                _buttonText.text = ButtonData.Name;
            }
        }

        void Update()
        {
            RefreshButton();
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