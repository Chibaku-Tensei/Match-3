using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Item")]
    public class Item : ScriptableObject
    {
        public int value;

        public Material materialColor;

        public Sprite sprite;
    }
}

