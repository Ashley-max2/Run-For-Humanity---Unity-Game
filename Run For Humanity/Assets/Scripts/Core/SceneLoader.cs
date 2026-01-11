using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace RunForHumanity.Core
{
    public class SceneLoader : MonoBehaviour
    {
        private static bool isLoading = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            isLoading = false;
            Debug.Log($"[SceneLoader] Escena {scene.name} cargada, flag reseteado");
            
            if (GameSettingsManager.Instance != null)
            {
                GameSettingsManager.Instance.OnSceneLoaded();
            }
        }

        public static void LoadScene(string sceneName)
        {
            if (isLoading)
            {
                Debug.LogWarning($"[SceneLoader] Ya hay una escena cargándose, ignorando petición para: {sceneName}");
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneLoader] Nombre de escena vacío");
                return;
            }

            Debug.Log($"[SceneLoader] Cargando escena: {sceneName}");
            
            Time.timeScale = 1f;
            
            isLoading = true;
            
            try
            {
                SceneManager.LoadScene(sceneName);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SceneLoader] Error cargando escena {sceneName}: {e.Message}");
                isLoading = false;
            }
        }

        public static void LoadSceneAsync(string sceneName, MonoBehaviour caller)
        {
            if (isLoading)
            {
                Debug.LogWarning($"[SceneLoader] Ya hay una escena cargándose, ignorando petición para: {sceneName}");
                return;
            }

            caller.StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
        }

        private static IEnumerator LoadSceneAsyncCoroutine(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneLoader] Nombre de escena vacío");
                yield break;
            }

            Debug.Log($"[SceneLoader] Cargando escena async: {sceneName}");
            
            Time.timeScale = 1f;
            
            isLoading = true;

            yield return new WaitForSeconds(0.1f);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            
            if (asyncLoad == null)
            {
                Debug.LogError($"[SceneLoader] No se pudo iniciar la carga de {sceneName}");
                isLoading = false;
                yield break;
            }

            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                Debug.Log($"[SceneLoader] Progreso: {asyncLoad.progress * 100}%");
                yield return null;
            }

            asyncLoad.allowSceneActivation = true;

            yield return new WaitUntil(() => asyncLoad.isDone);

            Debug.Log($"[SceneLoader] Escena {sceneName} cargada completamente");
            isLoading = false;
        }

        /// <summary>
        /// Recarga la escena actual de forma segura
        /// </summary>
        public static void ReloadCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            LoadScene(currentScene.name);
        }
    }
}
