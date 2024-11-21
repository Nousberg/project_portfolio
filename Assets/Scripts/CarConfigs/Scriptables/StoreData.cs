using UnityEngine;

namespace Assets.Scripts.CarConfigs.Scriptables
{
    public class StoreData : ScriptableObject
    {
        [field: Header("Shop Properties")]
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: TextArea] [field: SerializeField] public string Description { get; private set; }
    }
}