using UnityEngine;

namespace LD53.Abstractions
{
    public abstract class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
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