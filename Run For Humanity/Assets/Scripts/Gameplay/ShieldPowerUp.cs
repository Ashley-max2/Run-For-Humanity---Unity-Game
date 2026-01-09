using UnityEngine;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Power-up que otorga un escudo temporal al jugador (invencibilidad)
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ShieldPowerUp : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private float duration = 5f; // Duración del escudo
        
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
            Debug.Log($"[ShieldPowerUp] ¡Power-up recogido! Escudo activo por {duration}s");
            
            // Obtener PlayerController
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Aplicar efecto de escudo
                playerController.ApplyShield(duration);
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
