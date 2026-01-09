using UnityEngine;

namespace RunForHumanity.UI
{
    /// <summary>
    /// Adjusts UI RectTransform to device safe area (notch support)
    /// SOLID: Single Responsibility - Safe area handling
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaAdjuster : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Rect _lastSafeArea;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            ApplySafeArea();
        }
        
        private void Update()
        {
            // Check if safe area changed (orientation change, etc.)
            if (_lastSafeArea != Screen.safeArea)
            {
                ApplySafeArea();
            }
        }
        
        private void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;
            _lastSafeArea = safeArea;
            
            // Convert safe area to anchors
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            
            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
            
            Debug.Log($"[SafeAreaAdjuster] Applied safe area: {safeArea}");
        }
    }
}
