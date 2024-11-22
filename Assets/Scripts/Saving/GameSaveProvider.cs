using Assets.Scripts.CarConfigs;
using Assets.Scripts.CarConfigs.Scriptables;
using Assets.Scripts.Saving.Cars;
using Assets.Scripts.Shop;
using Assets.Scripts.Storage;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Saving
{
    [RequireComponent(typeof(ShopManager))]
    [RequireComponent(typeof(PlayerCarsManager))]
    public class GameSaveProvider : MonoBehaviour
    {
        public const string SHOP_DATA_FILE = "Main";
        public const string PLAYER_DATA_FILE = "Player";

        private const string GAME_SAVE_PATH = "Game/";

        private PlayerCarsManager carsManager => GetComponent<PlayerCarsManager>();
        private ShopManager shopManager => GetComponent<ShopManager>();

        //private void OnApplicationQuit() => SaveShop(SHOP_DATA_FILE);

        public void SaveShop(string name)
        {
            List<SavedBody> bodies = new List<SavedBody>();
            List<SavedCar> cars = new List<SavedCar>();
            List<SavedGear> gears = new List<SavedGear>();
            List<SavedEngine> engines = new List<SavedEngine>();

            foreach (var playerCar in carsManager.GetAviableCars())
            {
                string engine = playerCar.engine == null ? "none" : playerCar.engine.name;
                string gear = playerCar.gears == null ? "none" : playerCar.gears.name;
                string carcass = playerCar.carcass == null ? "none" : playerCar.carcass.data.name;
                string spoiler = playerCar.spoiler == null ? "none" : playerCar.spoiler.data.name;
                string headlights = playerCar.headlights == null ? "none" : playerCar.headlights.data.name;

                cars.Add(
                    new SavedCar(playerCar.data.name, 

                    new SavedEngine(engine, -1), 
                    new SavedGear(gear, -1), 
                    new SavedBody(carcass, carcass == "none" ? new SavedColor(Color.black) : new SavedColor(playerCar.carcass.material.color), -1), 
                    new SavedBody(spoiler, spoiler == "none" ? new SavedColor(Color.black) : new SavedColor(playerCar.spoiler.material.color), -1), 
                    new SavedBody(headlights, headlights == "none" ? new SavedColor(Color.black) : new SavedColor(playerCar.headlights.material.color), -1)
                    ));
            }
            foreach (var body in carsManager.GetAviableBodies())
                bodies.Add(new SavedBody(body.data.name, new SavedColor(body.material.color), body.ownerCarIndex));

            foreach (var engine in carsManager.GetAviableEngines())
                engines.Add(new SavedEngine(engine.data.name, engine.ownerCarIndex));

            foreach (var gear in carsManager.GetAviableGears())
                gears.Add(new SavedGear(gear.data.name, gear.ownerCarIndex));

            BinarySaver.Save(new SavedShopData(cars, new SavedParts(gears, engines, bodies), shopManager.Coins), GAME_SAVE_PATH, name);
        }
        public LoadedShopData LoadShop(string name)
        {
            try
            {
                SavedShopData data = BinarySaver.Load<SavedShopData>(GAME_SAVE_PATH + name);

                List<InStorageCarConfig> cars = new List<InStorageCarConfig>();
                List<InStorageBodyConfig> bodys = new List<InStorageBodyConfig>();
                List<InStorageEngineConfig> engines = new List<InStorageEngineConfig>();
                List<InStorageGearConfig> gears = new List<InStorageGearConfig>();

                foreach (var car in data.cars)
                {
                    InStorageCarConfig addedCar = new InStorageCarConfig(carsManager.AllInGameCars.Find(n => n.name == car.carName));

                    if (car.gear.gearName != "none")
                        addedCar.gears = carsManager.AllInGameGears.Find(n => n.name == car.gear.gearName);
                    else
                        addedCar.gears = null;

                    if (car.engine.engineName != "none")
                        addedCar.engine = carsManager.AllInGameEngines.Find(n => n.name == car.engine.engineName);
                    else
                        addedCar.engine = null;

                    if (car.headlights.bodyName != "none")
                    {
                        BodyConfigData findedBody = carsManager.AllInGameBodys.Find(n => n.name == car.headlights.bodyName && n.GetBody().bodyType == BodyType.Headlights);
                        addedCar.headlights = new InStorageBodyConfig(findedBody, -1);
                        addedCar.headlights.material = new Material(findedBody.GetBody().material);
                        addedCar.headlights.material.color = ColorToUnsaved(car.headlights.bodyColor);
                    }
                    else
                        addedCar.headlights = null;

                    if (car.spoiler.bodyName != "none")
                    {
                        BodyConfigData findedBody = carsManager.AllInGameBodys.Find(n => n.name == car.spoiler.bodyName && n.GetBody().bodyType == BodyType.Spoiler);
                        addedCar.spoiler = new InStorageBodyConfig(findedBody, -1);
                        addedCar.spoiler.material = new Material(findedBody.GetBody().material);
                        addedCar.spoiler.material.color = ColorToUnsaved(car.spoiler.bodyColor);
                    }
                    else
                        addedCar.spoiler = null;

                    if (car.carcass.bodyName != "none")
                    {
                        BodyConfigData findedBody = carsManager.AllInGameBodys.Find(n => n.name == car.carcass.bodyName && n.GetBody().bodyType == BodyType.Carcass);
                        addedCar.carcass = new InStorageBodyConfig(findedBody, -1);
                        addedCar.carcass.material = new Material(findedBody.GetBody().material);
                        addedCar.carcass.material.color = ColorToUnsaved(car.carcass.bodyColor);
                    }
                    else
                        addedCar.carcass = null;

                    cars.Add(addedCar);
                }
                foreach (var engine in data.parts.myEngines)
                    engines.Add(new InStorageEngineConfig(carsManager.AllInGameEngines.Find(n => n.name == engine.engineName), engine.ownerCarIndex));

                foreach (var gear in data.parts.myGears)
                    gears.Add(new InStorageGearConfig(carsManager.AllInGameGears.Find(n => n.name == gear.gearName), gear.ownerCarIndex));

                foreach (var body in data.parts.myBodys)
                    bodys.Add(new InStorageBodyConfig(carsManager.AllInGameBodys.Find(n => n.name == body.bodyName), -1));

                return new LoadedShopData(cars, engines, gears, bodys, data.coins);
            }
            catch
            {
                return null; 
            }
        }

        private Color ColorToUnsaved(SavedColor color)
        {
            return new Color(color.r, color.g, color.b, color.a);
        }
    }
}
