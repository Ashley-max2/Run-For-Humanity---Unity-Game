using UnityEngine;
using UnityEngine.UI;

namespace RunForHumanity.UI
{
    // Adapta elementos de UI al cambio de orientación
    [RequireComponent(typeof(RectTransform))]
    public class AdaptiveUIElement : MonoBehaviour
    {
        [Header("Adaptive Settings")]
        [SerializeField] private bool preventOffscreen = true; // Evita que salga de pantalla
        [SerializeField] private bool scaleWithOrientation = false; // Escala según orientación
        
        [Header("Scale Settings")]
        [SerializeField] private float landscapeScale = 1f;
        [SerializeField] private float portraitScale = 0.8f;
        
        [Header("Position Constraints")]
        [SerializeField] private float minDistanceFromEdge = 50f; // Distancia mínima del borde en píxeles
        
        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = false;
        
        private RectTransform rectTransform;
        private Vector2 lastScreenSize;
        private Vector2 initialAnchoredPosition;
        private Vector2 initialScale;
        
        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            lastScreenSize = new Vector2(Screen.width, Screen.height);
            initialAnchoredPosition = rectTransform.anchoredPosition;
            initialScale = rectTransform.localScale;
        }
        
        void Start()
        {
            ApplyAdaptiveSettings();
        }
        
        void Update()
        {
            // Detectar cambios en tamaño de pantalla (orientación)
            Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);
            if (currentScreenSize != lastScreenSize)
            {
                OnScreenSizeChanged(lastScreenSize, currentScreenSize);
                lastScreenSize = currentScreenSize;
            }
        }
        
        private void OnScreenSizeChanged(Vector2 oldSize, Vector2 newSize)
        {
            if (showDebugLogs)
            {
                Debug.Log($"[AdaptiveUI] {gameObject.name}: Pantalla cambió de {oldSize} a {newSize}");
            }
            
            ApplyAdaptiveSettings();
        }
        
        private void ApplyAdaptiveSettings()
        {
            if (scaleWithOrientation)
            {
                ApplyOrientationScale();
            }
            
            if (preventOffscreen)
            {
                ConstrainToScreen();
            }
            
            // Forzar actualización visual
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
        
        private void ApplyOrientationScale()
        {
            bool isLandscape = Screen.width > Screen.height;
            float targetScale = isLandscape ? landscapeScale : portraitScale;
            
            rectTransform.localScale = initialScale * targetScale;
            
            if (showDebugLogs)
            {
                Debug.Log($"[AdaptiveUI] {gameObject.name}: Scale ajustada a {targetScale} ({(isLandscape ? "Landscape" : "Portrait")})");
            }
        }
        
        private void ConstrainToScreen()
        {
            // Obtener posición en coordenadas de pantalla
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas == null) return;
            
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            
            // Verificar cada esquina
            bool needsAdjustment = false;
            Vector2 adjustment = Vector2.zero;
            
            foreach (Vector3 corner in corners)
            {
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, corner);
                
                // Verificar bordes
                if (screenPoint.x < minDistanceFromEdge)
                {
                    adjustment.x = Mathf.Max(adjustment.x, minDistanceFromEdge - screenPoint.x);
                    needsAdjustment = true;
                }
                else if (screenPoint.x > Screen.width - minDistanceFromEdge)
                {
                    adjustment.x = Mathf.Min(adjustment.x, (Screen.width - minDistanceFromEdge) - screenPoint.x);
                    needsAdjustment = true;
                }
                
                if (screenPoint.y < minDistanceFromEdge)
                {
                    adjustment.y = Mathf.Max(adjustment.y, minDistanceFromEdge - screenPoint.y);
                    needsAdjustment = true;
                }
                else if (screenPoint.y > Screen.height - minDistanceFromEdge)
                {
                    adjustment.y = Mathf.Min(adjustment.y, (Screen.height - minDistanceFromEdge) - screenPoint.y);
                    needsAdjustment = true;
                }
            }
            
            if (needsAdjustment)
            {
                rectTransform.anchoredPosition += adjustment / canvasRect.localScale.x;
                
                if (showDebugLogs)
                {
                    Debug.Log($"[AdaptiveUI] {gameObject.name}: Posición ajustada {adjustment} para mantenerse en pantalla");
                }
            }
        }
        
        // Método para resetear a posición inicial
        public void ResetToInitialPosition()
        {
            rectTransform.anchoredPosition = initialAnchoredPosition;
            rectTransform.localScale = initialScale;
        }
    }
}
