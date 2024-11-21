using System;

namespace Assets.Scripts.CarConfigs
{
    [Serializable]
    public class EngineData
    {
        public EngineType engineType;
        public float horsePower;
        public float overheatRate;
    }
}