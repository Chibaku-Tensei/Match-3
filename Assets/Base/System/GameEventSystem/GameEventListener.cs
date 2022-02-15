using System;

namespace Base.GameEventSystem
{
    [Serializable]
    public class GameEventListener
    {
        public GameEvent Event;
        public GameEventHandler EventHandler;
    

        public void RegisterEvent()
        {
            Event.RegisterListener(this);
        }

        public void UnRegisterEvent()
        {
            Event.UnRegisterListener(this);
        }

        public void OnEventInvoke(GameEventData eventData)
        {
            EventHandler?.Invoke(eventData);
        }
    }
}

