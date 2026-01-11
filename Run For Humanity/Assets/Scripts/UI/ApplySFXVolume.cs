using UnityEngine;
using RunForHumanity.Core;

namespace RunForHumanity.UI
{
    // Aplica el volumen de SFX del settings
    [RequireComponent(typeof(AudioSource))]
    public class ApplySFXVolume : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            ApplyVolume();
        }

        private void OnEnable()
        {
            ApplyVolume();
        }
        
        private void ApplyVolume()
        {
            if (audioSource != null)
            {
                audioSource.volume = GameSettingsManager.Instance.GetNormalizedSFXVolume();
            }
        }
    }
}
