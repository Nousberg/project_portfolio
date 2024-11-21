using Assets.Scripts.CarConfigs;
using Assets.Scripts.CarConfigs.Scriptables;
using Assets.Scripts.Saving;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    [RequireComponent(typeof(SimpleMenuSwitcher))]
    [RequireComponent(typeof(ShopManager))]
    [RequireComponent(typeof(PlayerCarsManager))]
    public class ShopUiManager : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Button buyCarButton;
        [SerializeField] private Image carIcon;

        [Header("Dropdowns")]
        [SerializeField] private TMP_Dropdown listOfBuyableParts;
        [SerializeField] private TMP_Dropdown listOfSellableParts;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI currentCarNameText;
        [SerializeField] private TextMeshProUGUI currentGearsText;
        [SerializeField] private TextMeshProUGUI currentEngineText;
        [SerializeField] private TextMeshProUGUI partCostText;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI carPriceText;

        private ShopManager shopManager => GetComponent<ShopManager>();
        private PlayerCarsManager carsManager => GetComponent<PlayerCarsManager>();
        private SimpleMenuSwitcher menuSwitcher => GetComponent<SimpleMenuSwitcher>();
        private List<object> sellableParts = new List<object>();
        private List<object> buyableParts = new List<object>();
        private int partToSellIndex;
        private int partToBuyIndex;
        private int currentPage;

        private void Start()
        {
            shopManager.OnCoinsChanged += UpdateCoins;
            carsManager.OnStorageUpdated += UpdatePage;
            menuSwitcher.onMenuChanged += UpdatePage;
            SetPage(0);
        }
        private void UpdateCoins()
        {
            coinsText.text = shopManager.Coins.ToString();
        }
        public void SetCarPartToBuy(int index)
        {
            if (index >= 0 && index < buyableParts.Count)
            {
                partToBuyIndex = index;

                if (buyableParts[index] is StoreData data)
                    partCostText.text = data.Price.ToString();
            }
        }
        public void SetCarPartToSell(int index)
        {
            if (index >= 0 && index < sellableParts.Count)
            {
                partToSellIndex = index;

                if (sellableParts[index] is StoreData data)
                    partCostText.text = data.Price.ToString();
            }
        }
        public void ApplyPartBuy()
        {
            if (partToBuyIndex < 0 || partToBuyIndex >= buyableParts.Count)
                return;

            if (buyableParts[partToBuyIndex] is EngineConfigData engine)
                shopManager.InteractCarEngine(ShopManager.InteractionType.Buy, 0, engine.name);
            else if (buyableParts[partToBuyIndex] is GearConfigData gear)
                shopManager.InteractCarGear(ShopManager.InteractionType.Buy, 0, gear.name);
            else if (buyableParts[partToBuyIndex] is BodyConfigData body)
                shopManager.InteractCarBody(ShopManager.InteractionType.Buy, 0, body.name);
        }    
        public void ApplyPartSell()
        {
            if (partToSellIndex < 0 || partToSellIndex >= sellableParts.Count)
                return;

            if (sellableParts[partToSellIndex] is EngineConfigData engine)
                shopManager.InteractCarEngine(ShopManager.InteractionType.Sell, carsManager.GetAviableEngines().IndexOf(carsManager.GetAviableEngines().Find(n => n.data.name == engine.name)), engine.name);
            else if (sellableParts[partToSellIndex] is GearConfigData gear)
                shopManager.InteractCarGear(ShopManager.InteractionType.Sell, carsManager.GetAviableGears().IndexOf(carsManager.GetAviableGears().Find(n => n.data.name == gear.name)), gear.name);
            else if (sellableParts[partToSellIndex] is BodyConfigData body)
                shopManager.InteractCarBody(ShopManager.InteractionType.Sell, carsManager.GetAviableBodies().IndexOf(carsManager.GetAviableBodies().Find(n => n.data.name == body.name)), body.name);

            listOfSellableParts.value = partToSellIndex - 1;
            listOfSellableParts.RefreshShownValue();
        }
        public void SetPage(int value)
        {
            currentPage += value;

            if (currentPage < 0)
                currentPage = shopManager.AviableCarsInShop.Count - 1;
            else if (currentPage >= shopManager.AviableCarsInShop.Count)
                currentPage = 0;

            UpdatePage();
        }

        private void UpdatePage()
        {
            buyCarButton.onClick.RemoveAllListeners();
            buyCarButton.onClick.AddListener(() => shopManager.InteractCar(ShopManager.InteractionType.Buy, shopManager.AviableCarsInShop[currentPage].name, currentPage));

            currentEngineText.text = shopManager.AviableCarsInShop[currentPage].GetDefaultEngine().name;
            currentGearsText.text = shopManager.AviableCarsInShop[currentPage].GetDefaultGear().name;
            currentCarNameText.text = shopManager.AviableCarsInShop[currentPage].name;

            carIcon.sprite = shopManager.AviableCarsInShop[currentPage].Icon;
            sellableParts.Clear();
            buyableParts.Clear();

            UpdateDropdowns();
            UpdateCoins();
        }
        private void UpdateDropdowns()
        {
            List<TMP_Dropdown.OptionData> optionParts = new List<TMP_Dropdown.OptionData>();

            foreach (var body in shopManager.AviableBodysInShop)
            {
                optionParts.Add(new TMP_Dropdown.OptionData(body.name, body.Icon));
                buyableParts.Add(body);
            }
            foreach (var engine in shopManager.AviableEnginesInShop)
            {
                optionParts.Add(new TMP_Dropdown.OptionData(engine.name, engine.Icon));
                buyableParts.Add(engine);
            }
            foreach (var gear in shopManager.AviableGearsInShop)
            {
                optionParts.Add(new TMP_Dropdown.OptionData(gear.name, gear.Icon));
                buyableParts.Add(gear);
            }

            listOfBuyableParts.options = optionParts;

            List<TMP_Dropdown.OptionData> partsToSellMenuList = new List<TMP_Dropdown.OptionData>();

            foreach (var body in carsManager.GetAviableBodies())
            {
                partsToSellMenuList.Add(new TMP_Dropdown.OptionData(body.data.name, body.data.Icon));
                sellableParts.Add(body.data);
            }
            foreach (var engine in carsManager.GetAviableEngines())
            {
                partsToSellMenuList.Add(new TMP_Dropdown.OptionData(engine.data.name, engine.data.Icon));
                sellableParts.Add(engine.data);
            }
            foreach (var gear in carsManager.GetAviableGears())
            {
                partsToSellMenuList.Add(new TMP_Dropdown.OptionData(gear.data.name, gear.data.Icon));
                sellableParts.Add(gear.data);
            }

            listOfSellableParts.options = partsToSellMenuList;
        }
    }
}