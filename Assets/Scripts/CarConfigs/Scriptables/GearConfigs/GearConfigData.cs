using Assets.Scripts.CarConfigs.Scriptables;
using UnityEngine;

namespace Assets.Scripts.CarConfigs
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CarConfigs/GearData")]
    public class GearConfigData : StoreData
    {
        [Header("References")]
        [SerializeField] private GearData gearData;

        public GearData GetGear() => gearData;
    }
}