using Assets.Scripts.CarConfig;
using Assets.Scripts.CarConfigs;
using Assets.Scripts.Storage;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    [RequireComponent(typeof(SimpleMenuSwitcher))]
    [RequireComponent(typeof(Screen))]
    [RequireComponent(typeof(PlayerCarsManager))]
    [RequireComponent(typeof(ShopManager))]
    public class StorageUiManager : MonoBehaviour
    {
        [Header("Other properties")]
        [SerializeField] private GameObject currentCarInfo;
        [SerializeField] private Transform carPrefabPosition;

        [Header("Buttons")]
        [SerializeField] private GameObject equippedCarButton;
        [SerializeField] private GameObject equipCarButton;
        [SerializeField] private Button carInfoButton;
        [SerializeField] private Button sellCarButton;

        [Header("Sprites")]
        [SerializeField] private Image carIcon;

        [Header("Texts")]
        [SerializeField] private GameObject noCarsText;
        [SerializeField] private TextMeshProUGUI currentCarNameText;
        [SerializeField] private TextMeshProUGUI currentCarDescriptionText;
        [SerializeField] private TextMeshProUGUI currentGearsText;
        [SerializeField] private TextMeshProUGUI currentEngineText;

        [Header("Scrolls")]
        [SerializeField] private TMP_Dropdown aviableCarBodys;
        [SerializeField] private TMP_Dropdown aviableCarEngines;
        [SerializeField] private TMP_Dropdown aviableCarGears;
        [SerializeField] private Transform scrollContent;

        private Screen screen => GetComponent<Screen>();
        private ShopManager shopManager => GetComponent<ShopManager>();
        private PlayerCarsManager carsManager => GetComponent<PlayerCarsManager>();
        private SimpleMenuSwitcher menuSwitcher => GetComponent<SimpleMenuSwitcher>();

        private List<GameObject> cars = new List<GameObject>();
        private List<InStorageBodyConfig> selectableBodys = new List<InStorageBodyConfig>();
        private List<InStorageEngineConfig> selectableEngines = new List<InStorageEngineConfig>();
        private List<InStorageGearConfig> selectableGears = new List<InStorageGearConfig>();
        private GameObject currentShowedCarcass;
        private GameObject currentShowedSpoiler;
        private List<GameObject> currentShowedGears = new List<GameObject>();
        private int selectedBody;
        private int selectedEngine;
        private int selectedGears;
        private int currentCarIndex;
        private int equippedCarIndex;
        private bool interactedBodysMenu;
        private bool interactedEnginesMenu;
        private bool interactedGearsMenu;

        private void Start()
        {
            carsManager.OnStorageUpdated += UpdateStorageView;
            menuSwitcher.onMenuChanged += UpdateStorageView;

            UpdateStorageView();
            SetCurrentCar(0);
        }
        private void UpdateStorageView()
        {
            foreach (GameObject t in cars)
                Destroy(t);

            cars.Clear();

            foreach (var car in carsManager.GetAviableCars())
            {
                GameObject carInStorage = new GameObject("Element");
                carInStorage.transform.parent = scrollContent;
                carInStorage.transform.localScale = Vector3.one;

                Button button = carInStorage.AddComponent<Button>();
                Image icon = carInStorage.AddComponent<Image>();

                icon.sprite = car.data.Icon;
                button.onClick.AddListener(() => SetCurrentCar(carsManager.GetAviableCars().IndexOf(car)));

                cars.Add(carInStorage);
            }

            noCarsText.SetActive(!(carsManager.GetAviableCars().Count > 0));
            currentCarInfo.SetActive(carsManager.GetAviableCars().Count > 0);

            if (carsManager.GetAviableCars().Count > 0)
            {
                SetCurrentCar(Mathf.Clamp(currentCarIndex, 0, carsManager.GetAviableCars().Count - 1));
                RefreshShownCarParts(currentCarIndex);
            }
        }
        private void UpdateDropdowns()
        {
            List<TMP_Dropdown.OptionData> bodys = new List<TMP_Dropdown.OptionData>();
            List<TMP_Dropdown.OptionData> engines = new List<TMP_Dropdown.OptionData>();
            List<TMP_Dropdown.OptionData> gears = new List<TMP_Dropdown.OptionData>();

            selectableEngines.Clear();
            selectableGears.Clear();

            foreach (var body in carsManager.GetAviableBodies())
                if (carsManager.GetAviableCars()[currentCarIndex].data.GetBodyConfigs().Contains(body.data))
                {
                    bodys.Add(new TMP_Dropdown.OptionData(body.data.name, body.data.Icon));
                    selectableBodys.Add(body);
                }
            foreach (var engine in carsManager.GetAviableEngines())
                if (carsManager.GetAviableCars()[currentCarIndex].data.GetEngineConfigs().Contains(engine.data))
                {
                    engines.Add(new TMP_Dropdown.OptionData(engine.data.name, engine.data.Icon));
                    selectableEngines.Add(engine);
                }
            foreach (var gear in carsManager.GetAviableGears())
                if (carsManager.GetAviableCars()[currentCarIndex].data.GetGearConfigs().Contains(gear.data))
                {
                    gears.Add(new TMP_Dropdown.OptionData(gear.data.name, gear.data.Icon));
                    selectableGears.Add(gear);
                }

            aviableCarEngines.options = engines;
            aviableCarGears.options = gears;
            aviableCarBodys.options = bodys;
            aviableCarBodys.value = -1;
            aviableCarGears.value = -1;
            aviableCarEngines.value = -1;

            aviableCarEngines.RefreshShownValue();
            aviableCarBodys.RefreshShownValue();
            aviableCarGears.RefreshShownValue();

            currentCarNameText.text = carsManager.GetAviableCars()[currentCarIndex].data.name;
        }
        private void RefreshShownCarParts(int carIndex)
        {
            Destroy(currentShowedCarcass);
            Destroy(currentShowedSpoiler);

            if (carsManager.GetAviableCars()[carIndex].carcass != null)
            {
                BodyData carcass = carsManager.GetAviableCars()[carIndex].carcass.data.GetBody();

                currentShowedCarcass = Instantiate(carsManager.GetAviableCars()[carIndex].carcass.data.Prefab, carPrefabPosition);

                Renderer carcassRenderer = currentShowedCarcass.GetComponent<Renderer>();
                carcassRenderer.material = carcass.material;
                carcassRenderer.material.color = carcass.color;

                currentShowedCarcass.transform.localPosition = carsManager.GetAviableCars()[carIndex].data.GetCarcassPos();
            }
            if (carsManager.GetAviableCars()[carIndex].spoiler != null)
            {
                BodyData spoiler = carsManager.GetAviableCars()[carIndex].spoiler.data.GetBody();

                currentShowedSpoiler = Instantiate(carsManager.GetAviableCars()[carIndex].spoiler.data.Prefab, carPrefabPosition);

                Renderer spoilerRenderer = currentShowedSpoiler.GetComponent<Renderer>();
                spoilerRenderer.material = spoiler.material;
                spoilerRenderer.material.color = spoiler.color;

                currentShowedSpoiler.transform.localPosition = carsManager.GetAviableCars()[carIndex].data.GetSpoilerPos();
            }
            if (carsManager.GetAviableCars()[carIndex].gears != null)
            {
                foreach (var gear in currentShowedGears)
                    Destroy(gear);

                currentShowedGears.Clear();

                foreach (var gearPos in carsManager.GetAviableCars()[carIndex].data.GetGearsPos())
                {
                    currentShowedGears.Add(Instantiate(carsManager.GetAviableCars()[carIndex].gears.Prefab, carPrefabPosition));
                    currentShowedGears[currentShowedGears.Count - 1].transform.position = gearPos;
                }
            }
        }
        public void SetCurrentCarBodyColor()
        {
            if (selectedBody < 0 || selectedBody >= selectableBodys.Count)
                return;

            carsManager.SetColorOfBody(screen.GetPixel(), carsManager.GetAviableBodies().IndexOf(selectableBodys[selectedBody]));
        }
        public void SetCarBody(int indexOfBody)
        {
            if (indexOfBody < 0 || indexOfBody >= selectableBodys.Count)
                return;

            selectedBody = indexOfBody;
            interactedBodysMenu = true;
        }
        public void SetCarEngine(int indexOfEngine)
        {
            if (indexOfEngine < 0 || indexOfEngine >= selectableEngines.Count)
                return;
        
            selectedEngine = indexOfEngine;
            interactedEnginesMenu = true;
        }
        public void SetCarGear(int indexOfGear)
        {
            if (indexOfGear < 0 || indexOfGear >= selectableGears.Count)
                return;

            selectedGears = indexOfGear;
            interactedGearsMenu = true;
        }
        public void ApplyСarPartsChange()
        {
            if (interactedBodysMenu)
                carsManager.SetBodyOfCar(carsManager.GetAviableBodies().IndexOf(selectableBodys[selectedBody]), currentCarIndex);
            if (interactedEnginesMenu)
                carsManager.SetEngineOfCar(carsManager.GetAviableEngines().IndexOf(selectableEngines[selectedEngine]), currentCarIndex);
            if (interactedGearsMenu)
                carsManager.SetGearsOfCar(carsManager.GetAviableGears().IndexOf(selectableGears[selectedGears]), currentCarIndex);

            interactedBodysMenu = false;
            interactedEnginesMenu = false;
            interactedGearsMenu = false;

            SetCurrentCar(equippedCarIndex);
        }
        public void SetEquippedCar()
        {
            equippedCarIndex = currentCarIndex;
            SetCurrentCar(equippedCarIndex);
        }
        public void ShowCarDetailInfo(int carIndex)
        {
            if (carIndex < 0 || carIndex >= carsManager.GetAviableCars().Count)
                return;

            menuSwitcher.SwitchMenu(SimpleMenuSwitcher.MenuType.carInfo);

            RefreshShownCarParts(carIndex);
        }
        public void SetCurrentCar(int index)
        {
            if (index < 0 || index >= carsManager.GetAviableCars().Count)
                return;

            currentCarIndex = index;

            carIcon.sprite = carsManager.GetAviableCars()[currentCarIndex].data.Icon;

            sellCarButton.onClick.RemoveAllListeners();
            sellCarButton.onClick.AddListener(() => shopManager.InteractCar(ShopManager.InteractionType.Sell, string.Empty, currentCarIndex));
            carInfoButton.onClick.RemoveAllListeners();
            carInfoButton.onClick.AddListener(() => ShowCarDetailInfo(currentCarIndex));

            equippedCarButton.SetActive(currentCarIndex == equippedCarIndex);
            equipCarButton.SetActive(currentCarIndex != equippedCarIndex);
            sellCarButton.gameObject.SetActive(true);

            currentEngineText.text = carsManager.GetAviableCars()[currentCarIndex].engine == null ? "--" : carsManager.GetAviableCars()[currentCarIndex].engine.name;
            currentGearsText.text = carsManager.GetAviableCars()[currentCarIndex].gears == null ? "--" : carsManager.GetAviableCars()[currentCarIndex].gears.name;
            currentCarDescriptionText.text = string.IsNullOrEmpty(carsManager.GetAviableCars()[currentCarIndex].data.Description) ? "--" : carsManager.GetAviableCars()[currentCarIndex].data.Description;

            UpdateDropdowns();
        }
    }
}