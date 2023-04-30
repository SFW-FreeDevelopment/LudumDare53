using System.Collections.Generic;
using System.Linq;
using LD53.Abstractions;
using LD53.Behaviors;
using UnityEngine;

namespace LD53.Managers
{
    public class AudioManager : GameSingleton<AudioManager>
    {
        [SerializeField] private GameObject _audioObjectPrefab;

        private readonly IDictionary<string, AudioClip> _audioClipDictionary = new Dictionary<string, AudioClip>();

        protected override void InitSingletonInstance()
        {
            var audioClips = Resources.LoadAll<AudioClip>("Audio");
            if (audioClips?.Any() ?? false)
            {
                foreach (var audioClip in audioClips)
                {
                    if (!_audioClipDictionary.TryGetValue(audioClip.name, out _))
                    {
                        _audioClipDictionary.Add(audioClip.name.ToLower(), audioClip);
                    }
                }
            }
        }

        public void Play(string clipName)
        {
            if (_audioClipDictionary.TryGetValue(clipName.ToLower(), out var clip))
            {
                var spawnedObj = Instantiate(_audioObjectPrefab);
                var audioSource = spawnedObj.GetComponent<AudioSource>();
                audioSource.clip = clip;
                try
                {
                    audioSource.volume = SettingsManager.Instance.Settings.SfxVolume;
                }
                catch
                {
                    // do nothing
                }
                audioSource.Play();
            }
        }
    }
}