using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base.GameEventSystem
{
    [CreateAssetMenu(menuName = "GameEventSystem/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        public void InvokeEvent(GameEventData data)
        {
            try
            {
                for (int i = listeners.Count - 1; i >= 0; i--)
                {
                    listeners[i].OnEventInvoke(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        
        }

        public void InvokeEvent()
        {
            try
            {
                for (int i = listeners.Count - 1; i >= 0; i--)
                {
                    listeners[i].OnEventInvoke(new GameEventData());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnRegisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}

