using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Base.GameEventSystem
{
    [System.Serializable]
    public class GameEventHandler : UnityEvent<GameEventData>
    {
        
    }
}

