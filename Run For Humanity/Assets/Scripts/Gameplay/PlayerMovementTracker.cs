using UnityEngine;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Rastrea el movimiento del jugador para calcular los metros recorridos
    /// Añade este script al jugador
    /// </summary>
    public class PlayerMovementTracker : MonoBehaviour
    {
        [SerializeField] private bool showDebugLogs = true; // Cambiado a true por defecto
        [SerializeField] private float updateInterval = 0.1f; // Actualizar cada 0.1s para optimizar
        
        private Vector3 lastPosition;
        private float totalDistance = 0f;
        private float timeSinceLastUpdate = 0f;
        private PlayerDeath playerDeath;

        private void Awake()
        {
            playerDeath = GetComponent<PlayerDeath>();
        }

        private void Start()
        {
            lastPosition = transform.position;
            
            if (showDebugLogs)
                Debug.Log($"[MovementTracker] Iniciado en posición: {lastPosition}");
        }

        private void Update()
        {
            // No trackear si está muerto
            if (playerDeath != null && playerDeath.IsDead())
            {
                if (showDebugLogs && timeSinceLastUpdate == 0f)
                    Debug.Log("[MovementTracker] Jugador muerto, no se trackea movimiento");
                return;
            }

            // Calcular distancia desde la última posición
            float distance = Vector3.Distance(transform.position, lastPosition);
            totalDistance += distance;
            lastPosition = transform.position;

            // Actualizar UI cada cierto intervalo para optimizar
            timeSinceLastUpdate += Time.deltaTime;
            if (timeSinceLastUpdate >= updateInterval)
            {
                if (UI.GameplayUIController.Instance != null)
                {
                    UI.GameplayUIController.Instance.SetMeters(totalDistance);
                    
                    if (showDebugLogs)
                        Debug.Log($"[MovementTracker] Distancia actualizada: {totalDistance:F2}m");
                }
                else
                {
                    Debug.LogError("[MovementTracker] GameplayUIController.Instance es NULL!");
                }
                
                timeSinceLastUpdate = 0f;
            }
        }

        public float GetTotalDistance() => totalDistance;

        public void ResetDistance()
        {
            totalDistance = 0f;
            lastPosition = transform.position;
            
            if (showDebugLogs)
                Debug.Log("[MovementTracker] Distancia reseteada");
        }
    }
}
