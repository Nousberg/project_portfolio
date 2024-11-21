using System;
using Assets.Scripts.CarConfigs;
using UnityEngine;

namespace Assets.Scripts.Storage
{
    public class InStorageGearConfig
    {
        public GearConfigData data = ScriptableObject.CreateInstance<GearConfigData>();
        public int ownerCarIndex;

        public InStorageGearConfig(GearConfigData data, int ownerCarIndex)
        {
            this.data = data;
            this.ownerCarIndex = ownerCarIndex;
        }
    }
}
