using Assets.Scripts.Saving.Cars;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Saving
{
    [Serializable]
    public class SavedShopData
    {
        public int coins;
        public SavedParts parts;
        public List<SavedCar> cars;

        public SavedShopData(List<SavedCar> cars, SavedParts parts, int coins)
        {
            this.cars = cars;
            this.parts = parts;
            this.coins = coins;
        }
    }
}
