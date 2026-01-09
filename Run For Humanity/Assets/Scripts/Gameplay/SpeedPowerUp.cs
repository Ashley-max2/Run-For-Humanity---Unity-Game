using UnityEngine;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Power-up que aumenta la velocidad del jugador temporalmente
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class SpeedPowerUp : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private float speedBoost = 5f; // Velocidad adicional
        [SerializeField] private float duration = 5f; // Duración del efecto
        
        [Header("Efectos Visuales")]
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private GameObject collectParticlePrefab;
        [SerializeField] private AudioClip collectSound;

        private void Start()
        {
            // Asegurar que el collider sea trigger
            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void Update()
        {
            // Rotación visual
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Verificar si es el jugador
            if (other.CompareTag("Player"))
            {
                CollectPowerUp(other.gameObject);
            }
        }

        void CollectPowerUp(GameObject player)
        {
            Debug.Log($"[SpeedPowerUp] ¡Power-up recogido! +{speedBoost} velocidad por {duration}s");
            
            // Obtener PlayerController
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Aplicar efecto de velocidad
                playerController.ApplySpeedBoost(speedBoost, duration);
            }
            
            // Crear partículas si hay prefab asignado
            if (collectParticlePrefab != null)
            {
                GameObject particles = Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);
                Destroy(particles, 2f);
            }
            
            // Reproducir sonido si hay clip asignado
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }
            
            // Destruir el power-up
            Destroy(gameObject);
        }
    }
}
