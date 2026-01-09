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
            // Buscar referencias automáticamente si no están asignadas
            AutoFindUIElements();
            
            // Mostrar tip aleatorio
            if (tipText != null && loadingTips.Length > 0)
            {
                tipText.text = loadingTips[Random.Range(0, loadingTips.Length)];
            }
            
            Debug.Log("[Preloader] Iniciando carga de escena: " + targetSceneName);
            
            // Iniciar carga
            StartCoroutine(LoadSceneAsync());
        }
        
        private void AutoFindUIElements()
        {
            // Buscar Progress Bar
            if (progressBar == null)
            {
                GameObject progressBarObj = GameObject.Find("ProgressBar");
                if (progressBarObj == null) progressBarObj = GameObject.Find("Progress Bar");
                if (progressBarObj == null) progressBarObj = GameObject.Find("LoadingBar");
                
                if (progressBarObj != null)
                {
                    progressBar = progressBarObj.GetComponent<Image>();
                    if (progressBar != null)
                    {
                        Debug.Log("[Preloader] ✓ Progress Bar encontrado automáticamente");
                    }
                }
                else
                {
                    Debug.LogWarning("[Preloader] ⚠ No se encontró Progress Bar. Búscalo manualmente o renómbralo a 'ProgressBar'");
                }
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
            float startTime = Time.time;
            
            // Iniciar carga asíncrona
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
            asyncLoad.allowSceneActivation = false;
            
            float progress = 0f;
            
            while (!asyncLoad.isDone)
            {
                // Progreso real de la carga (0.0 a 0.9)
                float realProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                
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
                    // Completar la barra
                    UpdateProgress(1f);
                    yield return new WaitForSeconds(0.5f);
                    
                    // Activar la escena
                    asyncLoad.allowSceneActivation = true;
                }
                
                yield return null;
            }
        }
        
        private void UpdateProgress(float progress)
        {
            // Actualizar barra de progreso
            if (progressBar != null)
            {
                progressBar.fillAmount = progress;
            }
            
            // Actualizar texto de porcentaje
            if (progressText != null)
            {
                progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
            }
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
