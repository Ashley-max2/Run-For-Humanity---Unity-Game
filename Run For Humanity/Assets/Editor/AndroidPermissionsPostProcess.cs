using UnityEditor;
using UnityEditor.Android;
using System.IO;

/// <summary>
/// Añade permisos necesarios al AndroidManifest en tiempo de build
/// </summary>
public class AndroidPermissionsPostProcess : IPostGenerateGradleAndroidProject
{
    public int callbackOrder => 1;
    
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        string manifestPath = Path.Combine(path, "src/main/AndroidManifest.xml");
        
        if (!File.Exists(manifestPath))
        {
            UnityEngine.Debug.LogWarning($"[AndroidPermissions] No se encontró AndroidManifest en: {manifestPath}");
            return;
        }
        
        string manifestContent = File.ReadAllText(manifestPath);
        
        // Añadir permisos si no existen
        if (!manifestContent.Contains("android.permission.CAMERA"))
        {
            manifestContent = manifestContent.Replace(
                "<uses-permission",
                "<!-- Permisos para sensores -->\n    <uses-permission android:name=\"android.permission.CAMERA\" />\n    <uses-permission android:name=\"android.permission.VIBRATE\" />\n    <uses-feature android:name=\"android.hardware.camera\" android:required=\"false\" />\n    <uses-feature android:name=\"android.hardware.camera.flash\" android:required=\"false\" />\n    \n    <uses-permission"
            );
            
            File.WriteAllText(manifestPath, manifestContent);
            UnityEngine.Debug.Log("[AndroidPermissions] Permisos de cámara y vibración añadidos al AndroidManifest");
        }
    }
}
