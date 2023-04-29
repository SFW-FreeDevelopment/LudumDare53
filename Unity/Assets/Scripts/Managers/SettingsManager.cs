using LD53.Abstractions;
using LD53.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace LD53.Managers
{
    public class SettingsManager : GameSingleton<SettingsManager>
    {
        public Settings Settings { get; private set; } = new();

        protected override void InitSingletonInstance()
        {
            Load();
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(Settings);
            PlayerPrefs.SetString("Settings", json);
        }

        private void Load()
        {
            if (PlayerPrefs.HasKey("Settings"))
            {
                var json = PlayerPrefs.GetString("Settings");
                try
                {
                    Settings = JsonConvert.DeserializeObject<Settings>(json) ?? new Settings();
                }
                catch
                {
                    Settings = new Settings();
                }
            }
            else
            {
                Settings = new Settings();
            }
        }
    }
}