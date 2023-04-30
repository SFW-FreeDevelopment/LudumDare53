using LD53.Managers;
using LD53.Models;
using UnityEngine;
using UnityEngine.UI;

namespace LD53.UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private Slider _musicVolumeSlider, _sfxVolumeSlider;

        private static Settings Settings => SettingsManager.Instance?.Settings ?? new Settings();
        private static void Save()
        {
            SettingsManager.Instance.Save();
        }

        private void Start()
        {
            _musicVolumeSlider.onValueChanged.AddListener(value =>
            {
                Settings.MusicVolume = value;
                Save();
            });
            _sfxVolumeSlider.onValueChanged.AddListener(value =>
            {
                Settings.SfxVolume = value;
                Save();
            });
        }

        private void OnEnable()
        {
            _musicVolumeSlider.value = Settings.MusicVolume;
            _sfxVolumeSlider.value = Settings.SfxVolume;
        }
    }
}