using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RunForHumanity.UI
{
    public class GameOverPopup : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject popupPanel;
        [SerializeField] private TextMeshProUGUI totalCoinsText;
        [SerializeField] private TextMeshProUGUI runCoinsText;
        [SerializeField] private TextMeshProUGUI metersText;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button homeButton;

        private GameplayUIController gameplayUI;
        private int previousTotalCoins = 0;

        private void Awake()
        {
            if (popupPanel != null)
                popupPanel.SetActive(false);
        }

        private void Start()
        {
            gameplayUI = FindObjectOfType<GameplayUIController>();
            
            if (retryButton != null)
                retryButton.onClick.AddListener(Retry);
            
            if (homeButton != null)
                homeButton.onClick.AddListener(GoHome);

            LoadPreviousCoins();
            
            Core.Events.EventManager.OnGameOver += Show;
        }
        
        private void OnDestroy()
        {
            Core.Events.EventManager.OnGameOver -= Show;
        }

        private void LoadPreviousCoins()
        {
            previousTotalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            Debug.Log($"[GameOverPopup] Total de monedas previas: {previousTotalCoins}");
        }

        public void Show()
        {
            Debug.Log("[GameOverPopup] Show() llamado");
            
            if (popupPanel == null)
            {
                Debug.LogError("[GameOverPopup] popupPanel es null!");
                return;
            }
            
            StartCoroutine(ShowWithDelay());
            
            Debug.Log("[GameOverPopup] Popup mostrado correctamente");
        }
        
        private System.Collections.IEnumerator ShowWithDelay()
        {
            yield return new WaitForSeconds(1.5f);
            
            popupPanel.SetActive(true);
            UpdateStats();
        }

        public void Hide()
        {
            if (popupPanel != null)
                popupPanel.SetActive(false);
            
            Debug.Log("[GameOverPopup] Ocultado");
        }

        private void UpdateStats()
        {
            if (gameplayUI == null)
                return;

            int runCoins = gameplayUI.GetCurrentCoins();
            float meters = gameplayUI.GetCurrentMeters();
            int newTotalCoins = previousTotalCoins + runCoins;

            PlayerPrefs.SetInt("TotalCoins", newTotalCoins);
            PlayerPrefs.Save();

            if (totalCoinsText != null)
                totalCoinsText.text = $"Total: {newTotalCoins}";
            
            if (runCoinsText != null)
                runCoinsText.text = $"+{runCoins}";
            
            if (metersText != null)
                metersText.text = $"{Mathf.RoundToInt(meters)}m";
        }

        public void Retry()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }

        public void GoHome()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        [ContextMenu("Show Game Over Popup")]
        public void ShowFromMenu()
        {
            Show();
        }
    }
}
