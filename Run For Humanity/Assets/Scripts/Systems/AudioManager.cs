using UnityEngine;
using System.Collections.Generic;
using RunForHumanity.Core;

namespace RunForHumanity.Systems
{
    /// <summary>
    /// Audio management system
    /// SOLID: Single Responsibility - Manages all audio
    /// </summary>
    public class AudioManager : MonoBehaviour, IInitializable
    {
        [System.Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            [Range(0f, 1f)] public float volume = 1f;
            [Range(0f, 3f)] public float pitch = 1f;
            public bool loop;
            [HideInInspector] public AudioSource source;
        }
        
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        
        [Header("Sound Library")]
        [SerializeField] private List<Sound> _musicTracks = new List<Sound>();
        [SerializeField] private List<Sound> _sfxSounds = new List<Sound>();
        
        private Dictionary<string, Sound> _musicDict = new Dictionary<string, Sound>();
        private Dictionary<string, Sound> _sfxDict = new Dictionary<string, Sound>();
        
        public bool IsInitialized { get; private set; }
        
        public void Initialize()
        {
            // Create audio sources if not assigned
            if (_musicSource == null)
            {
                GameObject musicGO = new GameObject("MusicSource");
                musicGO.transform.SetParent(transform);
                _musicSource = musicGO.AddComponent<AudioSource>();
                _musicSource.loop = true;
            }
            
            if (_sfxSource == null)
            {
                GameObject sfxGO = new GameObject("SFXSource");
                sfxGO.transform.SetParent(transform);
                _sfxSource = sfxGO.AddComponent<AudioSource>();
            }
            
            // Build dictionaries for fast lookup
            foreach (var music in _musicTracks)
            {
                if (!_musicDict.ContainsKey(music.name))
                    _musicDict.Add(music.name, music);
            }
            
            foreach (var sfx in _sfxSounds)
            {
                if (!_sfxDict.ContainsKey(sfx.name))
                    _sfxDict.Add(sfx.name, sfx);
            }
            
            ServiceLocator.RegisterService(this);
            IsInitialized = true;
            Debug.Log("[AudioManager] Initialized with " + _musicDict.Count + " music tracks and " + _sfxDict.Count + " SFX");
        }
        
        public void PlayMusic(string name, bool fadeIn = true)
        {
            if (_musicDict.TryGetValue(name, out Sound music))
            {
                if (_musicSource.isPlaying && fadeIn)
                {
                    // Fade out current, then fade in new
                    StartCoroutine(FadeMusic(music));
                }
                else
                {
                    _musicSource.clip = music.clip;
                    _musicSource.volume = music.volume * GameConstants.MUSIC_VOLUME;
                    _musicSource.pitch = music.pitch;
                    _musicSource.loop = music.loop;
                    _musicSource.Play();
                }
            }
            else
            {
                Debug.LogWarning($"[AudioManager] Music '{name}' not found!");
            }
        }
        
        public void StopMusic(bool fadeOut = true)
        {
            if (fadeOut)
            {
                StartCoroutine(FadeOutMusic());
            }
            else
            {
                _musicSource.Stop();
            }
        }
        
        public void PlaySFX(string name)
        {
            if (_sfxDict.TryGetValue(name, out Sound sfx))
            {
                _sfxSource.PlayOneShot(sfx.clip, sfx.volume * GameConstants.SFX_VOLUME);
            }
            else
            {
                Debug.LogWarning($"[AudioManager] SFX '{name}' not found!");
            }
        }
        
        public void PlaySFX(string name, Vector3 position)
        {
            if (_sfxDict.TryGetValue(name, out Sound sfx))
            {
                AudioSource.PlayClipAtPoint(sfx.clip, position, sfx.volume * GameConstants.SFX_VOLUME);
            }
        }
        
        public void SetMusicVolume(float volume)
        {
            _musicSource.volume = Mathf.Clamp01(volume) * GameConstants.MUSIC_VOLUME;
        }
        
        public void SetSFXVolume(float volume)
        {
            _sfxSource.volume = Mathf.Clamp01(volume) * GameConstants.SFX_VOLUME;
        }
        
        private System.Collections.IEnumerator FadeMusic(Sound newMusic)
        {
            float fadeTime = 1f;
            float elapsed = 0;
            float startVolume = _musicSource.volume;
            
            // Fade out
            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0, elapsed / fadeTime);
                yield return null;
            }
            
            // Switch music
            _musicSource.clip = newMusic.clip;
            _musicSource.Play();
            
            // Fade in
            elapsed = 0;
            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(0, newMusic.volume * GameConstants.MUSIC_VOLUME, elapsed / fadeTime);
                yield return null;
            }
        }
        
        private System.Collections.IEnumerator FadeOutMusic()
        {
            float fadeTime = 1f;
            float elapsed = 0;
            float startVolume = _musicSource.volume;
            
            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0, elapsed / fadeTime);
                yield return null;
            }
            
            _musicSource.Stop();
        }
    }
}
