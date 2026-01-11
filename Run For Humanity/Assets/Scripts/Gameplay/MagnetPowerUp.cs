using UnityEngine;
using RunForHumanity.Core;

namespace RunForHumanity.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class MagnetPowerUp : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private float duration = 5f; // Duración del imán
        [SerializeField] private float magnetRange = 10f; // Rango de atracción
        
        [Header("Efectos Visuales")]
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private GameObject collectParticlePrefab;
        [SerializeField] private AudioClip collectSound;

        private void Start()
        {
            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CollectPowerUp(other.gameObject);
            }
        }

        void CollectPowerUp(GameObject player)
        {
            Debug.Log($"[MagnetPowerUp] ¡Power-up recogido! Imán activo por {duration}s (rango: {magnetRange})");
            
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ApplyMagnet(duration, magnetRange);
            }
            
            if (collectParticlePrefab != null)
            {
                GameObject particles = Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);
                Destroy(particles, 2f);
            }
            
            // Reproducir sonido si hay clip asignado
            if (collectSound != null)
            {
                float sfxVolume = GameSettingsManager.Instance.GetNormalizedSFXVolume();
                AudioSource.PlayClipAtPoint(collectSound, transform.position, sfxVolume);
            }
            
            // Destruir el power-up
            Destroy(gameObject);
        }
    }
}
