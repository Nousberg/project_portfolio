using Assets.Scripts.CarConfigs;
using UnityEngine;

namespace Assets.Scripts.Storage
{
    public class InStorageEngineConfig
    {
        public EngineConfigData data = ScriptableObject.CreateInstance<EngineConfigData>();
        public int ownerCarIndex;

        public InStorageEngineConfig(EngineConfigData data, int ownerCarIndex)
        {
            this.data = data;
            this.ownerCarIndex = ownerCarIndex;
        }
    }
}
