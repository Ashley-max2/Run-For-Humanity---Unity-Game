using UnityEngine;

namespace RunForHumanity.Core
{
    // Configura el juego para PC y Móvil (Portrait/Landscape)
    public class PlatformAdapter : MonoBehaviour
    {
        [Header("Orientaciones Permitidas")]
        [SerializeField] private bool allowPortrait = true;
        [SerializeField] private bool allowLandscape = true;
        [SerializeField] private bool autoRotate = true;
        
        [Header("Configuración por Plataforma")]
        [SerializeField] private int targetFrameRate = 60;
        #pragma warning disable 0414
        [SerializeField] private bool adaptInputForPlatform = true;
        #pragma warning restore 0414
        
        [Header("UI Scaling")]
        [SerializeField] private bool autoConfigureCanvasScaler = true;
        
        private ScreenOrientation lastOrientation;
        private bool isMobile;

        void Awake()
        {
            DetectPlatform();
            
            ConfigureOrientation();
            
            Application.targetFrameRate = targetFrameRate;
            
            ConfigureQuality();
            
            if (autoConfigureCanvasScaler)
            {
                ConfigureAllCanvases();
            }
            
            Debug.Log($"[PlatformAdapter] Plataforma: {Application.platform}, Móvil: {isMobile}, Resolución: {Screen.width}x{Screen.height}");
        }

        void Start()
        {
            lastOrientation = Screen.orientation;
        }

        void Update()
        {
            if (Screen.orientation != lastOrientation)
            {
                CancelInvoke("ApplyOrientationChange");
                Invoke("ApplyOrientationChange", 0.1f);
            }
        }

        void ApplyOrientationChange()
        {
            lastOrientation = Screen.orientation;
            OnOrientationChanged();
        }

        void DetectPlatform()
        {
            isMobile = Application.isMobilePlatform;
            
            #if UNITY_EDITOR
            if (Screen.height > Screen.width)
            {
                Debug.Log("[PlatformAdapter] Editor en modo Portrait - simulando móvil");
            }
            #endif
        }

        void ConfigureOrientation()
        {
            if (autoRotate && (allowPortrait && allowLandscape))
            {
                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.autorotateToPortrait = allowPortrait;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = allowLandscape;
                Screen.autorotateToLandscapeRight = allowLandscape;
                
                Debug.Log("[PlatformAdapter] Auto-rotación activada (Portrait y Landscape)");
            }
            else if (allowPortrait && !allowLandscape)
            {
                Screen.orientation = ScreenOrientation.Portrait;
                Debug.Log("[PlatformAdapter] Forzado a Portrait");
            }
            else if (allowLandscape && !allowPortrait)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Debug.Log("[PlatformAdapter] Forzado a Landscape");
            }
        }

        void ConfigureQuality()
        {
            if (isMobile)
            {
                QualitySettings.SetQualityLevel(1, true);
                Debug.Log("[PlatformAdapter] Calidad configurada para móvil");
            }
            else
            {
                QualitySettings.SetQualityLevel(2, true);
                Debug.Log("[PlatformAdapter] Calidad configurada para PC");
            }
        }

        void ConfigureAllCanvases()
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>(true);
            
            bool isLandscape = Screen.width > Screen.height;
            
            foreach (Canvas canvas in canvases)
            {
                UnityEngine.UI.CanvasScaler scaler = canvas.GetComponent<UnityEngine.UI.CanvasScaler>();
                
                if (scaler != null)
                {
                    scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    
                    if (isLandscape)
                    {
                        scaler.referenceResolution = new Vector2(1920, 1080);
                        scaler.matchWidthOrHeight = 1f;
                    }
                    else
                    {
                        scaler.referenceResolution = new Vector2(1080, 1920);
                        scaler.matchWidthOrHeight = 0f;
                    }
                    
                    scaler.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    
                    UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(canvas.GetComponent<RectTransform>());
                    
                    Debug.Log($"[PlatformAdapter] Canvas '{canvas.gameObject.name}' configurado para {(isLandscape ? "Landscape" : "Portrait")}");
                }
            }
        }

        void OnOrientationChanged()
        {
            Debug.Log($"[PlatformAdapter] Orientación cambiada a: {Screen.orientation} ({Screen.width}x{Screen.height})");
            
            if (autoConfigureCanvasScaler)
            {
                ConfigureAllCanvases();
            }
        }

        public void SetOrientation(bool portrait)
        {
            if (portrait)
            {
                Screen.orientation = ScreenOrientation.Portrait;
                allowPortrait = true;
                allowLandscape = false;
            }
            else
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                allowPortrait = false;
                allowLandscape = true;
            }
            
            ConfigureOrientation();
        }

        public void AllowAllOrientations()
        {
            allowPortrait = true;
            allowLandscape = true;
            autoRotate = true;
            ConfigureOrientation();
        }

        public bool IsMobile() => isMobile;
        public bool IsPortrait() => Screen.height > Screen.width;
        public bool IsLandscape() => Screen.width > Screen.height;
    }
}
