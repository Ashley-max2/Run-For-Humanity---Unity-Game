using UnityEngine;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Obstáculo que hace perder al jugador al chocar
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Obstacle : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private bool destroyOnHit = false;
        
        [Header("Efectos")]
        [SerializeField] private GameObject hitParticlePrefab;
        [SerializeField] private AudioClip hitSound;

        private void Start()
        {
            // Asegurar que el trigger esté activado
            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Verificar si es el jugador
            if (other.CompareTag("Player"))
            {
                HitPlayer(other.gameObject);
            }
        }

        void HitPlayer(GameObject player)
        {
            Debug.Log("[Obstacle] ¡Jugador chocó con obstáculo! - MUERTE");
            
            // Crear partículas del obstáculo en el punto de impacto
            if (hitParticlePrefab != null)
            {
                GameObject particles = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
                Destroy(particles, 2f);
            }
            
            // Reproducir sonido si hay clip asignado
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }
            
            // MATAR AL JUGADOR
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Die();
            }
            
            // Destruir el obstáculo si está configurado
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
