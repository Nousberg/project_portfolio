using System;

namespace Assets.Scripts.Saving.Cars
{
    [Serializable]
    public class SavedGear
    {
        public string gearName;
        public int ownerCarIndex;

        public SavedGear(string gearName, int ownerCarIndex)
        {
            this.gearName = gearName;
            this.ownerCarIndex = ownerCarIndex;
        }
    }
}