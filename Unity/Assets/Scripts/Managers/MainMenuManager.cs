using LD53.Abstractions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LD53.Managers
{
    public class MainMenuManager : SceneSingleton<MainMenuManager>
    {
        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _creditsButton;
        
        [Header("Windows")]
        [SerializeField] private GameObject _settingsWindow;
        [SerializeField] private GameObject _creditsWindow;
        
        protected override void InitSingletonInstance()
        {
            _startButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.Play("click");
                SceneManager.LoadScene("Minigame");
            });
            _settingsButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.Play("click");
                _settingsWindow.SetActive(true);
            });
            _creditsButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.Play("click");
                _creditsWindow.SetActive(true);
            });
        }
    }
}