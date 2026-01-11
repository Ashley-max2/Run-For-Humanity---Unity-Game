using UnityEngine;

namespace RunForHumanity.Core
{
    /// <summary>
    /// Singleton que persiste entre escenas para gestionar todos los ajustes del juego
    /// Incluye volumen de audio, gráficos, controles, etc.
    /// </summary>
    public class GameSettingsManager : MonoBehaviour
    {
        private static GameSettingsManager _instance;
        
        public static GameSettingsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Buscar si ya existe una instancia en la escena
                    _instance = FindObjectOfType<GameSettingsManager>();
                    
                    // Si no existe, crear una nueva
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameSettingsManager");
                        _instance = go.AddComponent<GameSettingsManager>();
                    }
                }
                return _instance;
            }
        }
        
        // Claves de PlayerPrefs
        private const string AMBIENT_VOLUME_KEY = "AmbientVolume";
        private const string SFX_VOLUME_KEY = "SFXVolume";
        
        // Valores de volumen (0-100)
        public float AmbientVolume { get; private set; } = 70f;
        public float SFXVolume { get; private set; } = 70f;
        
        // Referencia al AudioSource del ambiente (just-relax-11157)
        private AudioSource ambientAudioSource;
        
        private void Awake()
        {
            // Patrón Singleton - asegurar que solo exista una instancia
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
            
            // RESETEAR TODOS LOS DATOS AL INICIAR (para nueva cuenta)
            //ResetAllGameData();
            
            // Cargar configuración guardada
            LoadSettings();
            
            Debug.Log("[GameSettingsManager] Inicializado - Ambiente: " + AmbientVolume + ", SFX: " + SFXVolume);
        }
        
        /// <summary>
        /// Resetea TODOS los datos del juego (monedas, multiplicador, ajustes)
        /// </summary>
        private void ResetAllGameData()
        {
            // Resetear monedas
            Data.CoinDataManager.ResetCoins();
            
            // Resetear datos de tienda (multiplicador)
            Data.ShopDataManager.ResetData();
            
            // Resetear PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            
            Debug.Log("[GameSettingsManager] ===== TODOS LOS DATOS RESETEADOS A 0 =====");
        }
        
        private void Start()
        {
            // Aplicar configuración al inicio de forma segura
            // Usar Invoke para evitar bloqueo en el Preloader
            Invoke(nameof(InitializeAudio), 0.1f);
        }
        
        private void InitializeAudio()
        {
            FindAndConfigureAmbientAudio();
            ApplyAllVolumes();
        }
        
        /// <summary>
        /// Cargar configuración guardada desde PlayerPrefs
        /// </summary>
        private void LoadSettings()
        {
            AmbientVolume = PlayerPrefs.GetFloat(AMBIENT_VOLUME_KEY, 70f);
            SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 70f);
        }
        
        /// <summary>
        /// Guardar configuración en PlayerPrefs
        /// </summary>
        private void SaveSettings()
        {
            PlayerPrefs.SetFloat(AMBIENT_VOLUME_KEY, AmbientVolume);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, SFXVolume);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Buscar el AudioSource del ambiente
        /// </summary>
        private void FindAndConfigureAmbientAudio()
        {
            try
            {
                AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>(true);
                
                foreach (AudioSource source in allAudioSources)
                {
                    if (source != null && source.clip != null && source.clip.name.Contains("just-relax-11157"))
                    {
                        ambientAudioSource = source;
                        ambientAudioSource.volume = AmbientVolume / 100f;
                        Debug.Log("[GameSettingsManager] Audio ambiente encontrado y configurado");
                        break;
                    }
                }
                
                if (ambientAudioSource == null)
                {
                    Debug.Log("[GameSettingsManager] No se encontró audio ambiente (normal en Preloader)");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[GameSettingsManager] Error buscando audio: {e.Message}");
            }
        }
        
        /// <summary>
        /// Cambiar el volumen del ambiente
        /// </summary>
        public void SetAmbientVolume(float volume)
        {
            AmbientVolume = Mathf.Clamp(volume, 0f, 100f);
            
            // Aplicar al AudioSource si existe
            if (ambientAudioSource != null)
            {
                ambientAudioSource.volume = AmbientVolume / 100f;
            }
            else
            {
                FindAndConfigureAmbientAudio();
            }
            
            SaveSettings();
            Debug.Log("[GameSettingsManager] Volumen ambiente cambiado a: " + AmbientVolume);
        }
        
        /// <summary>
        /// Cambiar el volumen de SFX (todos los sonidos excepto ambiente)
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            SFXVolume = Mathf.Clamp(volume, 0f, 100f);
            
            // Aplicar a todos los AudioSources excepto el ambiente
            ApplySFXVolumeToAll();
            
            SaveSettings();
            Debug.Log("[GameSettingsManager] Volumen SFX cambiado a: " + SFXVolume);
        }
        
        /// <summary>
        /// Aplicar volumen de SFX a todos los AudioSources existentes (excepto ambiente)
        /// </summary>
        private void ApplySFXVolumeToAll()
        {
            try
            {
                AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>(true);
                
                foreach (AudioSource source in allAudioSources)
                {
                    if (source == null) continue;
                    
                    // Saltar el audio de ambiente
                    if (source == ambientAudioSource)
                        continue;
                    
                    // Verificar si es el ambiente por nombre
                    if (source.clip != null && source.clip.name.Contains("just-relax-11157"))
                        continue;
                    
                    // Aplicar volumen SFX
                    source.volume = SFXVolume / 100f;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[GameSettingsManager] Error aplicando volumen SFX: {e.Message}");
            }
        }
        
        /// <summary>
        /// Aplicar todos los volúmenes guardados
        /// </summary>
        public void ApplyAllVolumes()
        {
            // Aplicar volumen de ambiente
            if (ambientAudioSource == null)
            {
                FindAndConfigureAmbientAudio();
            }
            else
            {
                ambientAudioSource.volume = AmbientVolume / 100f;
            }
            
            // Aplicar volumen de SFX
            ApplySFXVolumeToAll();
        }
        
        /// <summary>
        /// Reiniciar todos los ajustes a valores por defecto
        /// </summary>
        public void ResetToDefaults()
        {
            AmbientVolume = 70f;
            SFXVolume = 70f;
            
            ApplyAllVolumes();
            SaveSettings();
            
            Debug.Log("[GameSettingsManager] Configuración reiniciada a valores por defecto");
        }
        
        /// <summary>
        /// Obtener el volumen normalizado (0-1) para usar en AudioSource
        /// </summary>
        public float GetNormalizedSFXVolume()
        {
            return SFXVolume / 100f;
        }
        
        /// <summary>
        /// Obtener el volumen normalizado (0-1) para usar en AudioSource
        /// </summary>
        public float GetNormalizedAmbientVolume()
        {
            return AmbientVolume / 100f;
        }
        
        /// <summary>
        /// Llamar cuando se carga una nueva escena para reconfigurar audios
        /// </summary>
        public void OnSceneLoaded()
        {
            // Buscar nuevamente el audio ambiente por si cambió de escena
            ambientAudioSource = null;
            FindAndConfigureAmbientAudio();
            
            // Aplicar volúmenes a todos los audios de la nueva escena
            ApplyAllVolumes();
        }
    }
}
