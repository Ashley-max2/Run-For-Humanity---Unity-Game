using UnityEngine;
using RunForHumanity.Data;
using RunForHumanity.Core;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Moneda que se puede recoger en el juego
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Coin : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private int coinValue = 1;
        [SerializeField] private float rotationSpeed = 100f;
        
        [Header("Efectos")]
        [SerializeField] private GameObject collectParticlePrefab;
        [SerializeField] private AudioClip collectSound;
        
        [Header("Movimiento (opcional)")]
        [SerializeField] private bool moveTowardsPlayer = false;
        [SerializeField] private float attractionSpeed = 5f;
        [SerializeField] private float attractionDistance = 5f;
        
        private Transform playerTransform;
        private bool isCollected = false;

        private void Start()
        {
            // Buscar al jugador
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            
            // Asegurar que el trigger esté activado
            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void Update()
        {
            if (isCollected) return;
            
            // Rotar la moneda
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            
            // Atraer hacia el jugador si está cerca (efecto imán)
            if (moveTowardsPlayer && playerTransform != null)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                if (distance < attractionDistance)
                {
                    Vector3 direction = (playerTransform.position - transform.position).normalized;
                    transform.position += direction * attractionSpeed * Time.deltaTime;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isCollected) return;
            
            // Verificar si es el jugador
            if (other.CompareTag("Player"))
            {
                CollectCoin();
            }
        }

        /// <summary>
        /// Recoge la moneda
        /// </summary>
        void CollectCoin()
        {
            isCollected = true;
            
            // Aplicar multiplicador de la tienda
            int multiplier = Data.ShopDataManager.GetCoinMultiplierLevel();
            int coinsToAdd = coinValue * multiplier;
            
            // Añadir monedas al sistema de guardado GLOBAL (con multiplicador)
            CoinDataManager.AddCoins(coinsToAdd);
            
            // Añadir monedas al UI del GAMEPLAY (monedas de esta partida con multiplicador)
            if (UI.GameplayUIController.Instance != null)
            {
                UI.GameplayUIController.Instance.AddCoins(coinsToAdd);
            }
            
            // Crear partículas si hay prefab asignado
            if (collectParticlePrefab != null)
            {
                GameObject particles = Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);
                Destroy(particles, 2f); // Destruir después de 2 segundos
            }
            
            // Reproducir sonido si hay clip asignado
            if (collectSound != null)
            {
                float sfxVolume = GameSettingsManager.Instance.GetNormalizedSFXVolume();
                AudioSource.PlayClipAtPoint(collectSound, transform.position, sfxVolume);
            }
            
            // Notificar al contador de monedas en la UI
            CoinUICounter[] counters = Object.FindObjectsOfType<CoinUICounter>();
            foreach (CoinUICounter counter in counters)
            {
                counter.UpdateDisplay();
            }
            
            Debug.Log($"[Coin] ¡Moneda recogida! +{coinValue}");
            
            // Destruir o desactivar la moneda
            Destroy(gameObject);
        }

        /// <summary>
        /// Activar el efecto imán (para power-up de imán)
        /// </summary>
        public void ActivateMagnet(float distance, float speed)
        {
            moveTowardsPlayer = true;
            attractionDistance = distance;
            attractionSpeed = speed;
        }
    }
}
