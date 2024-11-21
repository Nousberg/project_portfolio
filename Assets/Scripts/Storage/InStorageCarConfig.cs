using Assets.Scripts.CarConfigs;
using Assets.Scripts.CarConfigs.Scriptables;
using UnityEngine;

namespace Assets.Scripts.Storage
{
    public class InStorageCarConfig
    {
        public DefaultCarConfigData data = ScriptableObject.CreateInstance<DefaultCarConfigData>();
        public EngineConfigData engine;
        public GearConfigData gears;
        public InStorageBodyConfig carcass;
        public InStorageBodyConfig headlights;
        public InStorageBodyConfig rightDoor;
        public InStorageBodyConfig leftDoor;
        public InStorageBodyConfig spoiler;

        public InStorageCarConfig(DefaultCarConfigData data)
        {
            this.data = data;

            carcass = new InStorageBodyConfig(data.GetDefaultBodies().Find(n => n.GetBody().bodyType == BodyType.Carcass), -1);
            headlights = new InStorageBodyConfig(data.GetDefaultBodies().Find(n => n.GetBody().bodyType == BodyType.Headlights), -1);
            spoiler = new InStorageBodyConfig(data.GetDefaultBodies().Find(n => n.GetBody().bodyType == BodyType.Spoiler), -1);

            engine = data.GetDefaultEngine();
            gears = data.GetDefaultGear();
        }
    }
}
