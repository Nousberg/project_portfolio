using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SimpleMenuSwitcher : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject uiBackground;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject carInfoMenu;
        [SerializeField] private GameObject storageMenu;
        [SerializeField] private GameObject carPartsMarketMenu;
        [SerializeField] private GameObject shopMenu;
        [SerializeField] private List<GameObject> menus = new List<GameObject>();

        public event Action onMenuChanged;

        private int currentMenu;

        private void HideAllMenus()
        {
            carInfoMenu.SetActive(false);
            uiBackground.SetActive(true);

            foreach (GameObject menu in menus)
                menu.SetActive(false);

            onMenuChanged?.Invoke();
        }
        private void HandleMenuChangeWithoutParams()
        {
            if (currentMenu < 0)
                currentMenu = menus.Count - 1;
            else if (currentMenu >= menus.Count)
                currentMenu = 0;

            HideAllMenus();

            menus[currentMenu].SetActive(true);
        }
        public void SwitchMenu(MenuType type)
        {
            switch (type)
            {
                case MenuType.Main:
                    HideAllMenus();
                    mainMenu.SetActive(true);
                    break;
                case MenuType.Storage:
                    HideAllMenus();
                    storageMenu.SetActive(true);
                    break;
                case MenuType.Shop:
                    HideAllMenus();
                    shopMenu.SetActive(true);
                    break;
                case MenuType.CarPartsMarket:
                    HideAllMenus();
                    carPartsMarketMenu.SetActive(true);
                    break;
                case MenuType.carInfo:
                    HideAllMenus();
                    uiBackground.SetActive(false);
                    carInfoMenu.SetActive(true);
                    break;
            }
        }
        public void NextMenu()
        {
            currentMenu++;
            HandleMenuChangeWithoutParams();
        }
        public void PrevMenu()
        {
            currentMenu--;
            HandleMenuChangeWithoutParams();
        }
        public enum MenuType : byte
        {
            Main,
            Storage,
            Shop,
            carInfo,
            CarPartsMarket
        }
    }
}