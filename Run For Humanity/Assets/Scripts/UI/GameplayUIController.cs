using UnityEngine;
using TMPro;

namespace RunForHumanity.UI
{
    public class GameplayUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI metersText;
        
        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = true;
        
        private int currentCoins = 0;
        private float currentMeters = 0f;
        
        private static GameplayUIController instance;
        public static GameplayUIController Instance => instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            
            if (showDebugLogs)
                Debug.Log("[GameplayUI] Instancia creada");
        }

        private void Start()
        {
            if (coinsText == null)
                Debug.LogError("[GameplayUI] coinsText no está asignado!");
            if (metersText == null)
                Debug.LogError("[GameplayUI] metersText no está asignado!");
                
            ResetStats();
        }

        public void AddCoins(int amount)
        {
            currentCoins += amount;
            UpdateCoinsUI();
            
            if (showDebugLogs)
                Debug.Log($"[GameplayUI] +{amount} monedas. Total: {currentCoins}");
        }

        public void SetMeters(float meters)
        {
            currentMeters = meters;
            UpdateMetersUI();
        }

        private void UpdateUI()
        {
            UpdateCoinsUI();
            UpdateMetersUI();
        }

        private void UpdateCoinsUI()
        {
            if (coinsText != null)
            {
                coinsText.text = currentCoins.ToString();
                
                if (showDebugLogs)
                    Debug.Log($"[GameplayUI] Texto de monedas actualizado: {currentCoins}");
            }
            else
            {
                Debug.LogError("[GameplayUI] coinsText es NULL! No se puede actualizar.");
            }
        }

        private void UpdateMetersUI()
        {
            if (metersText != null)
            {
                metersText.text = Mathf.RoundToInt(currentMeters).ToString() + "m";
                
                if (showDebugLogs)
                    Debug.Log($"[GameplayUI] Texto de metros actualizado: {Mathf.RoundToInt(currentMeters)}m");
            }
            else
            {
                Debug.LogError("[GameplayUI] metersText es NULL! No se puede actualizar.");
            }
        }

        public int GetCurrentCoins() => currentCoins;
        public float GetCurrentMeters() => currentMeters;

        public void ResetStats()
        {
            currentCoins = 0;
            currentMeters = 0f;
            UpdateUI();
            
            if (showDebugLogs)
                Debug.Log("[GameplayUI] Estadísticas reseteadas");
        }
    }
}
