using NaughtyAttributes;
using UnityEngine;

namespace Base.AssetReference
{
    [System.Serializable]
    public struct PrefabRefItem
    {
        public string nameOfPrefab;
        public Transform prefab;
    }

    [System.Serializable]
    public struct SoundRefItem
    {
        public string nameOfSound;
        public AudioClip clip;
    }
}
