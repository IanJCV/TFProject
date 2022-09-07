using System.Collections.Generic;

namespace TF.Base.Events
{
    /// <summary>
    /// Global events manager.
    /// </summary>
    public class GameEvents
    {
        private static GameEvents _instance;
        
        private static Dictionary<string, GameEvent> events;

        public static GameEvents GetInstance()
        {
            if (_instance is null)
                return new GameEvents();
            return _instance;
        }

        private GameEvents()
        {
            _instance = this;
            events = new Dictionary<string, GameEvent>();
        }

        public static void TriggerEvent(string name)
        {
            
        }

        public static void CreateEvent(string name)
        {
            
        }
        
    }

    public class GameEvent
    {
        
    }
}