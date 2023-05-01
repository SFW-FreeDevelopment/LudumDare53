﻿using LD53.Abstractions;
using LD53.Enums;
using LD53.Minigame;
using UnityEngine;
using UnityEngine.UI;

namespace LD53.Managers
{
    public class TruckManager : SceneSingleton<TruckManager>
    {
        [SerializeField] private Button _giveConeButton;
        [SerializeField] private Button _trashButton;
        [SerializeField] private Button _vanillaButton, _chocolateButton, _strawberryButton, _mintButton,
            _blueberryButton, _sherbetButton;
        [SerializeField] private Image _kid;
        [SerializeField] private SpriteRenderer _iceCream1, _iceCream2;
        [SerializeField] private Image _requestedIceCream1, _requestedIceCream2;
        [SerializeField] private Sprite[] _iceCreamFlavorSprites;
        public Order Order { get; set; } = new Order();
        
        public IceCreamFlavor? Flavor1 { get; private set; }
        public IceCreamFlavor? Flavor2 { get; private set; }
        
        protected override void InitSingletonInstance()
        {
            _trashButton.onClick.AddListener(Trash);
            _giveConeButton.onClick.AddListener(AttemptGiveCone);
            _vanillaButton.onClick.AddListener(() => Scoop(IceCreamFlavor.Vanilla));
            _chocolateButton.onClick.AddListener(() => Scoop(IceCreamFlavor.Chocolate));
            _strawberryButton.onClick.AddListener(() => Scoop(IceCreamFlavor.Strawberry));
            _mintButton.onClick.AddListener(() => Scoop(IceCreamFlavor.Mint));
            _blueberryButton.onClick.AddListener(() => Scoop(IceCreamFlavor.Blueberry));
            _sherbetButton.onClick.AddListener(() => Scoop(IceCreamFlavor.Sherbet));
        }

        private void Scoop(IceCreamFlavor flavor)
        {
            if (Flavor1 == null)
            {
                AudioManager.Instance.Play("scoop");
                Flavor1 = flavor;
                _iceCream1.sprite = _iceCreamFlavorSprites[(int)flavor];
            }
            else if (Flavor2 == null)
            {
                AudioManager.Instance.Play("scoop");
                Flavor2 = flavor;
                _iceCream2.sprite = _iceCreamFlavorSprites[(int)flavor];;
            }
        }
        
        private void DriveUp()
        {
            Trash();
            Order.Generate();
            _requestedIceCream1.sprite = _iceCreamFlavorSprites[(int)Order.Flavor1];
            if (Order.Flavor2 != null)
            {
                _requestedIceCream2.sprite = _iceCreamFlavorSprites[(int)Order.Flavor2];
                _requestedIceCream2.enabled = true;
            }
            else
            {
                _requestedIceCream2.enabled = false;
            }
        }
        
        private void AttemptGiveCone()
        {
            if (Flavor1 == null) return;
            AudioManager.Instance.Play("register");
            var coneWasCorrect = Order.GiveCone(Flavor1.Value, Flavor2);
            if (coneWasCorrect)
            {
                // do things
            }
            else
            {
                
            }
        }
        
        private void Trash()
        {
            Flavor1 = null;
            Flavor2 = null;
            _iceCream1.sprite = null;
            _iceCream2.sprite = null;
        }
    }
}