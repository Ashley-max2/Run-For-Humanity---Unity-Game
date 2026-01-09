using System.Collections.Generic;
using UnityEngine;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Cámara que sigue al jugador estilo Subway Surfers
    /// Con sistema de transparencia para objetos que bloquean la vista
    /// </summary>
    public class SubwaySurfersCamera : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target; // El jugador
        
        [Header("Camera Position")]
        [SerializeField] private Vector3 offset = new Vector3(0, 3f, -6f); // Offset desde el jugador
        
        [Header("Smoothing")]
        [SerializeField] private float positionSmoothSpeed = 10f; // Suavizado de posición
        [SerializeField] private float rotationSmoothSpeed = 5f; // Suavizado de rotación
        
        [Header("Look At")]
        [SerializeField] private Vector3 lookAtOffset = new Vector3(0, 1f, 2f); // Punto donde mira la cámara relativo al jugador
        
        [Header("Tilt")]
        [SerializeField] private float maxTiltAngle = 5f; // Inclinación máxima al moverse lateralmente
        [SerializeField] private float tiltSpeed = 3f;
        
        [Header("Transparency")]
        [SerializeField] private bool enableTransparency = true; // Activar/desactivar transparencia
        [SerializeField] private float transparencyAlpha = 0.3f; // Nivel de transparencia (0-1)
        [SerializeField] private LayerMask obstacleLayerMask = -1; // Capas que pueden volverse transparentes
        
        private Vector3 currentVelocity;
        private float currentTilt = 0f;
        private Vector3 lastTargetPosition;
        
        // Sistema de transparencia
        private List<Renderer> fadedObjects = new List<Renderer>();
        private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
        private Dictionary<Renderer, Material[]> transparentMaterials = new Dictionary<Renderer, Material[]>();

        void Start()
        {
            // Encontrar al jugador si no está asignado
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                }
            }

            if (target != null)
            {
                lastTargetPosition = target.position;
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            // Calcular posición deseada detrás del jugador
            Vector3 desiredPosition = target.position + offset;
            
            // Aplicar smooth follow
            transform.position = Vector3.Lerp(
                transform.position, 
                desiredPosition, 
                positionSmoothSpeed * Time.deltaTime
            );

            // Punto donde la cámara debe mirar
            Vector3 lookAtPosition = target.position + lookAtOffset;
            
            // Calcular rotación deseada
            Quaternion desiredRotation = Quaternion.LookRotation(lookAtPosition - transform.position);
            
            // Detectar movimiento lateral para inclinar la cámara (Subway Surfers style)
            float lateralMovement = (target.position.x - lastTargetPosition.x) / Time.deltaTime;
            float targetTilt = Mathf.Clamp(lateralMovement * -maxTiltAngle, -maxTiltAngle, maxTiltAngle);
            currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);
            
            // Aplicar inclinación a la rotación
            desiredRotation *= Quaternion.Euler(0, 0, currentTilt);
            
            // Aplicar smooth rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                desiredRotation, 
                rotationSmoothSpeed * Time.deltaTime
            );

            lastTargetPosition = target.position;
            
            // Manejar transparencia de objetos que bloquean la vista
            if (enableTransparency)
            {
                HandleTransparency();
            }
        }

        void HandleTransparency()
        {
            // Lista temporal de objetos que actualmente bloquean la vista
            List<Renderer> currentBlockingObjects = new List<Renderer>();
            
            // Raycast desde la cámara hacia el jugador
            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude;
            
            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, obstacleLayerMask);
            
            foreach (RaycastHit hit in hits)
            {
                // Ignorar el jugador mismo
                if (hit.transform == target || hit.transform.IsChildOf(target))
                    continue;
                
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    currentBlockingObjects.Add(renderer);
                    
                    // Si no está en la lista de objetos desvanecidos, añadirlo
                    if (!fadedObjects.Contains(renderer))
                    {
                        FadeOut(renderer);
                    }
                }
            }
            
            // Restaurar objetos que ya no bloquean
            for (int i = fadedObjects.Count - 1; i >= 0; i--)
            {
                if (!currentBlockingObjects.Contains(fadedObjects[i]))
                {
                    FadeIn(fadedObjects[i]);
                    fadedObjects.RemoveAt(i);
                }
            }
        }

        void FadeOut(Renderer renderer)
        {
            if (renderer == null) return;
            
            fadedObjects.Add(renderer);
            
            // Guardar materiales originales
            if (!originalMaterials.ContainsKey(renderer))
            {
                originalMaterials[renderer] = renderer.materials;
            }
            
            // Crear materiales transparentes
            Material[] transparentMats = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material mat = new Material(renderer.materials[i]);
                
                // Cambiar a modo transparente
                if (mat.HasProperty("_Mode"))
                {
                    mat.SetFloat("_Mode", 3); // Transparent mode
                }
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
                
                // Aplicar transparencia
                Color color = mat.color;
                color.a = transparencyAlpha;
                mat.color = color;
                
                transparentMats[i] = mat;
            }
            
            transparentMaterials[renderer] = transparentMats;
            renderer.materials = transparentMats;
        }

        void FadeIn(Renderer renderer)
        {
            if (renderer == null) return;
            
            // Restaurar materiales originales
            if (originalMaterials.ContainsKey(renderer))
            {
                renderer.materials = originalMaterials[renderer];
                originalMaterials.Remove(renderer);
            }
            
            // Limpiar materiales transparentes
            if (transparentMaterials.ContainsKey(renderer))
            {
                foreach (Material mat in transparentMaterials[renderer])
                {
                    if (mat != null)
                        Destroy(mat);
                }
                transparentMaterials.Remove(renderer);
            }
        }

        void OnDestroy()
        {
            // Limpiar todos los materiales creados
            foreach (var kvp in transparentMaterials)
            {
                if (kvp.Value != null)
                {
                    foreach (Material mat in kvp.Value)
                    {
                        if (mat != null)
                            Destroy(mat);
                    }
                }
            }
        }

        // Método para cambiar objetivo (útil si hay múltiples jugadores)
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            if (target != null)
            {
                lastTargetPosition = target.position;
            }
        }

        // Gizmos para visualizar en el editor
        void OnDrawGizmosSelected()
        {
            if (target == null) return;

            // Mostrar la posición de la cámara
            Gizmos.color = Color.blue;
            Vector3 camPos = target.position + offset;
            Gizmos.DrawWireSphere(camPos, 0.5f);
            
            // Mostrar el punto de mirada
            Gizmos.color = Color.red;
            Vector3 lookPos = target.position + lookAtOffset;
            Gizmos.DrawWireSphere(lookPos, 0.3f);
            
            // Línea entre cámara y punto de mirada
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(camPos, lookPos);
        }
    }
}
