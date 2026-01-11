using UnityEngine;

namespace RunForHumanity.Audio
{
    // Música de fondo persistente entre escenas (Singleton)
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusicManager : MonoBehaviour
    {
        [Header("Music Settings")]
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField][Range(0f, 1f)] private float volume = 0.3f;
        [SerializeField] private bool playOnAwake = true;
        [SerializeField] private bool loop = true;
        
        [Header("Fade Settings")]
        [SerializeField] private bool useFadeIn = true;
        [SerializeField] private float fadeInDuration = 2f;
        [SerializeField] private bool useFadeOut = true;
        [SerializeField] private float fadeOutDuration = 1f;
        
        private AudioSource audioSource;
        private static BackgroundMusicManager instance;
        private bool isFading = false;
        private float targetVolume;
        
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.Log("[BackgroundMusic] Ya existe una instancia, destruyendo duplicado");
                Destroy(gameObject);
                return;
            }
            
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[BackgroundMusic] ✓ Instancia creada y marcada como persistente");
            
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                Debug.LogError("[BackgroundMusic] ⚠ AudioSource no encontrado! Agregándolo automáticamente...");
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            if (backgroundMusic == null)
            {
                Debug.LogError("[BackgroundMusic] ⚠ No hay clip de música asignado! Asigna un AudioClip en el Inspector.");
                return;
            }
            
            audioSource.clip = backgroundMusic;
            audioSource.loop = loop;
            audioSource.playOnAwake = false;
            targetVolume = volume;
            
            Debug.Log($"[BackgroundMusic] Configurado - Clip: {backgroundMusic.name}, Volumen: {volume * 100}%");
            
            if (playOnAwake)
            {
                PlayMusic();
                Debug.Log("[BackgroundMusic] ♪ Reproduciendo música de fondo");
            }
        }
        
        public void PlayMusic()
        {
            if (audioSource == null)
            {
                Debug.LogError("[BackgroundMusic] AudioSource es null!");
                return;
            }
            
            if (backgroundMusic == null)
            {
                Debug.LogError("[BackgroundMusic] No hay clip de música asignado!");
                return;
            }
            
            if (!audioSource.isPlaying)
            {
                if (useFadeIn)
                {
                    audioSource.volume = 0f;
                    audioSource.Play();
                    FadeIn();
                    Debug.Log($"[BackgroundMusic] Reproduciendo con fade in ({fadeInDuration}s)");
                }
                else
                {
                    audioSource.volume = volume;
                    audioSource.Play();
                    Debug.Log($"[BackgroundMusic] Reproduciendo a volumen {volume * 100}%");
                }
            }
            else
            {
                Debug.Log("[BackgroundMusic] La música ya se está reproduciendo");
            }
        }
        
        public void PauseMusic()
        {
            if (audioSource == null || !audioSource.isPlaying) return;
            
            if (useFadeOut)
            {
                FadeOut(true);
            }
            else
            {
                audioSource.Pause();
            }
        }
        
        public void StopMusic()
        {
            if (audioSource == null) return;
            
            if (useFadeOut && audioSource.isPlaying)
            {
                FadeOut(false);
            }
            else
            {
                audioSource.Stop();
            }
        }
        
        public void SetVolume(float newVolume)
        {
            volume = Mathf.Clamp01(newVolume);
            targetVolume = volume;
            if (!isFading)
            {
                audioSource.volume = volume;
            }
        }
        
        public void ChangeMusic(AudioClip newClip, bool fadeTransition = true)
        {
            if (newClip == null) return;
            
            if (fadeTransition && audioSource.isPlaying)
            {
                StartCoroutine(ChangeWithFade(newClip));
            }
            else
            {
                audioSource.Stop();
                audioSource.clip = newClip;
                backgroundMusic = newClip;
                PlayMusic();
            }
        }
        
        public void SetMute(bool mute)
        {
            if (audioSource != null)
            {
                audioSource.mute = mute;
            }
        }
        
        private void FadeIn()
        {
            if (isFading) StopAllCoroutines();
            StartCoroutine(FadeCoroutine(targetVolume, fadeInDuration));
        }
        
        private void FadeOut(bool pauseAfter)
        {
            if (isFading) StopAllCoroutines();
            StartCoroutine(FadeCoroutine(0f, fadeOutDuration, pauseAfter));
        }
        
        private System.Collections.IEnumerator FadeCoroutine(float targetVol, float duration, bool pauseAfter = false)
        {
            isFading = true;
            float startVolume = audioSource.volume;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVol, elapsed / duration);
                yield return null;
            }
            
            audioSource.volume = targetVol;
            isFading = false;
            
            if (pauseAfter)
            {
                audioSource.Pause();
            }
        }
        
        private System.Collections.IEnumerator ChangeWithFade(AudioClip newClip)
        {
            yield return FadeCoroutine(0f, fadeOutDuration);
            
            audioSource.Stop();
            audioSource.clip = newClip;
            backgroundMusic = newClip;
            
            audioSource.Play();
            yield return FadeCoroutine(targetVolume, fadeInDuration);
        }
        
        public static BackgroundMusicManager Instance => instance;
        public bool IsPlaying => audioSource != null && audioSource.isPlaying;
        public float CurrentVolume => audioSource != null ? audioSource.volume : 0f;
    }
}
