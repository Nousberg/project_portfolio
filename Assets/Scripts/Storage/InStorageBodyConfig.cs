using Assets.Scripts.CarConfigs.Scriptables;
using UnityEngine;

namespace Assets.Scripts.Storage
{
    public class InStorageBodyConfig
    {
        public BodyConfigData data = ScriptableObject.CreateInstance<BodyConfigData>();
        public int ownerCarIndex;
        public Color color;

        public InStorageBodyConfig(BodyConfigData data, int ownerCarIndex)
        {
            this.data = data;
            color = data.GetBody().color;
            this.ownerCarIndex = ownerCarIndex;
        }
    }
}
