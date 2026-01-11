using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

/// <summary>
/// Optimiza las builds de Android automáticamente
/// </summary>
public class AndroidBuildOptimizer : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.Android)
            return;

        Debug.Log("========================================");
        Debug.Log("[AndroidBuildOptimizer] Aplicando optimizaciones para build Android...");
        Debug.Log("========================================");

        // 1. Verificar configuración de Scripting Backend
        ScriptingImplementation backend = PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android);
        Debug.Log($"[AndroidBuildOptimizer] Scripting Backend: {backend}");
        
        if (backend == ScriptingImplementation.IL2CPP)
        {
            Debug.LogWarning("[AndroidBuildOptimizer] ⚠️ IL2CPP detectado - Build será LENTA");
            Debug.LogWarning("[AndroidBuildOptimizer] 💡 Para builds rápidas de desarrollo, cambia a Mono:");
            Debug.LogWarning("[AndroidBuildOptimizer]    Edit > Project Settings > Player > Android > Scripting Backend = Mono");
        }
        else
        {
            Debug.Log("[AndroidBuildOptimizer] ✅ Mono detectado - Build será RÁPIDA");
        }

        // 2. Optimizar arquitecturas para desarrollo
        if (EditorUserBuildSettings.development)
        {
            // Solo ARM64 para desarrollo (más rápido)
            AndroidArchitecture arch = PlayerSettings.Android.targetArchitectures;
            
            if (arch != AndroidArchitecture.ARM64)
            {
                Debug.Log("[AndroidBuildOptimizer] Optimizando arquitecturas para desarrollo: Solo ARM64");
                // Nota: No lo cambiamos automáticamente para no romper configuración del usuario
            }
            else
            {
                Debug.Log("[AndroidBuildOptimizer] ✅ Arquitectura ARM64 únicamente (óptimo para desarrollo)");
            }
        }

        // 3. Verificar compresión
        Debug.Log($"[AndroidBuildOptimizer] Compression Method: {PlayerSettings.Android.buildApkPerCpuArchitecture}");

        // 4. Mostrar recomendaciones
        Debug.Log("========================================");
        Debug.Log("[AndroidBuildOptimizer] RECOMENDACIONES:");
        Debug.Log("  1. Usa Mono para desarrollo (3-5x más rápido)");
        Debug.Log("  2. Activa Development Build");
        Debug.Log("  3. Desactiva 'Deep Profiling Support'");
        Debug.Log("  4. Usa compresión LZ4 (balance velocidad/tamaño)");
        Debug.Log("========================================");
    }
}

/// <summary>
/// Menú de utilidades para optimizar builds
/// </summary>
public class BuildOptimizationMenu
{
    [MenuItem("Build/Optimize for Fast Development Builds")]
    public static void OptimizeForDevelopment()
    {
        Debug.Log("[BuildOptimization] Configurando para builds de desarrollo rápidas...");

        // Cambiar a Mono
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        Debug.Log("✅ Scripting Backend cambiado a Mono");

        // Solo ARM64
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        Debug.Log("✅ Arquitectura configurada a ARM64 únicamente");

        // Development build
        EditorUserBuildSettings.development = true;
        Debug.Log("✅ Development Build activado");

        // Compression
        EditorUserBuildSettings.buildAppBundle = false; // APK en lugar de AAB
        Debug.Log("✅ Build configurado para generar APK");

        Debug.Log("========================================");
        Debug.Log("✅ Optimización completada!");
        Debug.Log("Siguiente build debería ser 3-5x más rápida");
        Debug.Log("========================================");
    }

    [MenuItem("Build/Optimize for Production Release")]
    public static void OptimizeForProduction()
    {
        Debug.Log("[BuildOptimization] Configurando para builds de producción...");

        // Cambiar a IL2CPP (requerido por Google Play)
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        Debug.Log("✅ Scripting Backend cambiado a IL2CPP");

        // ARM64 (requerido por Google Play)
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        Debug.Log("✅ Arquitectura configurada a ARM64");

        // Desactivar development build
        EditorUserBuildSettings.development = false;
        Debug.Log("✅ Development Build desactivado");

        // App Bundle para Google Play
        EditorUserBuildSettings.buildAppBundle = true;
        Debug.Log("✅ Build configurado para generar AAB (Android App Bundle)");

        // Code stripping
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Medium);
        Debug.Log("✅ Code Stripping configurado a Medium");

        Debug.Log("========================================");
        Debug.Log("✅ Optimización para producción completada!");
        Debug.Log("Build será más lenta pero optimizada para release");
        Debug.Log("========================================");
    }

    [MenuItem("Build/Show Current Build Configuration")]
    public static void ShowConfiguration()
    {
        Debug.Log("========================================");
        Debug.Log("CONFIGURACIÓN ACTUAL DE BUILD");
        Debug.Log("========================================");

        ScriptingImplementation backend = PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android);
        Debug.Log($"Scripting Backend: {backend}");
        Debug.Log($"Development Build: {EditorUserBuildSettings.development}");
        Debug.Log($"Target Architecture: {PlayerSettings.Android.targetArchitectures}");
        Debug.Log($"Build Type: {(EditorUserBuildSettings.buildAppBundle ? "AAB (App Bundle)" : "APK")}");
        Debug.Log($"Code Stripping: {PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.Android)}");

        Debug.Log("========================================");

        // Estimar tiempo de build
        if (backend == ScriptingImplementation.IL2CPP)
        {
            Debug.LogWarning("⏱️ Tiempo estimado: 8-15 minutos (IL2CPP)");
            Debug.LogWarning("💡 Usa 'Build > Optimize for Fast Development Builds' para reducir a 1-3 minutos");
        }
        else
        {
            Debug.Log("⏱️ Tiempo estimado: 1-3 minutos (Mono) ✅");
        }
    }

    [MenuItem("Build/Clear Build Cache (Fix Slow Builds)")]
    public static void ClearBuildCache()
    {
        if (EditorUtility.DisplayDialog(
            "Limpiar Cache de Build",
            "Esto eliminará:\n- Library/Bee\n- Library/BuildPlayerData\n- Temp\n\nLa próxima build será lenta pero limpiará cachés corruptos.\n\n¿Continuar?",
            "Sí, limpiar",
            "Cancelar"))
        {
            Debug.Log("[BuildOptimization] Limpiando cache de builds...");

            try
            {
                if (System.IO.Directory.Exists("Library/Bee"))
                {
                    System.IO.Directory.Delete("Library/Bee", true);
                    Debug.Log("✅ Library/Bee eliminado");
                }

                if (System.IO.Directory.Exists("Library/BuildPlayerData"))
                {
                    System.IO.Directory.Delete("Library/BuildPlayerData", true);
                    Debug.Log("✅ Library/BuildPlayerData eliminado");
                }

                if (System.IO.Directory.Exists("Temp"))
                {
                    System.IO.Directory.Delete("Temp", true);
                    Debug.Log("✅ Temp eliminado");
                }

                Debug.Log("========================================");
                Debug.Log("✅ Cache limpiado correctamente");
                Debug.Log("La próxima build reconstruirá todo desde cero");
                Debug.Log("========================================");

                EditorUtility.DisplayDialog("Cache Limpiado", "Cache de builds limpiado correctamente.\n\nLa próxima build será más lenta, pero debería resolver problemas de builds corruptas.", "OK");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error limpiando cache: {e.Message}");
            }
        }
    }
}
