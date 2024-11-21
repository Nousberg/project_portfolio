using Assets.Scripts.CarConfigs;
using Assets.Scripts.CarConfigs.Scriptables;
using Assets.Scripts.Storage;
using System.Collections.Generic;

namespace Assets.Scripts.Saving
{
    public class LoadedShopData
    {
        public int coins;
        public List<InStorageCarConfig> cars;
        public List<InStorageBodyConfig> bodys;
        public List<InStorageEngineConfig> engines;
        public List<InStorageGearConfig> gears;

        public LoadedShopData(List<InStorageCarConfig> cars, List<InStorageEngineConfig> engines, List<InStorageGearConfig> gears, List<InStorageBodyConfig> bodys, int coins)
        {
            this.cars = cars;
            this.bodys = bodys;
            this.engines = engines;
            this.gears = gears;
            this.coins = coins;
        }
    }
}
