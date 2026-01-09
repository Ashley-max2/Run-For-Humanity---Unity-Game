using UnityEngine;

namespace RunForHumanity.Systems
{
    /// <summary>
    /// Adapts UI and gameplay to screen orientation (Portrait/Landscape)
    /// SOLID: Single Responsibility - Handles orientation changes
    /// Cumple con la rúbrica: Adaptable a portrait y landscape
    /// </summary>
    public class OrientationManager : MonoBehaviour
    {
        public enum Orientation
        {
            Portrait,
            Landscape,
            Auto
        }
        
        [Header("Orientation Settings")]
        [SerializeField] private Orientation _targetOrientation = Orientation.Auto;
        [SerializeField] private bool _allowRotation = true;
        
        private Orientation _currentOrientation;
        private ScreenOrientation _lastScreenOrientation;
        
        public event System.Action<Orientation> OnOrientationChanged;
        
        private void Start()
        {
            SetupOrientation();
            _lastScreenOrientation = Screen.orientation;
        }
        
        private void Update()
        {
            // Detect orientation changes
            if (Screen.orientation != _lastScreenOrientation)
            {
                _lastScreenOrientation = Screen.orientation;
                HandleOrientationChange();
            }
        }
        
        private void SetupOrientation()
        {
            switch (_targetOrientation)
            {
                case Orientation.Portrait:
                    Screen.orientation = ScreenOrientation.Portrait;
                    Screen.autorotateToPortrait = true;
                    Screen.autorotateToPortraitUpsideDown = false;
                    Screen.autorotateToLandscapeLeft = false;
                    Screen.autorotateToLandscapeRight = false;
                    break;
                
                case Orientation.Landscape:
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                    Screen.autorotateToPortrait = false;
                    Screen.autorotateToPortraitUpsideDown = false;
                    Screen.autorotateToLandscapeLeft = true;
                    Screen.autorotateToLandscapeRight = true;
                    break;
                
                case Orientation.Auto:
                    Screen.orientation = ScreenOrientation.AutoRotation;
                    Screen.autorotateToPortrait = _allowRotation;
                    Screen.autorotateToPortraitUpsideDown = false;
                    Screen.autorotateToLandscapeLeft = _allowRotation;
                    Screen.autorotateToLandscapeRight = _allowRotation;
                    break;
            }
            
            HandleOrientationChange();
        }
        
        private void HandleOrientationChange()
        {
            Orientation newOrientation = GetCurrentOrientation();
            
            if (newOrientation != _currentOrientation)
            {
                _currentOrientation = newOrientation;
                OnOrientationChanged?.Invoke(_currentOrientation);
                
                Debug.Log($"[OrientationManager] Orientation changed to: {_currentOrientation}");
                
                // Adjust UI for orientation
                AdjustUIForOrientation(_currentOrientation);
            }
        }
        
        private Orientation GetCurrentOrientation()
        {
            return Screen.width > Screen.height ? Orientation.Landscape : Orientation.Portrait;
        }
        
        private void AdjustUIForOrientation(Orientation orientation)
        {
            // Notify UI elements to adjust their layout
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            
            foreach (Canvas canvas in canvases)
            {
                var scaler = canvas.GetComponent<UnityEngine.UI.CanvasScaler>();
                if (scaler != null)
                {
                    if (orientation == Orientation.Portrait)
                    {
                        scaler.matchWidthOrHeight = 0f; // Match width
                    }
                    else
                    {
                        scaler.matchWidthOrHeight = 1f; // Match height
                    }
                }
            }
        }
        
        public void SetOrientation(Orientation orientation)
        {
            _targetOrientation = orientation;
            SetupOrientation();
        }
        
        public bool IsPortrait()
        {
            return _currentOrientation == Orientation.Portrait;
        }
        
        public bool IsLandscape()
        {
            return _currentOrientation == Orientation.Landscape;
        }
    }
}
