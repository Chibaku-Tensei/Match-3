using System;


namespace Base.GameEventSystem
{
    [Serializable]
    public class GameEventData : EventArgs
    {
        public object Data { get; set; }
        
        public GameEventData() {}

        public GameEventData(object data)
        {
            Data = data;
        }
    }
}
