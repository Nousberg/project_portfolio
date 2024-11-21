using System;

namespace Assets.Scripts.Saving.Cars
{
    [Serializable]
    public class SavedCar
    {
        public string carName;
        public SavedBody carcass;
        public SavedBody spoiler;
        public SavedBody headlights;
        public SavedEngine engine;
        public SavedGear gear;

        public SavedCar(string carName, SavedEngine engine, SavedGear gear, SavedBody carcass, SavedBody spoiler, SavedBody headlights)
        {
            this.carName = carName;
            this.engine = engine;
            this.gear = gear;
            this.carcass = carcass;
            this.spoiler = spoiler;
            this.headlights = headlights;
        }
    }
}
