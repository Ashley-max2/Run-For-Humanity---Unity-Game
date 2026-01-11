using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace RunForHumanity.UI
{
    /// <summary>
    /// Gestiona la pantalla de carga (preloader) con barra de progreso y tips.
    /// Coloca este script en la escena Preloader.
    /// </summary>
    public class PreloaderManager : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("Arrastra aquí un Slider o una Image con tipo Fill")]
        [SerializeField] private Slider progressSlider;
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI tipText;
        
        [Header("Settings")]
        [SerializeField] private string targetSceneName = "MainMenu";
        [SerializeField] private float minimumLoadTime = 2f;
        [SerializeField] private bool simulateLoading = true;
        
        [Header("Loading Tips")]
        [SerializeField] private string[] loadingTips = new string[]
        {
            "Corre por la humanidad, cada paso cuenta...",
            "Recoge monedas para donar a ONGs",
            "Desliza hacia abajo para evitar obstáculos",
            "Usa power-ups para mejorar tu rendimiento",
            "¡Compite contra tus amigos en el ranking!",
            "Cada carrera ayuda a cambiar el mundo"
        };
        
        private void Start()
        {
            Debug.Log("[Preloader] =================================");
            Debug.Log("[Preloader] Iniciando PreloaderManager");
            Debug.Log("[Preloader] =================================");
            
            // Buscar referencias automáticamente si no están asignadas
            try
            {
                AutoFindUIElements();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Preloader] Error buscando UI: {e.Message}");
            }
            
            // Mostrar tip aleatorio
            if (tipText != null && loadingTips.Length > 0)
            {
                tipText.text = loadingTips[Random.Range(0, loadingTips.Length)];
                Debug.Log("[Preloader] Tip mostrado");
            }
            else
            {
                Debug.LogWarning("[Preloader] No se pudo mostrar tip");
            }
            
            Debug.Log("[Preloader] Iniciando carga de escena: " + targetSceneName);
            
            // Iniciar carga
            StartCoroutine(LoadSceneAsync());
        }
        
        private void AutoFindUIElements()
        {
            // Buscar Progress Slider primero
            if (progressSlider == null)
            {
                GameObject sliderObj = GameObject.Find("ProgressBar");
                if (sliderObj == null) sliderObj = GameObject.Find("Progress Bar");
                if (sliderObj == null) sliderObj = GameObject.Find("ProgressSlider");
                if (sliderObj == null) sliderObj = GameObject.Find("LoadingBar");
                
                if (sliderObj != null)
                {
                    progressSlider = sliderObj.GetComponent<Slider>();
                    if (progressSlider != null)
                    {
                        // Configurar slider
                        progressSlider.minValue = 0f;
                        progressSlider.maxValue = 1f;
                        progressSlider.value = 0f;
                        Debug.Log("[Preloader] ✓ Progress Slider encontrado y configurado automáticamente");
                    }
                }
            }
            
            // Si no hay slider, buscar Progress Bar (Image)
            if (progressSlider == null && progressBar == null)
            {
                GameObject progressBarObj = GameObject.Find("ProgressBar");
                if (progressBarObj == null) progressBarObj = GameObject.Find("Progress Bar");
                if (progressBarObj == null) progressBarObj = GameObject.Find("LoadingBar");
                
                if (progressBarObj != null)
                {
                    progressBar = progressBarObj.GetComponent<Image>();
                    if (progressBar != null)
                    {
                        Debug.Log("[Preloader] ✓ Progress Bar (Image) encontrado automáticamente");
                    }
                }
            }
            
            if (progressSlider == null && progressBar == null)
            {
                Debug.LogWarning("[Preloader] ⚠ No se encontró Progress Bar ni Slider. Asígnalo manualmente en el Inspector");
            }
            
            // Buscar Progress Text
            if (progressText == null)
            {
                GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
                foreach (var obj in allObjects)
                {
                    if (obj.name.ToLower().Contains("progress") || obj.name.ToLower().Contains("percent"))
                    {
                        var tmp = obj.GetComponent<TextMeshProUGUI>();
                        if (tmp != null)
                        {
                            progressText = tmp;
                            Debug.Log("[Preloader] ✓ Progress Text encontrado: " + obj.name);
                            break;
                        }
                    }
                }
            }
            
            // Buscar Tip Text
            if (tipText == null)
            {
                GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
                foreach (var obj in allObjects)
                {
                    if (obj.name.ToLower().Contains("tip") || obj.name.ToLower().Contains("hint"))
                    {
                        var tmp = obj.GetComponent<TextMeshProUGUI>();
                        if (tmp != null)
                        {
                            tipText = tmp;
                            Debug.Log("[Preloader] ✓ Tip Text encontrado: " + obj.name);
                            break;
                        }
                    }
                }
            }
        }
        
        private IEnumerator LoadSceneAsync()
        {
            Debug.Log("[Preloader] Coroutine iniciada");
            
            float startTime = Time.time;
            
            // Pequeño delay para asegurar que todo está inicializado
            yield return new WaitForSeconds(0.1f);
            
            Debug.Log($"[Preloader] Cargando escena: {targetSceneName}");
            
            // Iniciar carga asíncrona
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
            
            if (asyncLoad == null)
            {
                Debug.LogError("[Preloader] ERROR: No se pudo iniciar la carga de escena!");
                yield break;
            }
            
            asyncLoad.allowSceneActivation = false;
            
            float progress = 0f;
            
            while (!asyncLoad.isDone)
            {
                // Progreso real de la carga (0.0 a 0.9)
                float realProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                
                Debug.Log($"[Preloader] Real progress: {asyncLoad.progress:F2}, Normalized: {realProgress:F2}");
                
                // Si estamos simulando, hacer que la carga sea más gradual
                if (simulateLoading)
                {
                    progress = Mathf.MoveTowards(progress, realProgress, Time.deltaTime * 0.5f);
                }
                else
                {
                    progress = realProgress;
                }
                
                // Actualizar UI
                UpdateProgress(progress);
                
                // Esperar al tiempo mínimo y que la carga esté completa
                float elapsedTime = Time.time - startTime;
                if (asyncLoad.progress >= 0.9f && elapsedTime >= minimumLoadTime)
                {
                    Debug.Log("[Preloader] Carga completa, activando escena...");
                    
                    // Completar la barra
                    UpdateProgress(1f);
                    yield return new WaitForSeconds(0.5f);
                    
                    // Activar la escena
                    asyncLoad.allowSceneActivation = true;
                }
                
                yield return null;
            }
            
            Debug.Log("[Preloader] Escena cargada completamente!");
        }
        
        private void UpdateProgress(float progress)
        {
            // Actualizar slider si existe
            if (progressSlider != null)
            {
                progressSlider.value = progress;
            }
            // O actualizar barra de progreso (Image)
            else if (progressBar != null)
            {
                progressBar.fillAmount = progress;
            }
            
            // Actualizar texto de porcentaje
            if (progressText != null)
            {
                // Usar formato sin símbolo % para evitar problemas con fuentes que no lo incluyen
                progressText.text = $"Loading: {Mathf.RoundToInt(progress * 100)}";
                
                // FORZAR VISIBILIDAD
                progressText.enabled = true;
                progressText.color = Color.white;
                progressText.fontSize = 48; // Tamaño grande para asegurar visibilidad
                
                Debug.Log($"[Preloader] Texto actualizado: {progressText.text}");
            }
            else
            {
                Debug.LogWarning("[Preloader] ProgressText es NULL!");
            }
            
            // Debug para verificar
            Debug.Log($"[Preloader] Progreso: {Mathf.RoundToInt(progress * 100)}%");
        }
        
        /// <summary>
        /// Llama este método desde un botón si quieres permitir saltar la carga
        /// </summary>
        public void SkipLoading()
        {
            StopAllCoroutines();
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
