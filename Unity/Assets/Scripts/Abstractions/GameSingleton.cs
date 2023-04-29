using UnityEngine;

namespace LD53.Abstractions
{
    public abstract class GameSingleton<T> : MonoBehaviour where T : GameSingleton<T>
    {
        public static T Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                Instance = (T)this;
            }
        }

        private void Start()
        {
            if (Instance == this)
                InitSingletonInstance();
        }

        protected abstract void InitSingletonInstance();
    }
}