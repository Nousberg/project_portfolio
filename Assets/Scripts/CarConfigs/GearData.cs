using System;

namespace Assets.Scripts.CarConfigs
{
    [Serializable]
    public class GearData
    {
        public GearSeasonType seasonType;
        public int gearSlot;
        public float durability;
        public float friction;
    }
}
