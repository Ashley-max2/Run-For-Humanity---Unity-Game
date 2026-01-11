using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RunForHumanity.Data;

namespace RunForHumanity.UI
{
    public class ShopUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI coinAmountText;
        [SerializeField] private TextMeshProUGUI multiplierLevelText;
        [SerializeField] private TextMeshProUGUI multiplierPriceText;
        [SerializeField] private Button buyMultiplierButton;
        
        [Header("Settings")]
        [SerializeField] private int basePrice = 100;
        [SerializeField] private float growthMultiplier = 1.5f;
        
        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = true;

        private void Start()
        {
            if (buyMultiplierButton != null)
            {
                buyMultiplierButton.onClick.AddListener(BuyMultiplierUpgrade);
            }
            
            RefreshUI();
            
            if (showDebugLogs)
                Debug.Log("[ShopUI] Tienda inicializada");
        }

        public void RefreshUI()
        {
            UpdateCoinDisplay();
            UpdateMultiplierDisplay();
        }

        private void UpdateCoinDisplay()
        {
            if (coinAmountText != null)
            {
                int coins = CoinDataManager.GetTotalCoins();
                coinAmountText.text = coins.ToString();
                
                if (showDebugLogs)
                    Debug.Log($"[ShopUI] Monedas actualizadas: {coins}");
            }
        }

        private void UpdateMultiplierDisplay()
        {
            int currentLevel = ShopDataManager.GetCoinMultiplierLevel();
            int nextPrice = ShopDataManager.GetNextMultiplierPrice(basePrice, growthMultiplier);
            int currentCoins = CoinDataManager.GetTotalCoins();
            
            if (multiplierLevelText != null)
            {
                multiplierLevelText.text = $"x{currentLevel}";
            }
            
            if (multiplierPriceText != null)
            {
                multiplierPriceText.text = $"{nextPrice}";
            }
            
            if (buyMultiplierButton != null)
            {
                buyMultiplierButton.interactable = currentCoins >= nextPrice;
            }
            
            if (showDebugLogs)
                Debug.Log($"[ShopUI] Multiplicador nivel {currentLevel}, próximo precio: {nextPrice}");
        }

        public void BuyMultiplierUpgrade()
        {
            int price = ShopDataManager.GetNextMultiplierPrice(basePrice, growthMultiplier);
            
            if (ShopDataManager.UpgradeCoinMultiplier(price))
            {
                if (showDebugLogs)
                    Debug.Log($"[ShopUI] Multiplicador comprado por {price} monedas");
                
                RefreshUI();
                
                PlayPurchaseEffect();
            }
            else
            {
                if (showDebugLogs)
                    Debug.LogWarning("[ShopUI] No hay suficientes monedas para comprar");
                
                ShowInsufficientCoinsMessage();
            }
        }

        private void PlayPurchaseEffect()
        {
            Debug.Log("[ShopUI] ¡Compra exitosa! ✨");
        }

        private void ShowInsufficientCoinsMessage()
        {
            Debug.Log("[ShopUI] ❌ No tienes suficientes monedas");
        }
        
        [ContextMenu("Reset Shop Data")]
        public void ResetShopData()
        {
            ShopDataManager.ResetData();
            RefreshUI();
            Debug.Log("[ShopUI] Tienda reseteada");
        }
    }
}
