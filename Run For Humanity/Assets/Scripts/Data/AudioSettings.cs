using UnityEngine;

namespace RunForHumanity.Data
{
    /// <summary>
    /// Gestiona la configuración de audio del juego
    /// Almacena volúmenes y estados de habilitación para música y efectos de sonido
    /// </summary>
    [System.Serializable]
    public class AudioSettings
    {
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string MUSIC_ENABLED_KEY = "MusicEnabled";
        private const string SFX_VOLUME_KEY = "SFXVolume";
        private const string SFX_ENABLED_KEY = "SFXEnabled";
        
        public float musicVolume { get; set; } = 0.7f;
        public bool musicEnabled { get; set; } = true;
        public float sfxVolume { get; set; } = 0.7f;
        public bool sfxEnabled { get; set; } = true;
        
        /// <summary>
        /// Constructor por defecto con valores iniciales
        /// </summary>
        public AudioSettings()
        {
            musicVolume = 0.7f;
            musicEnabled = true;
            sfxVolume = 0.7f;
            sfxEnabled = true;
        }
        
        /// <summary>
        /// Carga la configuración de audio desde PlayerPrefs
        /// </summary>
        public void Load()
        {
            musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
            musicEnabled = PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1) == 1;
            sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.7f);
            sfxEnabled = PlayerPrefs.GetInt(SFX_ENABLED_KEY, 1) == 1;
            
            Debug.Log($"[AudioSettings] Configuración cargada - Music: {musicVolume} ({musicEnabled}), SFX: {sfxVolume} ({sfxEnabled})");
        }
        
        /// <summary>
        /// Guarda la configuración de audio en PlayerPrefs
        /// </summary>
        public void Save()
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
            PlayerPrefs.SetInt(MUSIC_ENABLED_KEY, musicEnabled ? 1 : 0);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
            PlayerPrefs.SetInt(SFX_ENABLED_KEY, sfxEnabled ? 1 : 0);
            PlayerPrefs.Save();
            
            Debug.Log($"[AudioSettings] Configuración guardada - Music: {musicVolume} ({musicEnabled}), SFX: {sfxVolume} ({sfxEnabled})");
        }
        
        /// <summary>
        /// Reinicia la configuración de audio a valores por defecto
        /// </summary>
        public void Reset()
        {
            musicVolume = 0.7f;
            musicEnabled = true;
            sfxVolume = 0.7f;
            sfxEnabled = true;
            
            Save();
            
            Debug.Log("[AudioSettings] Configuración reiniciada a valores por defecto");
        }
    }
}
