using LD53.Abstractions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LD53.Managers
{
    public class MinigameManager : SceneSingleton<MinigameManager>
    {
        [Header("Buttons")]
        [SerializeField] private Button _driveAwayButton;

        [Header("Views")]
        [SerializeField] private GameObject _truckCanvas;
        [SerializeField] private GameObject _truckSprites;
        [SerializeField] private GameObject _overworldCanvas;
        [SerializeField] private GameObject _overworldSprites;
        [SerializeField] private GameObject _pauseMenu;
        
        [Header("Misc")]
        [SerializeField] private AudioSource _musicAudioSource;
        
        public ushort DayNumber { get; private set; } = 1;

        private bool _isPaused = false;
        
        protected override void InitSingletonInstance()
        {
            try
            {
                _musicAudioSource.volume = SettingsManager.Instance.Settings.MusicVolume;
            }
            catch
            {
                // do nothing
            }
            
            EventManager.OnGameOver += OnGameOver;
            
            _driveAwayButton.onClick.AddListener(DriveAway);
        }

        private void OnDestroy()
        {
            EventManager.OnGameOver -= OnGameOver;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            _pauseMenu.SetActive(_isPaused);
            if (!_isPaused)
            {
                try
                {
                    _musicAudioSource.volume = SettingsManager.Instance.Settings.MusicVolume;
                }
                catch
                {
                    // do nothing
                }
                _musicAudioSource.Play();
            }
            else
            {
                _musicAudioSource.Pause();
            }
        }

        public void Quit()
        {
            // TODO: Write stats
            SceneManager.LoadScene("Menu");
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

        private void DriveUp()
        {
            _truckCanvas.SetActive(true);
            _truckSprites.SetActive(true);
            _overworldCanvas.SetActive(false);
            _overworldSprites.SetActive(false);
        }
        
        private void DriveAway()
        {
            _overworldCanvas.SetActive(true);
            _overworldSprites.SetActive(true);
            _truckCanvas.SetActive(false);
            _truckSprites.SetActive(false);
        }
    }
}