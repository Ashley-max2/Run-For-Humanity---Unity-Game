using UnityEngine;
using UnityEngine.Advertisements;
using System;
using RunForHumanity.Core;

namespace RunForHumanity.Systems
{
    /// <summary>
    /// Manages Unity Ads integration
    /// SOLID: Single Responsibility - Advertisement management
    /// </summary>
    public class AdManager : MonoBehaviour, IInitializable, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [Header("Ad Configuration")]
        [SerializeField] private string _androidGameId = "YOUR_ANDROID_GAME_ID";
        [SerializeField] private string _iosGameId = "YOUR_IOS_GAME_ID";
        [SerializeField] private bool _testMode = true;
        
        [Header("Ad Placements")]
        [SerializeField] private string _bannerPlacement = "Banner_Android";
        [SerializeField] private string _rewardedPlacement = "Rewarded_Android";
        [SerializeField] private string _interstitialPlacement = "Interstitial_Android";
        
        private string _gameId;
        private Action _onRewardedAdComplete;
        
        public bool IsInitialized { get; private set; }
        public static AdManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void Initialize()
        {
#if UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_IOS
            _gameId = _iosGameId;
#else
            _gameId = _androidGameId;
#endif
            
            if (!Advertisement.isInitialized)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
            
            ServiceLocator.RegisterService(this);
        }
        
        public void OnInitializationComplete()
        {
            IsInitialized = true;
            Debug.Log("[AdManager] Unity Ads initialized successfully");
            LoadBanner();
        }
        
        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"[AdManager] Initialization failed: {error} - {message}");
        }
        
        // Banner Ads (non-intrusive, bottom of screen in menus only)
        public void LoadBanner()
        {
            if (!IsInitialized) return;
            
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        }
        
        public void ShowBanner()
        {
            if (!IsInitialized) return;
            
            Advertisement.Banner.Load(_bannerPlacement, new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            });
        }
        
        public void HideBanner()
        {
            Advertisement.Banner.Hide();
        }
        
        private void OnBannerLoaded()
        {
            Advertisement.Banner.Show(_bannerPlacement);
            Debug.Log("[AdManager] Banner loaded and shown");
        }
        
        private void OnBannerError(string message)
        {
            Debug.LogError($"[AdManager] Banner error: {message}");
        }
        
        // Rewarded Video Ads (OPTIONAL - never forced)
        public void ShowRewardedAd(Action onComplete)
        {
            if (!IsInitialized)
            {
                Debug.LogWarning("[AdManager] Ads not initialized yet");
                return;
            }
            
            _onRewardedAdComplete = onComplete;
            Advertisement.Load(_rewardedPlacement, this);
        }
        
        // Interstitial Ads (max 1 every 2 games)
        public void ShowInterstitial()
        {
            if (!IsInitialized) return;
            
            Advertisement.Load(_interstitialPlacement, this);
        }
        
        // IUnityAdsLoadListener
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"[AdManager] Ad loaded: {placementId}");
            
            if (placementId == _rewardedPlacement || placementId == _interstitialPlacement)
            {
                Advertisement.Show(placementId, this);
            }
        }
        
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"[AdManager] Failed to load ad: {placementId} - {error} - {message}");
        }
        
        // IUnityAdsShowListener
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogError($"[AdManager] Failed to show ad: {placementId} - {error} - {message}");
        }
        
        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"[AdManager] Ad started: {placementId}");
            Time.timeScale = 0f; // Pause game
        }
        
        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"[AdManager] Ad clicked: {placementId}");
        }
        
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Time.timeScale = 1f; // Resume game
            
            if (placementId == _rewardedPlacement && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log("[AdManager] Rewarded ad completed - giving reward");
                _onRewardedAdComplete?.Invoke();
                _onRewardedAdComplete = null;
                
                // Track revenue for donation
                DonationSystem.Instance?.ProcessGameRevenue(0.5f); // Example: $0.50 per rewarded ad
            }
        }
    }
}
