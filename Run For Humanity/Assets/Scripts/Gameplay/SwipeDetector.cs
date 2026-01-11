using UnityEngine;
using UnityEngine.InputSystem;

namespace RunForHumanity.Gameplay
{
    public class SwipeDetector : MonoBehaviour
    {
        [Header("Swipe Settings")]
        [SerializeField] private float minSwipeDistance = 50f;
        [SerializeField] private float maxSwipeTime = 0.5f;
        [SerializeField] private float directionThreshold = 0.5f;
        
        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = true;
        
        public event System.Action OnSwipeUp;
        public event System.Action OnSwipeDown;
        public event System.Action OnSwipeLeft;
        public event System.Action OnSwipeRight;
        
        private Vector2 touchStartPosition;
        private float touchStartTime;
        private bool isTouching = false;
        
        private Touchscreen touchscreen;
        
        void Awake()
        {
            touchscreen = Touchscreen.current;
            
            if (touchscreen == null)
            {
                Debug.LogWarning("[SwipeDetector] No se detectó touchscreen. Los swipes no funcionarán.");
            }
        }
        
        void Update()
        {
            if (touchscreen == null) return;
            
            if (touchscreen.primaryTouch.press.wasPressedThisFrame)
            {
                OnTouchStart(touchscreen.primaryTouch.position.ReadValue());
            }
            else if (touchscreen.primaryTouch.press.wasReleasedThisFrame && isTouching)
            {
                OnTouchEnd(touchscreen.primaryTouch.position.ReadValue());
            }
        }
        
        private void OnTouchStart(Vector2 position)
        {
            touchStartPosition = position;
            touchStartTime = Time.time;
            isTouching = true;
            
            if (showDebugLogs)
            {
                Debug.Log($"[SwipeDetector] Touch started at {position}");
            }
        }
        
        private void OnTouchEnd(Vector2 position)
        {
            isTouching = false;
            float touchDuration = Time.time - touchStartTime;
            Vector2 swipeVector = position - touchStartPosition;
            float swipeDistance = swipeVector.magnitude;
            
            if (showDebugLogs)
            {
                Debug.Log($"[SwipeDetector] Touch ended. Duration: {touchDuration:F2}s, Distance: {swipeDistance:F0}px");
            }
            
            if (swipeDistance < minSwipeDistance)
            {
                if (showDebugLogs) Debug.Log($"[SwipeDetector] Movement too short: {swipeDistance:F0}px < {minSwipeDistance}px");
                return;
            }
            
            if (touchDuration > maxSwipeTime)
            {
                if (showDebugLogs) Debug.Log($"[SwipeDetector] Movement too slow: {touchDuration:F2}s > {maxSwipeTime}s");
                return;
            }
            
            Vector2 swipeDirection = swipeVector.normalized;
            
            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                if (swipeDirection.x > directionThreshold)
                {
                    if (showDebugLogs) Debug.Log("[SwipeDetector] SWIPE RIGHT");
                    OnSwipeRight?.Invoke();
                }
                else if (swipeDirection.x < -directionThreshold)
                {
                    if (showDebugLogs) Debug.Log("[SwipeDetector] SWIPE LEFT");
                    OnSwipeLeft?.Invoke();
                }
            }
            else
            {
                if (swipeDirection.y > directionThreshold)
                {
                    if (showDebugLogs) Debug.Log("[SwipeDetector] SWIPE UP");
                    OnSwipeUp?.Invoke();
                }
                else if (swipeDirection.y < -directionThreshold)
                {
                    if (showDebugLogs) Debug.Log("[SwipeDetector] SWIPE DOWN");
                    OnSwipeDown?.Invoke();
                }
            }
        }
        
        private void OnDestroy()
        {
            OnSwipeUp = null;
            OnSwipeDown = null;
            OnSwipeLeft = null;
            OnSwipeRight = null;
        }
    }
}
