using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RunForHumanity.Core;

namespace RunForHumanity.UI
{
    public class AudioSettingsController : MonoBehaviour
    {
        [Header("Ambient Music (just-relax-11157)")]
        [SerializeField] private Slider ambientVolumeSlider;
        [SerializeField] private TextMeshProUGUI ambientVolumeText;
        
        [Header("All Other Sounds (SFX)")]
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private TextMeshProUGUI sfxVolumeText;

        private void Start()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            GameSettingsManager settings = GameSettingsManager.Instance;
            
            if (ambientVolumeSlider != null)
            {
                ambientVolumeSlider.minValue = 0f;
                ambientVolumeSlider.maxValue = 100f;
                ambientVolumeSlider.wholeNumbers = true;
                ambientVolumeSlider.value = settings.AmbientVolume;
                ambientVolumeSlider.onValueChanged.AddListener(OnAmbientVolumeChanged);
            }
            
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.minValue = 0f;
                sfxVolumeSlider.maxValue = 100f;
                sfxVolumeSlider.wholeNumbers = true;
                sfxVolumeSlider.value = settings.SFXVolume;
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            }
            
            UpdateVolumeTexts();
            
            settings.ApplyAllVolumes();
        }

        private void OnAmbientVolumeChanged(float value)
        {
            GameSettingsManager.Instance.SetAmbientVolume(value);
            UpdateVolumeTexts();
        }

        private void OnSFXVolumeChanged(float value)
        {
            GameSettingsManager.Instance.SetSFXVolume(value);
            UpdateVolumeTexts();
        }

        private void UpdateVolumeTexts()
        {
            GameSettingsManager settings = GameSettingsManager.Instance;
            
            if (ambientVolumeText != null)
                ambientVolumeText.text = $"{Mathf.RoundToInt(settings.AmbientVolume)}";
            
            if (sfxVolumeText != null)
                sfxVolumeText.text = $"{Mathf.RoundToInt(settings.SFXVolume)}";
        }

        public void ResetToDefaults()
        {
            GameSettingsManager settings = GameSettingsManager.Instance;
            settings.ResetToDefaults();
            
            if (ambientVolumeSlider != null)
                ambientVolumeSlider.value = settings.AmbientVolume;
            
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.value = settings.SFXVolume;
            
            UpdateVolumeTexts();
            
            Debug.Log("[AudioSettingsController] Configuración reiniciada a valores por defecto");
        }
    }
}

