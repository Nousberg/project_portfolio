using System;

namespace Assets.Scripts.Saving.Cars
{
    [Serializable]
    public class SavedEngine
    {
        public string engineName;
        public int ownerCarIndex;

        public SavedEngine(string engineName, int ownerCarIndex)
        {
            this.engineName = engineName;
            this.ownerCarIndex = ownerCarIndex;
        }
    }
}