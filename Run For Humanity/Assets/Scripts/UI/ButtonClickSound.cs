using UnityEngine;
using UnityEngine.UI;
using RunForHumanity.Core;

namespace RunForHumanity.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonClickSound : MonoBehaviour
    {
        [Header("Sound Settings")]
        [Tooltip("Arrastra aquí el archivo de audio (mp3, wav, etc.)")]
        [SerializeField] private AudioClip soundClip;
        
        [Tooltip("Volumen del sonido (0-1)")]
        [Range(0f, 1f)]
        [SerializeField] private float volume = 1f;
        
        private Button button;
        private static AudioSource _sharedAudioSource;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PlaySound);
            
            if (_sharedAudioSource == null)
            {
                GameObject audioGO = new GameObject("ButtonClickAudioSource");
                _sharedAudioSource = audioGO.AddComponent<AudioSource>();
                _sharedAudioSource.playOnAwake = false;
                _sharedAudioSource.loop = false;
                _sharedAudioSource.spatialBlend = 0f;
                _sharedAudioSource.priority = 0;
                DontDestroyOnLoad(audioGO);
                Debug.Log("[ButtonClickSound] AudioSource compartido creado");
            }
        }

        private void Start()
        {
            if (soundClip == null)
            {
                Debug.LogWarning($"[ButtonClickSound] No hay AudioClip asignado en el botón '{gameObject.name}'");
            }
        }

        private void OnDestroy()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(PlaySound);
            }
        }

        private void PlaySound()
        {
            if (soundClip != null && _sharedAudioSource != null)
            {
                float sfxVolume = GameSettingsManager.Instance.GetNormalizedSFXVolume();
                _sharedAudioSource.PlayOneShot(soundClip, volume * sfxVolume);
                Debug.Log($"[ButtonClickSound] Reproduciendo: {soundClip.name} con volumen {volume * sfxVolume}");
            }
        }
    }
}
