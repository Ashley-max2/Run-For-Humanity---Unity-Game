using UnityEngine;
using System.IO;
using System.Text;

public class AndroidDebugLogger : MonoBehaviour
{
    private static AndroidDebugLogger _instance;
    private static string logFilePath;
    private static StringBuilder logBuffer = new StringBuilder();
    private static int logCount = 0;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GameObject go = new GameObject("AndroidDebugLogger");
        _instance = go.AddComponent<AndroidDebugLogger>();
        DontDestroyOnLoad(go);

        logFilePath = Path.Combine(Application.persistentDataPath, "debug_log.txt");

        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }

        Application.logMessageReceived += HandleLog;

        LogToFile("========================================");
        LogToFile($"AndroidDebugLogger INICIADO - {System.DateTime.Now}");
        LogToFile($"Application.persistentDataPath: {Application.persistentDataPath}");
        LogToFile($"Application.dataPath: {Application.dataPath}");
        LogToFile($"Platform: {Application.platform}");
        LogToFile($"Unity Version: {Application.unityVersion}");
        LogToFile("========================================");

        Debug.Log($"[AndroidDebugLogger] Logs guardándose en: {logFilePath}");
    }

    private static void HandleLog(string logString, string stackTrace, LogType type)
    {
        string prefix = "";
        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                prefix = "ERROR";
                break;
            case LogType.Warning:
                prefix = "WARNING";
                break;
            default:
                prefix = "INFO";
                break;
        }

        string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
        string logEntry = $"[{timestamp}] [{prefix}] {logString}";

        if (!string.IsNullOrEmpty(stackTrace) && (type == LogType.Error || type == LogType.Exception))
        {
            logEntry += $"\n{stackTrace}";
        }

        LogToFile(logEntry);
    }

    private static void LogToFile(string message)
    {
        try
        {
            logBuffer.AppendLine(message);
            logCount++;

            if (logCount >= 10)
            {
                FlushLogs();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error escribiendo log: {e.Message}");
        }
    }

    private static void FlushLogs()
    {
        try
        {
            File.AppendAllText(logFilePath, logBuffer.ToString());
            logBuffer.Clear();
            logCount = 0;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error guardando logs: {e.Message}");
        }
    }

    private void OnApplicationQuit()
    {
        FlushLogs();
        LogToFile($"========================================");
        LogToFile($"Aplicación cerrada - {System.DateTime.Now}");
        LogToFile($"========================================");
        FlushLogs();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            FlushLogs();
        }
    }

    public static string GetFullLog()
    {
        FlushLogs();
        
        if (File.Exists(logFilePath))
        {
            return File.ReadAllText(logFilePath);
        }
        
        return "No hay logs disponibles";
    }
}

#if UNITY_EDITOR
public class AndroidDebugMenu
{
    [UnityEditor.MenuItem("Debug/Show Android Debug Log")]
    public static void ShowDebugLog()
    {
        string log = AndroidDebugLogger.GetFullLog();
        Debug.Log("========== DEBUG LOG ==========\n" + log);
        
        UnityEditor.EditorUtility.DisplayDialog(
            "Android Debug Log", 
            $"Log guardado en:\n{Application.persistentDataPath}/debug_log.txt\n\nVer consola para log completo.", 
            "OK");
    }
}
#endif
