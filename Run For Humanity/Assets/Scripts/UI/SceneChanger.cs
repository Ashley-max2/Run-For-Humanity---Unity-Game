using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script simple para cambiar de escena. 
/// Agrégalo a un GameObject con un botón UI y asigna el método público desde el Inspector.
/// </summary>
public class SceneChanger : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("Nombre de la escena a cargar (debe estar agregada en Build Settings)")]
    public string sceneName;

    [Tooltip("Índice de la escena a cargar (alternativa al nombre)")]
    public int sceneIndex = -1;

    [Header("Opciones")]
    [Tooltip("Cargar la escena de forma asíncrona (recomendado para escenas grandes)")]
    public bool loadAsync = false;

    /// <summary>
    /// Carga la escena configurada por nombre o índice.
    /// Llama este método desde un evento de botón UI (OnClick).
    /// </summary>
    public void LoadConfiguredScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            LoadSceneByName(sceneName);
        }
        else if (sceneIndex >= 0)
        {
            LoadSceneByIndex(sceneIndex);
        }
        else
        {
            Debug.LogError("SceneChanger: No se configuró ningún nombre o índice de escena.");
        }
    }

    /// <summary>
    /// Carga una escena específica por nombre.
    /// También puedes llamar este método directamente desde el Inspector pasando el nombre.
    /// </summary>
    public void LoadSceneByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("SceneChanger: El nombre de la escena está vacío.");
            return;
        }

        Debug.Log($"Cargando escena: {name}");

        if (loadAsync)
        {
            SceneManager.LoadSceneAsync(name);
        }
        else
        {
            SceneManager.LoadScene(name);
        }
    }

    /// <summary>
    /// Carga una escena específica por índice.
    /// También puedes llamar este método directamente desde el Inspector pasando el índice.
    /// </summary>
    public void LoadSceneByIndex(int index)
    {
        if (index < 0 || index >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"SceneChanger: Índice de escena inválido: {index}");
            return;
        }

        Debug.Log($"Cargando escena con índice: {index}");

        if (loadAsync)
        {
            SceneManager.LoadSceneAsync(index);
        }
        else
        {
            SceneManager.LoadScene(index);
        }
    }

    /// <summary>
    /// Recarga la escena actual.
    /// </summary>
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log($"Recargando escena: {currentScene.name}");
        
        if (loadAsync)
        {
            SceneManager.LoadSceneAsync(currentScene.buildIndex);
        }
        else
        {
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }

    /// <summary>
    /// Carga la siguiente escena en el Build Settings.
    /// </summary>
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = (currentIndex + 1) % SceneManager.sceneCountInBuildSettings;
        LoadSceneByIndex(nextIndex);
    }

    /// <summary>
    /// Carga la escena anterior en el Build Settings.
    /// </summary>
    public void LoadPreviousScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int previousIndex = currentIndex - 1;
        if (previousIndex < 0)
        {
            previousIndex = SceneManager.sceneCountInBuildSettings - 1;
        }
        LoadSceneByIndex(previousIndex);
    }

    /// <summary>
    /// Cierra la aplicación.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Cerrando aplicación...");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
