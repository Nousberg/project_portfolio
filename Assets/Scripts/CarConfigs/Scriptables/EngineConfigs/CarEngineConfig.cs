using Assets.Scripts.CarConfigs.Scriptables;
using UnityEngine;

namespace Assets.Scripts.CarConfigs
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CarConfigs/EngineData")]
    public class EngineConfigData : StoreData
    {
        [Header("References")]
        [SerializeField] private EngineData engineData;

        public EngineData GetEngine() => engineData;
    }
}