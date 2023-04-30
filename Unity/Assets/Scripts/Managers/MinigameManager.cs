using LD53.Abstractions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LD53.Managers
{
    public class MinigameManager : SceneSingleton<MinigameManager>
    {
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _dayText, _moneyText;
        
        [Header("Buttons")]
        [SerializeField] private Button _driveAwayButton;

        [Header("Views")]
        [SerializeField] private GameObject _truckCanvas;
        [SerializeField] private GameObject _truckSprites;
        [SerializeField] private GameObject _overworldCanvas;
        [SerializeField] private GameObject _overworldSprites;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _jokeWindow, _proverbWindow;
        
        [Header("Misc")]
        [SerializeField] private AudioSource _musicAudioSource;
        
        public ushort DayNumber { get; private set; } = 1;
        private bool _isPaused = false;
        private bool _gameOver = false;
        private Coroutine _timerRoutine;
        private ushort _currentTime = 0;
        
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
            EventManager.OnDayOver += EndDay;
            
            _driveAwayButton.onClick.AddListener(DriveAway);
            
            _timerRoutine = StartCoroutine(CoroutineTemplate.DelayAndFireLoopRoutine(1, () =>
            {
                if (_gameOver) return;
                if (_isPaused) return;
                _currentTime++;
                if (_currentTime == 120)
                {
                    EventManager.DayOver();
                }
                _timerText.text = $"{(_currentTime / 60) + 1}:{_currentTime % 60:00}";
            }));
        }

        private void OnDestroy()
        {
            EventManager.OnGameOver -= OnGameOver;
            EventManager.OnDayOver -= EndDay;
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
            AudioManager.Instance.Play("click");
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
            StopCoroutine(_timerRoutine);
            _timerRoutine = null;
            _jokeWindow.SetActive(true);
        }

        private void StartDay()
        {
            DayNumber++;
            _proverbWindow.SetActive(true);
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