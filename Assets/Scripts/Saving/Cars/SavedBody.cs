using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Saving.Cars
{
    [Serializable]
    public class SavedBody
    {
        public string bodyName;
        public int ownerCarIndex;
        public SavedColor bodyColor;

        public SavedBody(string bodyName, SavedColor bodyColor, int ownerCarIndex)
        {
            this.bodyName = bodyName;
            this.bodyColor = bodyColor;
            this.ownerCarIndex = ownerCarIndex;
        }
    }
}
