using Assets.Scripts.CarConfigs.Scriptables;
using Assets.Scripts.Saving;
using Assets.Scripts.Storage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CarConfigs
{
    [RequireComponent(typeof(GameSaveProvider))]
    public class PlayerCarsManager : MonoBehaviour
    {
        [field: Header("Base data")]
        [field: SerializeField] public List<DefaultCarConfigData> AllInGameCars { get; private set; } = new List<DefaultCarConfigData>();
        [field: SerializeField] public List<EngineConfigData> AllInGameEngines { get; private set; } = new List<EngineConfigData>();
        [field: SerializeField] public List<GearConfigData> AllInGameGears { get; private set; } = new List<GearConfigData>();
        [field: SerializeField] public List<BodyConfigData> AllInGameBodys { get; private set; } = new List<BodyConfigData>();

        public event Action OnStorageUpdated;

        public List<InStorageBodyConfig> GetAviableBodies() => aviableBodys;
        public List<InStorageCarConfig> GetAviableCars() => aviableCars;
        public List<InStorageEngineConfig> GetAviableEngines() => aviableEngines;
        public List<InStorageGearConfig> GetAviableGears() => aviableGears;

        private GameSaveProvider saveProvider => GetComponent<GameSaveProvider>();
        private List<InStorageCarConfig> aviableCars = new List<InStorageCarConfig>();
        private List<InStorageBodyConfig> aviableBodys = new List<InStorageBodyConfig>();
        private List<InStorageEngineConfig> aviableEngines = new List<InStorageEngineConfig>();
        private List<InStorageGearConfig> aviableGears = new List<InStorageGearConfig>();

        private void Start()
        {
            LoadedShopData data = saveProvider.LoadShop(GameSaveProvider.SHOP_DATA_FILE);
            
            if (data != null)
            {
                aviableCars = data.cars;
                
                foreach (var body in data.bodys)
                    if (body != null)
                        aviableBodys.Add(body);

                foreach (var engine in data.engines)
                    if (engine != null)
                        aviableEngines.Add(engine);

                foreach (var gear in data.gears)
                    if (gear != null)
                        aviableGears.Add(gear);
            }
        }

        public void SetColorOfBody(Color c, int bodyIndex)
        {
            if (bodyIndex < 0 || bodyIndex >= aviableBodys.Count)
                return;

            if (aviableBodys[bodyIndex].ownerCarIndex > -1 && aviableBodys[bodyIndex].ownerCarIndex < aviableCars.Count)
                switch (aviableBodys[bodyIndex].data.GetBody().bodyType)
                {
                    case BodyType.Carcass:
                        aviableCars[aviableBodys[bodyIndex].ownerCarIndex].carcass.color = c;
                        break;
                    case BodyType.Spoiler:
                        aviableCars[aviableBodys[bodyIndex].ownerCarIndex].spoiler.color = c;
                        break;
                    case BodyType.Headlights:
                        aviableCars[aviableBodys[bodyIndex].ownerCarIndex].headlights.color = c;
                        break;
                }

            aviableBodys[bodyIndex].color = c;
            OnStorageUpdated?.Invoke();
        }
        public void SetBodyOfCar(int bodyIndex, int carIndex)
        {
            if (carIndex < 0 || carIndex >= aviableCars.Count)
                return;

            if (bodyIndex < 0 || bodyIndex >= aviableBodys.Count)
                return;

            switch (aviableBodys[bodyIndex].data.GetBody().bodyType)
            {
                case BodyType.Carcass:
                    aviableCars[carIndex].carcass = aviableBodys[bodyIndex];
                    break;
                case BodyType.Spoiler:
                    aviableCars[carIndex].spoiler = aviableBodys[bodyIndex];
                    break;
                case BodyType.Headlights:
                    aviableCars[carIndex].spoiler = aviableBodys[bodyIndex];
                    break;
            }
            OnStorageUpdated?.Invoke();
        }
        public void SetEngineOfCar(int engineIndex, int carIndex)
        {
            if (carIndex < 0 || carIndex >= aviableCars.Count)
                return;

            if (engineIndex < 0 || engineIndex >= aviableEngines.Count)
                return;

            if (aviableEngines[engineIndex].ownerCarIndex > -1 && aviableEngines[engineIndex].ownerCarIndex < aviableCars.Count)
                if (aviableCars[carIndex].engine != null)
                {
                    aviableCars[aviableEngines[engineIndex].ownerCarIndex].engine = aviableCars[carIndex].engine;
                    aviableEngines.Find(n => n.data == aviableCars[carIndex].engine);
                }
                else
                    aviableCars[aviableEngines[engineIndex].ownerCarIndex].engine = null;

            aviableEngines[engineIndex].ownerCarIndex = carIndex;
            aviableCars[carIndex].engine = aviableEngines[engineIndex].data;
            OnStorageUpdated?.Invoke();
        }
        public void SetGearsOfCar(int gearsIndex, int carIndex)
        {
            if (carIndex < 0 || carIndex >= aviableCars.Count)
                return;

            if (gearsIndex < 0 || gearsIndex >= aviableGears.Count)
                return;

            if (aviableGears[gearsIndex].ownerCarIndex > -1 && aviableGears[gearsIndex].ownerCarIndex < aviableCars.Count)
                if (aviableCars[carIndex].gears != null)
                {
                    aviableCars[aviableGears[gearsIndex].ownerCarIndex].gears = aviableCars[carIndex].gears;
                    aviableGears.Find(n => n.data == aviableCars[carIndex].gears);
                }
                else
                    aviableCars[aviableGears[gearsIndex].ownerCarIndex].gears = null;

            aviableGears[gearsIndex].ownerCarIndex = carIndex;
            aviableCars[carIndex].gears = aviableGears[gearsIndex].data;
            OnStorageUpdated?.Invoke();
        }
        public bool AddCar(string carName)
        {
            DefaultCarConfigData findedCar = AllInGameCars.Find(n => n.name == carName);

            if (findedCar != null)
            {
                aviableCars.Add(new InStorageCarConfig(findedCar));
                aviableEngines.Add(new InStorageEngineConfig(aviableCars[aviableCars.Count - 1].data.GetDefaultEngine(), aviableCars.Count - 1));
                aviableGears.Add(new InStorageGearConfig(aviableCars[aviableCars.Count - 1].data.GetDefaultGear(), aviableCars.Count - 1));

                foreach (var body in aviableCars[aviableCars.Count - 1].data.GetDefaultBodies())
                    aviableBodys.Add(new InStorageBodyConfig(body, aviableCars.Count - 1));

                OnStorageUpdated?.Invoke();
                return true;
            }
            return false;
        }
        public bool AddBody(string name)
        {
            BodyConfigData findedBody = AllInGameBodys.Find(n => n.name == name);

            if (findedBody != null)
            {
                aviableBodys.Add(new InStorageBodyConfig(findedBody, -1));
                OnStorageUpdated?.Invoke();
                return true;
            }
            return false;
        }
        public bool AddEngine(string name)
        {
            EngineConfigData findedEngine = AllInGameEngines.Find(n => n.name == name);

            if (findedEngine != null)
            {
                aviableEngines.Add(new InStorageEngineConfig(findedEngine, -1));
                OnStorageUpdated?.Invoke();
                return true;
            }
            return false;
        }
        public bool AddGears(string name)
        {
            GearConfigData findedGears = AllInGameGears.Find(n => n.name == name);

            if (findedGears != null)
            {
                aviableGears.Add(new InStorageGearConfig(findedGears, -1));
                OnStorageUpdated?.Invoke();
                return true;
            }
            return false;
        }
        public bool RemoveCar(int carIndex)
        {
            if (carIndex < 0 || carIndex >= aviableCars.Count)
                return false;

            aviableCars.RemoveAt(carIndex);
            OnStorageUpdated?.Invoke();
            return true;
        }
        public bool TryRemoveBody(int bodyIndex)
        {
            if (bodyIndex < 0 || bodyIndex >= aviableBodys.Count)
                return false;

            if (aviableBodys[bodyIndex].ownerCarIndex > -1 && aviableBodys[bodyIndex].ownerCarIndex < aviableCars.Count)
                switch (aviableBodys[bodyIndex].data.GetBody().bodyType)
                {
                    case BodyType.Carcass:
                        aviableCars[aviableBodys[bodyIndex].ownerCarIndex].carcass = null;
                        break;
                    case BodyType.Spoiler:
                        aviableCars[aviableBodys[bodyIndex].ownerCarIndex].spoiler = null;
                        break;
                    case BodyType.Headlights:
                        aviableCars[aviableBodys[bodyIndex].ownerCarIndex].headlights = null;
                        break;
                }

            aviableBodys.RemoveAt(bodyIndex);

            OnStorageUpdated?.Invoke();
            return true;
        }    
        public bool TryRemoveEngine(int engineIndex)
        {
            if (engineIndex < 0 || engineIndex >= aviableEngines.Count)
                return false;

            if (aviableEngines[engineIndex].ownerCarIndex > -1 && aviableEngines[engineIndex].ownerCarIndex < aviableCars.Count)
                aviableCars[aviableEngines[engineIndex].ownerCarIndex].engine = null;

            aviableEngines.RemoveAt(engineIndex);

            OnStorageUpdated?.Invoke();
            return true;
        }
        public bool TryRemoveGears(int gearsIndex)
        {
            if (gearsIndex < 0 || gearsIndex >= aviableGears.Count)
                return false;

            if (aviableGears[gearsIndex].ownerCarIndex > -1 && aviableGears[gearsIndex].ownerCarIndex < aviableCars.Count)
                aviableCars[aviableGears[gearsIndex].ownerCarIndex].gears = null;

            aviableGears.RemoveAt(gearsIndex);

            OnStorageUpdated?.Invoke();
            return true;
        }
    }
}