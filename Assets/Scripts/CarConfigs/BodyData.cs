﻿using Assets.Scripts.CarConfigs;
using System;
using UnityEngine;

namespace Assets.Scripts.CarConfig
{
    [Serializable]
    public class BodyData
    {
        public BodyType bodyType;
        public Color color = Color.black;
        public Material material;
    }
}