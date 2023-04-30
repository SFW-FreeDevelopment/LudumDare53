using System;

namespace LD53.Managers
{
    public static class EventManager
    {
        public static event Action OnGameStarted;
        public static void GameStarted() => OnGameStarted?.Invoke();
        
        public static event Action OnGameOver;
        public static void GameOver() => OnGameOver?.Invoke();
        
        public static event Action OnDayOver;
        public static void DayOver() => OnDayOver?.Invoke();
        
        public static event Action OnDayStart;
        public static void DayStart() => OnDayStart?.Invoke();
    }
}