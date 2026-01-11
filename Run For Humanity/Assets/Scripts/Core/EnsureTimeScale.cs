using UnityEngine;

namespace RunForHumanity.Core
{
    /// <summary>
    /// Asegura que Time.timeScale esté en 1 al iniciar cualquier escena
    /// Añade este script a un GameObject en cada escena o usa RuntimeInitializeOnLoadMethod
    /// </summary>
    public class EnsureTimeScale : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetTimeScale()
        {
            Time.timeScale = 1f;
            Debug.Log("[EnsureTimeScale] Time.timeScale reiniciado a 1");
        }

        private void Awake()
        {
            Time.timeScale = 1f;
            Debug.Log($"[EnsureTimeScale] Time.timeScale = {Time.timeScale} en escena {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
    }
}
