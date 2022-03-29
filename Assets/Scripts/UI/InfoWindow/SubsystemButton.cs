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
        [SerializeField] protected Button _button;
        [SerializeField] private Text _buttonText;
        [SerializeField] protected Image _image;
        public ShipStats Ship;
        public ShipStats Player;
        public Subsystem Subsystem;
        public ButtonData ButtonData;
        public DamageableComponent SubsystemComponent;

        protected virtual void Start()
        {
            if (ButtonData != null)
            {
                _buttonText.text = ButtonData.Name;
                _button.onClick.AddListener(OnClick);
                _image.sprite = ButtonData.Icon;
            }
        }

        protected virtual void Update()
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
                    if (SubsystemComponent is Weapon weapon)
                    {
                        if (weapon.Disabled)
                        {
                            SetButtonDamaged();
                        }
                        else
                        {
                            SetButtonNormal();
                        }
                    }
                    break;
                case Subsystem.Engine:
                    if (Ship.SpeedMultiplier.CurrentValue <= 0)
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

        protected virtual void OnClick()
        {
            if (Player != null && SubsystemComponent != null)
            {
                Player.TargetingHelper.SetTarget(SubsystemComponent);
            }
        }

        private void SetButtonDamaged()
        {
            if (ButtonData != null)
            {
                _button.image.sprite = ButtonData.DamagedButton;
            }
        }

        private void SetButtonNormal()
        {
            if (ButtonData != null)
            {
                _button.image.sprite = ButtonData.Button;
            }
        }

        public void DisableButton()
        {
            _button.interactable = false;
        }

        public void EnableButton()
        {
            _button.interactable = true;
        }
    }
}