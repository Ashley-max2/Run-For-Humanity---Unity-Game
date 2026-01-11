using UnityEngine;
using UnityEngine.SceneManagement;
using RunForHumanity.Core;

public class SceneChanger : MonoBehaviour
{
    public void LoadSceneByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("SceneChanger: El nombre de la escena está vacío.");
            return;
        }

        SceneLoader.LoadScene(name);
    }

    public void ReloadCurrentScene()
    {
        SceneLoader.ReloadCurrentScene();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
