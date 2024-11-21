using System;
using UnityEngine;

namespace Assets.Scripts.Saving
{
    [Serializable]
    public class SavedColor
    {
        public float r, g, b, a;

        public SavedColor(Color color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }
    }
}
