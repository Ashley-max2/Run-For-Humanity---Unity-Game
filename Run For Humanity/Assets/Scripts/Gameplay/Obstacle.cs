using UnityEngine;
using RunForHumanity.Core;

namespace RunForHumanity.Gameplay
{
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
            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HitPlayer(other.gameObject);
            }
        }

        void HitPlayer(GameObject player)
        {
            Debug.Log("[Obstacle] ¡Jugador chocó con obstáculo! - MUERTE");
            
            if (hitParticlePrefab != null)
            {
                GameObject particles = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
                Destroy(particles, 2f);
            }
            
            if (hitSound != null)
            {
                float sfxVolume = GameSettingsManager.Instance.GetNormalizedSFXVolume();
                AudioSource.PlayClipAtPoint(hitSound, transform.position, sfxVolume);
            }
            
            // Mata al jugador
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Die();
            }
            
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
