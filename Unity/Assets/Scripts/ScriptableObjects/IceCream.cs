using LD53.Enums;
using UnityEngine;

namespace LD53.ScriptableObjects
{
    [CreateAssetMenu(menuName = "LD53/Ice Cream")]
    public class IceCream : ScriptableObject
    {
        public string Name => name;

        [SerializeField] private IceCreamFlavor _flavor;
        public IceCreamFlavor Flavor => _flavor;
        
        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;
    }
}