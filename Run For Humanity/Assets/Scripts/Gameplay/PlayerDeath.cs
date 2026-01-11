using UnityEngine;
using System.Collections;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Detecta la muerte del jugador y muestra el popup de Game Over
    /// Añade este script al jugador
    /// </summary>
    public class PlayerDeath : MonoBehaviour
    {
        private UI.GameOverPopup gameOverPopup;
        private PlayerStateMachine stateMachine;
        private bool isDead = false;

        [SerializeField] private float deathDelay = 0.5f; // Delay antes de mostrar el popup

        private void Awake()
        {
            stateMachine = GetComponent<PlayerStateMachine>();
        }

        private void Start()
        {
            gameOverPopup = FindObjectOfType<UI.GameOverPopup>();
            
            if (gameOverPopup == null)
            {
                Debug.LogError("[PlayerDeath] No se encontró GameOverPopup en la escena!");
            }
            else
            {
                Debug.Log("[PlayerDeath] GameOverPopup encontrado correctamente");
            }
        }

        /// <summary>
        /// Llama este método cuando el jugador muere
        /// </summary>
        public void Die()
        {
            if (isDead) return;
            
            isDead = true;
            Debug.Log("[PlayerDeath] El jugador ha muerto - iniciando secuencia de muerte");
            
            // Cambiar estado a muerto
            if (stateMachine != null)
            {
                stateMachine.ChangeState(PlayerState.Dead);
            }

            // Mostrar popup después de un pequeño delay
            StartCoroutine(ShowGameOverDelayed());
        }

        private IEnumerator ShowGameOverDelayed()
        {
            yield return new WaitForSecondsRealtime(deathDelay);
            
            Debug.Log("[PlayerDeath] Mostrando popup de Game Over");
            
            if (gameOverPopup != null)
            {
                gameOverPopup.Show();
            }
            else
            {
                Debug.LogError("[PlayerDeath] gameOverPopup es null al intentar mostrarlo!");
            }
        }

        /// <summary>
        /// Llamado por la máquina de estados cuando entra en estado Dead
        /// </summary>
        public void OnStateMachineDeath()
        {
            // Aquí puedes añadir efectos de muerte, partículas, etc.
            Debug.Log("[PlayerDeath] Estado cambiado a Dead en la máquina de estados");
        }

        // Detectar colisión con obstáculos
        private void OnCollisionEnter(Collision collision)
        {
            if (isDead) return;
            
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log($"[PlayerDeath] Colisión con obstáculo: {collision.gameObject.name}");
                Die();
            }
        }

        // Detectar trigger con obstáculos
        private void OnTriggerEnter(Collider other)
        {
            if (isDead) return;
            
            if (other.CompareTag("Obstacle"))
            {
                Debug.Log($"[PlayerDeath] Trigger con obstáculo: {other.gameObject.name}");
                Die();
            }
        }

        /// <summary>
        /// Método público para matar al jugador desde otros scripts
        /// </summary>
        public void Kill()
        {
            Die();
        }

        public bool IsDead() => isDead;
    }
}
