using Assets.Scripts.CarConfigs.Scriptables;
using UnityEngine;

namespace Assets.Scripts.Storage
{
    public class InStorageBodyConfig
    {
        public BodyConfigData data = ScriptableObject.CreateInstance<BodyConfigData>();
        public int ownerCarIndex;
        public Material material;

        public InStorageBodyConfig(BodyConfigData data, int ownerCarIndex)
        {
            this.data = data;
            material = new Material(data.GetBody().material);
            this.ownerCarIndex = ownerCarIndex;
        }
    }
}