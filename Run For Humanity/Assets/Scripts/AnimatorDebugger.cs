using UnityEngine;

/// <summary>
/// Script de diagnóstico para detectar problemas con el Animator
/// Adjunta este script al mismo GameObject que tiene el Animator
/// </summary>
public class AnimatorDebugger : MonoBehaviour
{
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError($"❌ [AnimatorDebugger] NO hay componente Animator en '{gameObject.name}'");
            return;
        }
        
        Debug.Log($"✅ [AnimatorDebugger] Animator encontrado en '{gameObject.name}'");
        
        // Verificar Animator Controller
        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError($"❌ [AnimatorDebugger] El Animator NO tiene Animator Controller asignado!");
        }
        else
        {
            Debug.Log($"✅ [AnimatorDebugger] Animator Controller: {animator.runtimeAnimatorController.name}");
        }
        
        // Verificar Avatar (debe ser None para Generic)
        if (animator.avatar != null)
        {
            Debug.LogWarning($"⚠️ [AnimatorDebugger] Avatar asignado: {animator.avatar.name}. Para Generic debe ser None!");
        }
        else
        {
            Debug.Log($"✅ [AnimatorDebugger] Avatar: None (correcto para Generic)");
        }
        
        // Verificar Apply Root Motion
        if (animator.applyRootMotion)
        {
            Debug.LogWarning($"⚠️ [AnimatorDebugger] Apply Root Motion está activado. Desactívalo para evitar movimiento no deseado!");
        }
        else
        {
            Debug.Log($"✅ [AnimatorDebugger] Apply Root Motion: False (correcto)");
        }
        
        // Verificar parámetros
        VerifyParameters();
    }
    
    void VerifyParameters()
    {
        if (animator.runtimeAnimatorController == null) return;
        
        Debug.Log("--- Verificando Parámetros del Animator ---");
        
        string[] requiredParams = { "isGrounded", "isSliding", "Jump", "Die" };
        
        foreach (string paramName in requiredParams)
        {
            bool found = false;
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName)
                {
                    found = true;
                    Debug.Log($"✅ Parámetro '{paramName}' encontrado (tipo: {param.type})");
                    break;
                }
            }
            
            if (!found)
            {
                Debug.LogError($"❌ Parámetro '{paramName}' NO encontrado en el Animator!");
            }
        }
    }
    
    void Update()
    {
        if (animator == null || animator.runtimeAnimatorController == null) return;
        
        // Mostrar estado actual del Animator cada 2 segundos
        if (Time.frameCount % 120 == 0)
        {
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                Debug.Log($"🎬 [Animator] Reproduciendo: {clipInfo[0].clip.name}");
            }
            else
            {
                Debug.LogWarning("⚠️ [Animator] No hay animación reproduciéndose!");
            }
            
            // Mostrar valores de parámetros
            if (HasParameter("isGrounded") && HasParameter("isSliding"))
            {
                Debug.Log($"📊 isGrounded: {animator.GetBool("isGrounded")}, isSliding: {animator.GetBool("isSliding")}");
            }
        }
    }
    
    bool HasParameter(string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }
}
