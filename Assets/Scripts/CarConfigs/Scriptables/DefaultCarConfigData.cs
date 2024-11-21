using Assets.Scripts.CarConfigs.Scriptables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CarConfigs
{
    [CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/CarConfigs/CarData")]
    public class DefaultCarConfigData : StoreData
    {
        [field: Header("Properties")]
        [field: SerializeField] public MoveType MovementType { get; private set; }

        [Space(25f)]
        [SerializeField] private List<BodyConfigData> allowedBodyConfigs = new List<BodyConfigData>();
        [SerializeField] private List<GearConfigData> allowedGearConfigs = new List<GearConfigData>();
        [SerializeField] private List<EngineConfigData> allowedEngineConfigs = new List<EngineConfigData>();

        [Space(25f)]
        [SerializeField] private List<BodyConfigData> defaultBodies;

        [Space(25f)]
        [SerializeField] private EngineConfigData defaultEngine;
        [SerializeField] private GearConfigData defaultGear;

        [Space(25f)]
        [SerializeField] private Vector3 enginePos;
        [SerializeField] private Vector3[] gearsPos = new Vector3[4];
        [SerializeField] private Vector3 carcassPos;
        [SerializeField] private Vector3 spoilerPos;

        private void OnValidate()
        {
            if (gearsPos.Length < 4 || gearsPos.Length > 4)
                gearsPos = new Vector3[4];

            if (defaultEngine != null && !allowedEngineConfigs.Contains(defaultEngine))
            {
                allowedEngineConfigs.Add(defaultEngine);
            }
            if (defaultGear != null && !allowedGearConfigs.Contains(defaultGear))
            {
                allowedGearConfigs.Add(defaultGear);
            }
        }

        public Vector3 GetEnginePos() => enginePos;
        public Vector3[] GetGearsPos() => gearsPos;
        public Vector3 GetCarcassPos() => carcassPos;
        public Vector3 GetSpoilerPos() => spoilerPos;

        public List<BodyConfigData> GetDefaultBodies() => defaultBodies;
        public EngineConfigData GetDefaultEngine() => defaultEngine;
        public GearConfigData GetDefaultGear() => defaultGear;

        public List<BodyConfigData> GetBodyConfigs() => allowedBodyConfigs;
        public List<GearConfigData> GetGearConfigs() => allowedGearConfigs;
        public List<EngineConfigData> GetEngineConfigs() => allowedEngineConfigs;
    }
    public enum BodyType : byte
    {
        Spoiler,
        Headlights,
        LeftDoor,
        Carcass
    }
    public enum GearSeasonType : byte
    {
        Summer,
        Winter,
        Universal
    }
    public enum EngineType : byte
    {
        Disel,
        Fuel,
        Gas
    }
    public enum MoveType : byte
    {
        Forward,
        Backward,
        Full
    }
}