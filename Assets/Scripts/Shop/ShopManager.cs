using Assets.Scripts.CarConfigs;
using Assets.Scripts.CarConfigs.Scriptables;
using Assets.Scripts.Saving;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    [RequireComponent(typeof(PlayerCarsManager))]
    public class ShopManager : MonoBehaviour
    {
        [field: Header("Store")]
        [field: SerializeField] public List<DefaultCarConfigData> AviableCarsInShop { get; private set; } = new List<DefaultCarConfigData>();
        [field: SerializeField] public List<BodyConfigData> AviableBodysInShop { get; private set; } = new List<BodyConfigData>();
        [field: SerializeField] public List<EngineConfigData> AviableEnginesInShop { get; private set; } = new List<EngineConfigData>();
        [field: SerializeField] public List<GearConfigData> AviableGearsInShop { get; private set; } = new List<GearConfigData>();

        public event Action OnCoinsChanged;

        public int Coins { get; private set; }

        private GameSaveProvider saveProvider => GetComponent<GameSaveProvider>();
        private PlayerCarsManager carsManager;
        private int previousCoinsAmount;

        private void Start()
        {
            LoadedShopData data = saveProvider.LoadShop(GameSaveProvider.SHOP_DATA_FILE);

            if (data != null)
            {
                Coins = data.coins;
                previousCoinsAmount = Coins;
            }
        }
        private void OnValidate()
        {
            List<DefaultCarConfigData> carsToRemove = new List<DefaultCarConfigData>();

            if (Application.isEditor)
                carsManager = GetComponent<PlayerCarsManager>();

            foreach (var car in AviableCarsInShop)
                if (!carsManager.AllInGameCars.Contains(car))
                    carsToRemove.Add(car);

            foreach (var car in carsToRemove)
                AviableCarsInShop.Remove(car);
        }
        private void CheckForPidorases()
        {
            if (Coins != previousCoinsAmount)
                Application.Quit();
        }

        public void RedeemPromocode(string code)
        {
            switch (code)
            {
                case "test":
                    Coins += 99999999;
                    CheckForPidorases();
                    OnCoinsChanged?.Invoke();
                    break;
            }
        }
        public void InteractCar(InteractionType type, string carName, int index)
        {
            CheckForPidorases();

            if (type == InteractionType.Buy)
            {
                DefaultCarConfigData carToBuy = AviableCarsInShop.Find(n => n.name == carName);

                if (carToBuy != null && Coins >= carToBuy.Price)
                {
                    if (carsManager.AddCar(carToBuy.name))
                    {
                        Coins -= carToBuy.Price;
                        previousCoinsAmount = Coins;

                        OnCoinsChanged?.Invoke();
                    }
                }
            }
            else
            {
                Coins += carsManager.GetAviableCars()[index].data.Price;
                previousCoinsAmount = Coins;

                carsManager.RemoveCar(index);

                OnCoinsChanged?.Invoke();
            }
        }
        public void InteractCarBody(InteractionType type, int bodyIndex, string bodyName)
        {
            CheckForPidorases();

            if (type == InteractionType.Buy)
            {
                BodyConfigData bodyToBuy = AviableBodysInShop.Find(n => n.name == bodyName);
                if (bodyToBuy != null && Coins >= bodyToBuy.Price)
                {
                    if (carsManager.AddBody(bodyToBuy.name))
                    {
                        Coins -= bodyToBuy.Price;
                        previousCoinsAmount = Coins;

                        OnCoinsChanged?.Invoke();
                    }
                }
            }
            else
            {
                BodyConfigData bodyToSell = carsManager.GetAviableBodies()[bodyIndex].data;

                if (bodyToSell != null && carsManager.TryRemoveBody(bodyIndex))
                {
                    Coins += bodyToSell.Price;
                    previousCoinsAmount = Coins;

                    OnCoinsChanged?.Invoke();
                }
            }
        }
        public void InteractCarEngine(InteractionType type, int engineIndex, string engineName)
        {
            CheckForPidorases();

            if (type == InteractionType.Buy)
            {
                EngineConfigData engineToBuy = AviableEnginesInShop.Find(n => n.name == engineName);

                if (engineToBuy != null && Coins >= engineToBuy.Price)
                {
                    if (carsManager.AddEngine(engineToBuy.name))
                    {
                        Coins -= engineToBuy.Price;
                        previousCoinsAmount = Coins;

                        OnCoinsChanged?.Invoke();
                    }
                }
            }
            else
            {
                EngineConfigData engineToSell = carsManager.GetAviableEngines()[engineIndex].data;

                if (engineToSell != null && carsManager.TryRemoveEngine(engineIndex))
                {
                    Coins += engineToSell.Price;
                    previousCoinsAmount = Coins;

                    OnCoinsChanged?.Invoke();
                }
            }
        }
        public void InteractCarGear(InteractionType type, int gearIndex, string gearName)
        {
            CheckForPidorases();

            if (type == InteractionType.Buy)
            {
                GearConfigData gearToBuy = AviableGearsInShop.Find(n => n.name == gearName);

                if (gearToBuy != null && Coins >= gearToBuy.Price)
                {
                    if (carsManager.AddGears(gearToBuy.name))
                    {
                        Coins -= gearToBuy.Price;
                        previousCoinsAmount = Coins;

                        OnCoinsChanged?.Invoke();
                    }
                }
            }
            else
            {
                GearConfigData gearToSell = carsManager.GetAviableGears()[gearIndex].data;

                if (gearToSell != null)
                {
                    if (carsManager.TryRemoveGears(gearIndex))
                    {
                        Coins += gearToSell.Price;
                        previousCoinsAmount = Coins;

                        OnCoinsChanged?.Invoke();
                    }
                }
            }
        }

        public enum InteractionType : byte
        {
            Buy,
            Sell
        }
    }
}