using System;
using LD53.Enums;
using Random = UnityEngine.Random;

namespace LD53.Minigame
{
    public class Order
    {
        public string KidVariant { get; private set; }
        public IceCreamFlavor Flavor1 { get; private set; }
        public IceCreamFlavor? Flavor2 { get; private set; }

        public void Generate()
        {
            var iceCreamFlavorValues = Enum.GetValues(typeof(IceCreamFlavor));
            Flavor1 = (IceCreamFlavor)iceCreamFlavorValues.GetValue(Random.Range(0, iceCreamFlavorValues.Length));
            if (Random.Range(0, 10) > 5)
            {
                Flavor2 = (IceCreamFlavor)iceCreamFlavorValues.GetValue(Random.Range(0, iceCreamFlavorValues.Length));
            }
        }

        public void GiveCone(IceCreamFlavor flavor1, IceCreamFlavor? flavor2)
        {
            if (flavor1 == Flavor1 && flavor2 == Flavor2)
            {
                // happy
            }
            else
            {
                // unhappy
            }
        }
    }
}