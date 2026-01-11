using UnityEngine;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Recolecta monedas cuando el jugador pasa por ellas
    /// Añade este script al jugador
    /// </summary>
    public class CoinCollector : MonoBehaviour
    {
        [SerializeField] private int coinValue = 1;
        [SerializeField] private bool showDebugLogs = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coin"))
            {
                if (showDebugLogs)
                    Debug.Log($"[CoinCollector] Moneda detectada: {other.gameObject.name}");
                    
                CollectCoin(other.gameObject);
            }
        }

        private void CollectCoin(GameObject coin)
        {
            // Añadir monedas al UI
            if (UI.GameplayUIController.Instance != null)
            {
                UI.GameplayUIController.Instance.AddCoins(coinValue);
                
                if (showDebugLogs)
                    Debug.Log($"[CoinCollector] Moneda recolectada: +{coinValue}");
            }
            else
            {
                Debug.LogError("[CoinCollector] GameplayUIController.Instance es null!");
            }

            // Destruir la moneda
            Destroy(coin);
        }
    }
}
