using System;
using System.Collections.Generic;

namespace Assets.Scripts.Saving.Cars
{
    [Serializable]
    public class SavedParts
    {
        public List <SavedBody> myBodys = new List <SavedBody>();
        public List<SavedGear> myGears = new List<SavedGear>();
        public List<SavedEngine> myEngines = new List<SavedEngine>();

        public SavedParts(List<SavedGear> gears, List<SavedEngine> engines, List<SavedBody> bodys)
        {
            myEngines = engines;
            myGears = gears;
            myBodys = bodys;
        }
    }
}
