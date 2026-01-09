using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace RunForHumanity.Setup
{
    /// <summary>
    /// Auto-configures all game systems and managers
    /// Run this to set up the entire game automatically
    /// </summary>
    public class GameAutoConfigurator : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private bool setupOnAwake = true;
        [SerializeField] private bool setupAllScenes = true;
        
        private void Awake()
        {
            if (setupOnAwake)
            {
                SetupGame();
            }
        }
        
        [ContextMenu("Setup Complete Game")]
        public void SetupGame()
        {
            Debug.Log("=== Starting Game Auto-Configuration ===");
            
            // Setup persistent managers
            SetupPersistentManagers();
            
            // Setup current scene
            SetupCurrentScene();
            
            Debug.Log("=== Game Auto-Configuration Complete ===");
        }
        
        private void SetupPersistentManagers()
        {
            Debug.Log("Setting up persistent managers...");
            
            // Event Manager (Root)
            SetupEventManager();
            
            // Audio Manager (Root)
            SetupAudioManager();
            
            // Ad Manager (Root)
            SetupAdManager();
            
            // Donation System (Root)
            SetupDonationSystem();
        }
        
        private void SetupEventManager()
        {
            var existing = GameObject.Find("EventManager");
            if (existing != null)
            {
                Debug.Log("EventManager already exists");
                return;
            }
            
            GameObject eventManagerObj = new GameObject("EventManager");
            eventManagerObj.transform.SetParent(null); // Root level
            
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(eventManagerObj);
            }
            
            Debug.Log("✓ EventManager created (add EventManager component manually)");
        }
        
        private void SetupAudioManager()
        {
            // Check if AudioManager script exists
            var existing = GameObject.Find("AudioManager");
            if (existing != null)
            {
                // Ensure it's at root
                if (existing.transform.parent != null)
                {
                    existing.transform.SetParent(null);
                    Debug.Log("✓ AudioManager moved to root");
                }
                Debug.Log("AudioManager already exists");
                return;
            }
            
            GameObject audioManagerObj = new GameObject("AudioManager");
            audioManagerObj.transform.SetParent(null); // Root level
            
            // Add Audio Sources
            AudioSource musicSource = audioManagerObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = 0.7f;
            
            AudioSource sfxSource = audioManagerObj.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.volume = 0.7f;
            
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(audioManagerObj);
            }
            
            Debug.Log("✓ AudioManager created and configured");
        }
        
        private void SetupAdManager()
        {
            var existing = GameObject.Find("AdManager");
            if (existing != null)
            {
                // Ensure it's at root
                if (existing.transform.parent != null)
                {
                    existing.transform.SetParent(null);
                    Debug.Log("✓ AdManager moved to root");
                }
                Debug.Log("AdManager already exists");
                return;
            }
            
            GameObject adManagerObj = new GameObject("AdManager");
            adManagerObj.transform.SetParent(null); // Root level
            
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(adManagerObj);
            }
            
            Debug.Log("✓ AdManager created (add AdManager component manually)");
        }
        
        private void SetupDonationSystem()
        {
            var existing = GameObject.Find("DonationSystem");
            if (existing != null)
            {
                // Ensure it's at root
                if (existing.transform.parent != null)
                {
                    existing.transform.SetParent(null);
                    Debug.Log("✓ DonationSystem moved to root");
                }
                Debug.Log("DonationSystem already exists");
                return;
            }
            
            GameObject donationSystemObj = new GameObject("DonationSystem");
            donationSystemObj.transform.SetParent(null); // Root level
            
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(donationSystemObj);
            }
            
            Debug.Log("✓ DonationSystem created (add DonationSystem component manually)");
        }
        
        private void SetupCurrentScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            Debug.Log($"Setting up scene: {sceneName}");
            
            switch (sceneName)
            {
                case "Preloader":
                    SetupPreloaderScene();
                    break;
                    
                case "MainMenu":
                    SetupMainMenuScene();
                    break;
                    
                case "Gameplay":
                    SetupGameplayScene();
                    break;
                    
                case "Shop":
                    SetupShopScene();
                    break;
                    
                case "ONGSelection":
                    SetupONGSelectionScene();
                    break;
                    
                default:
                    Debug.LogWarning($"No setup configuration for scene: {sceneName}");
                    break;
            }
        }
        
        private void SetupPreloaderScene()
        {
            Debug.Log("Setting up Preloader scene...");
            
            // No specific managers needed, just UI
            Debug.Log("✓ Preloader scene setup complete");
        }
        
        private void SetupMainMenuScene()
        {
            Debug.Log("Setting up MainMenu scene...");
            
            // Setup UIManager GameObject
            var uiManagerObj = GameObject.Find("UIManager");
            if (uiManagerObj == null)
            {
                uiManagerObj = new GameObject("UIManager");
            }
            
            // Setup GameManager GameObject
            var gameManagerObj = GameObject.Find("GameManager");
            if (gameManagerObj == null)
            {
                gameManagerObj = new GameObject("GameManager");
            }
            
            Debug.Log("✓ MainMenu scene setup complete");
        }
        
        private void SetupGameplayScene()
        {
            Debug.Log("Setting up Gameplay scene...");
            
            // Setup UIManager GameObject
            var uiManagerObj = GameObject.Find("UIManager");
            if (uiManagerObj == null)
            {
                uiManagerObj = new GameObject("UIManager");
            }
            
            // Setup GameManager GameObject
            var gameManagerObj = GameObject.Find("GameManager");
            if (gameManagerObj == null)
            {
                gameManagerObj = new GameObject("GameManager");
            }
            
            // Setup Player GameObject
            var playerObj = GameObject.Find("Player");
            if (playerObj == null)
            {
                playerObj = new GameObject("Player");
                
                // Agregar CharacterController (mejor que Rigidbody para endless runner)
                var controller = playerObj.AddComponent<CharacterController>();
                controller.center = new Vector3(0, 0, 0);
                controller.radius = 0.5f;
                controller.height = 2f;
                controller.skinWidth = 0.08f;
                controller.minMoveDistance = 0.001f;
                controller.slopeLimit = 45f;
                controller.stepOffset = 0.3f;
                
                // Agregar PlayerSetup para configuración automática
                var playerSetup = playerObj.AddComponent<PlayerSetup>();
                
                // Posicionar correctamente (altura 2m, carril del medio)
                playerObj.transform.position = new Vector3(0, 2, 0);
                playerObj.tag = "Player";
                
                Debug.Log("✓ Player created with CharacterController at height 2m");
            }
            
            // Setup Ground
            var groundObj = GameObject.Find("Ground");
            if (groundObj == null)
            {
                groundObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
                groundObj.name = "Ground";
                groundObj.transform.position = Vector3.zero;
                groundObj.transform.localScale = new Vector3(10, 1, 100);
                groundObj.tag = "Track";
                
                Debug.Log("✓ Ground created with Track tag");
            }
            
            // Setup LevelGenerator GameObject
            var levelGenObj = GameObject.Find("LevelGenerator");
            if (levelGenObj == null)
            {
                levelGenObj = new GameObject("LevelGenerator");
            }
            
            Debug.Log("✓ Gameplay scene setup complete");
        }
        
        private void SetupShopScene()
        {
            Debug.Log("Setting up Shop scene...");
            
            // Setup ShopManager GameObject
            var shopManagerObj = GameObject.Find("ShopManager");
            if (shopManagerObj == null)
            {
                shopManagerObj = new GameObject("ShopManager");
            }
            
            Debug.Log("✓ Shop scene setup complete");
        }
        
        private void SetupONGSelectionScene()
        {
            Debug.Log("Setting up ONGSelection scene...");
            
            // Setup ONGSelectionManager GameObject
            var ongManagerObj = GameObject.Find("ONGSelectionManager");
            if (ongManagerObj == null)
            {
                ongManagerObj = new GameObject("ONGSelectionManager");
            }
            
            Debug.Log("✓ ONGSelection scene setup complete");
        }
        
        [ContextMenu("Fix Manager Hierarchy")]
        public void FixManagerHierarchy()
        {
            Debug.Log("Fixing manager hierarchy...");
            
            // Move all managers to root level
            MoveToRootByName("EventManager");
            MoveToRootByName("AudioManager");
            MoveToRootByName("AdManager");
            MoveToRootByName("DonationSystem");
            
            Debug.Log("✓ Manager hierarchy fixed");
        }
        
        private void MoveToRootByName(string managerName)
        {
            var manager = GameObject.Find(managerName);
            if (manager != null && manager.transform.parent != null)
            {
                manager.transform.SetParent(null);
                Debug.Log($"✓ {managerName} moved to root");
            }
        }
        
        [ContextMenu("Validate Game Setup")]
        public void ValidateGameSetup()
        {
            Debug.Log("=== Validating Game Setup ===");
            
            // Check persistent managers (should exist in all scenes)
            ValidateGameObjectExists("EventManager", true);
            ValidateGameObjectExists("AudioManager", true);
            ValidateGameObjectExists("AdManager", true);
            ValidateGameObjectExists("DonationSystem", true);
            
            // Scene-specific objects (optional, depends on scene)
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "MainMenu" || sceneName == "Gameplay")
            {
                var uiManager = GameObject.Find("UIManager");
                if (uiManager != null)
                {
                    Debug.Log($"✓ UIManager found in {sceneName}");
                }
                else
                {
                    Debug.Log($"ℹ UIManager not found in {sceneName} (will be created when needed)");
                }
                
                var gameManager = GameObject.Find("GameManager");
                if (gameManager != null)
                {
                    Debug.Log($"✓ GameManager found in {sceneName}");
                }
                else
                {
                    Debug.Log($"ℹ GameManager not found in {sceneName} (will be created when needed)");
                }
            }
            
            Debug.Log("=== Validation Complete ===");
        }
        
        private void ValidateGameObjectExists(string name, bool shouldBeRoot)
        {
            var obj = GameObject.Find(name);
            if (obj == null)
            {
                Debug.LogWarning($"⚠ {name} not found - it will be created when needed");
                return;
            }
            
            if (shouldBeRoot && obj.transform.parent != null)
            {
                Debug.LogWarning($"⚠ {name} is not at root level!");
                return;
            }
            
            Debug.Log($"✓ {name} configured correctly");
        }
    }
}
