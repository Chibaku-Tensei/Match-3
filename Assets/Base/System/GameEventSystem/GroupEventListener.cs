using System.Collections.Generic;
using UnityEngine;

namespace Base.GameEventSystem
{
    [AddComponentMenu("Custom Event System/Event Listener")]
    public class GroupEventListener : MonoBehaviour
    {
        [SerializeField] private List<GameEventListener> groupEventListener = new List<GameEventListener>();

        private void OnEnable()
        {
            foreach (var listener in groupEventListener)
            {
                listener.RegisterEvent();
            }
        }

        private void OnDisable()
        {
            foreach (var listener in groupEventListener)
            {
                listener.UnRegisterEvent();
            }
        }
    }
}

