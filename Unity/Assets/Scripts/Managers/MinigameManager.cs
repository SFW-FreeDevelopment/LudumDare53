using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LD53.Abstractions;
using LD53.Behaviors;
using LD53.Clients;
using LudumDare53.API.Models;
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
        [SerializeField] private GameObject _gameOverWindow;
        
        [Header("Misc")]
        [SerializeField] private AudioSource _musicAudioSource;
        
        public ushort DayNumber { get; private set; } = 1;
        public int ConesServed { get; set; } = 0;
        private bool _isPaused = false;
        public bool IsPaused => _isPaused;
        private bool _gameOver = false;
        private Coroutine _timerRoutine;
        private ushort _currentTime = 0;
        private int _cash = 0;
        private int _totalCash = 0;
        public int Cash => _totalCash;
        [SerializeField] private GameObject[] _houses;
        [SerializeField] private List<GameObject> _activeHouses = new List<GameObject>();
        public GameObject LastHouse { get; set; }
        public int LastHouseGameObjectId { get; set; }
        public (float x, float y) LastHouseCoordinates { get; set; }
        
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

            _houses = GameObject.FindGameObjectsWithTag("House");
            
            EventManager.OnGameOver += OnGameOver;
            EventManager.OnDayOver += EndDay;
            EventManager.OnDayStart += StartDay;
            EventManager.OnIceCreamDelivered += OnIceCreamDelivered;
            
            _driveAwayButton.onClick.AddListener(DriveAway);

            StartTime();

            for (var i = 0; i < 5; i++)
            {
                var rng = UnityEngine.Random.Range(0, 10);
                var index = (i * 10) + rng;
                _activeHouses.Add(_houses[index]);
            }

            foreach (var house in _activeHouses)
            {
                var houseComponent = house.GetComponent<House>();
                houseComponent.Active = true;
                house.GetComponentInChildren<ConeIndicator>().gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        private void StartTime()
        {
            _timerText.text = $"{(_currentTime / 60) + 1}:{_currentTime % 60:00} PM";
            _timerRoutine = StartCoroutine(CoroutineTemplate.DelayAndFireLoopRoutine(1, () =>
            {
                if (_gameOver) return;
                if (_isPaused) return;
                _currentTime += 5;
                if (_currentTime == 180)
                {
                    EventManager.DayOver();
                }
                _timerText.text = $"{(_currentTime / 60) + 1}:{_currentTime % 60:00} PM";
            }));
        }

        private void OnDestroy()
        {
            EventManager.OnGameOver -= OnGameOver;
            EventManager.OnDayOver -= EndDay;
            EventManager.OnDayStart -= StartDay;
            EventManager.OnIceCreamDelivered -= OnIceCreamDelivered;
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

        public void EndSession()
        {
            EventManager.GameOver();
        }
        
        public void Quit()
        {
            AudioManager.Instance.Play("click");
            PostResults();
            SceneManager.LoadScene("Menu");
        }

        private async Task PostResults()
        {
            try
            {
                ApiClient.ProcessGameResults(new PlayerDto
                {
                    Name = $"Anonymous_{System.Guid.NewGuid().ToString("N")}",
                    DaysCompleted = DayNumber,
                    TotalMoneyEarned = Cash,
                    DeliveriesMade = ConesServed

                }, null);
            }
            catch
            {
                // do nothing
            }
            await Task.CompletedTask;
        }
        
        private void OnGameOver()
        {
            _gameOverWindow.SetActive(true);
        }
        
        private void OnIceCreamDelivered(int money)
        {
            _cash += money;
            _totalCash += money;
            ConesServed++;
            _moneyText.text = _cash.ToString("C0");
        }

        private void EndDay()
        {
            StopCoroutine(_timerRoutine);
            DriveAway();
            _timerRoutine = null;
            _proverbWindow.SetActive(true);
            AudioManager.Instance.Play("end-day");
        }

        public void CloseProverbWindow()
        {
            EventManager.DayStart();
        }
        
        private void StartDay()
        {
            AudioManager.Instance.Play("register");
            _cash -= 20;
            if (_cash < 0)
            {
                EventManager.GameOver();
                return;
            }
            _currentTime = 0;
            DayNumber++;
            _dayText.text = $"Day {DayNumber}";
            StartTime();
            AudioManager.Instance.Play("rooster");
            _moneyText.text = _cash.ToString("C0");
        }

        public void DeactivateHouse(House house)
        {
            _activeHouses.Remove(house.gameObject);
            house.Active = false;
            house.GetComponentInChildren<ConeIndicator>().gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        private void ActivateHouse()
        {
            var inactiveHouses = _houses.Except(_activeHouses);
            var index = UnityEngine.Random.Range(0, inactiveHouses.Count());
            GameObject house = null;
            int i = 0;
            foreach (var obj in inactiveHouses)
            {
                if (i == index)
                {
                    house = obj;
                    break;
                }
                i++;
            }
            
            _activeHouses.Add(house);
            
            var houseComponent = house.GetComponent<House>();
            houseComponent.Active = true;
            LastHouse = house;
            LastHouseGameObjectId = LastHouse.GetInstanceID();
            LastHouseCoordinates = (house.transform.position.x, house.transform.position.y);
            house.GetComponentInChildren<ConeIndicator>().gameObject.GetComponent<SpriteRenderer>().enabled = true;
            house.GetComponent<BoxCollider2D>().enabled = false;
        }
        
        public void DriveUp(House house)
        {
            DeactivateHouse(house);
            ActivateHouse();
            AudioManager.Instance.Play("click");
            _truckCanvas.SetActive(true);
            _truckSprites.SetActive(true);
            _overworldCanvas.SetActive(false);
            _overworldSprites.SetActive(false);
            TruckManager.Instance.DriveUp();
        }
        
        public void DriveAway()
        {
            AudioManager.Instance.Play("click");
            _overworldCanvas.SetActive(true);
            _overworldSprites.SetActive(true);
            _truckCanvas.SetActive(false);
            _truckSprites.SetActive(false);
            if (LastHouse != null)
            {
                LastHouse.GetComponent<House>().JustLeft = true;
                LastHouse.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
}