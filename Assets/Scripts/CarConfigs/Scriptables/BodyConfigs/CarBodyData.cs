using Assets.Scripts.CarConfig;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.CarConfigs.Scriptables
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CarConfigs/BodyData")]
    public class BodyConfigData : StoreData
    {
        [Header("References")]
        [SerializeField] private BodyData bodyData;

        public BodyData GetBody() => bodyData;
    }
}