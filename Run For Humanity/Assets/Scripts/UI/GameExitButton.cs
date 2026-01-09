using UnityEngine;

/// <summary>
/// Script simple para salir del juego.
/// Agrégalo a un GameObject con un botón UI y asigna el método QuitGame() desde el Inspector.
/// </summary>
public class GameExitButton : MonoBehaviour
{
    [Header("Opciones")]
    [Tooltip("Mostrar diálogo de confirmación antes de salir")]
    public bool showConfirmation = false;

    [Tooltip("Mensaje de confirmación (solo si showConfirmation está activado)")]
    public string confirmationMessage = "¿Estás seguro que deseas salir?";

    /// <summary>
    /// Cierra la aplicación.
    /// Llama este método desde un evento de botón UI (OnClick).
    /// </summary>
    public void QuitGame()
    {
        if (showConfirmation)
        {
            // En un juego real, aquí mostrarías un popup
            // Por ahora solo muestra en consola
            Debug.Log(confirmationMessage);
        }

        Debug.Log("Cerrando el juego...");

        #if UNITY_EDITOR
        // Si estamos en el editor, detiene el play mode
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // En el build, cierra la aplicación
        Application.Quit();
        #endif
    }

    /// <summary>
    /// Cierra la aplicación inmediatamente sin confirmación.
    /// </summary>
    public void QuitGameImmediate()
    {
        Debug.Log("Cerrando el juego inmediatamente...");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
