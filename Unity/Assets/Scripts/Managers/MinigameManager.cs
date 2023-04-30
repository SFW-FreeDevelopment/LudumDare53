using LD53.Abstractions;
using UnityEngine;

namespace LD53.Managers
{
    public class MinigameManager : SceneSingleton<MinigameManager>
    {
        public ushort DayNumber { get; private set; } = 1;
        
        protected override void InitSingletonInstance()
        {
            EventManager.OnGameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            EventManager.OnGameOver -= OnGameOver;
        }
        
        private void OnGameOver()
        {
            Debug.Log("Game over!");
        }

        private void EndDay()
        {
            // TODO: Show joke
        }

        private void StartDay()
        {
            // TODO: Show proverb
        }
    }
}