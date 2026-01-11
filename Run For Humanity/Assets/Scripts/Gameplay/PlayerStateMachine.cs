using UnityEngine;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Máquina de estados del jugador
    /// </summary>
    public class PlayerStateMachine : MonoBehaviour
    {
        public PlayerState CurrentState { get; private set; } = PlayerState.Idle;
        
        [Header("References")]
        [SerializeField] private Animator animator;
        
        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = true;

        private void Start()
        {
            if (animator == null)
                animator = GetComponent<Animator>();
                
            // Iniciar corriendo automáticamente
            ChangeState(PlayerState.Running);
        }

        /// <summary>
        /// Cambiar a un nuevo estado
        /// </summary>
        public void ChangeState(PlayerState newState)
        {
            if (CurrentState == newState) return;
            
            // No permitir cambios si está muerto
            if (CurrentState == PlayerState.Dead && newState != PlayerState.Dead)
                return;

            PlayerState previousState = CurrentState;
            CurrentState = newState;
            
            if (showDebugLogs)
                Debug.Log($"[PlayerStateMachine] {previousState} → {newState}");
            
            OnStateEnter(newState);
            UpdateAnimator();
        }

        /// <summary>
        /// Verificar si puede cambiar a un estado específico
        /// </summary>
        public bool CanTransitionTo(PlayerState newState)
        {
            // Muerto no puede cambiar de estado
            if (CurrentState == PlayerState.Dead)
                return false;

            switch (newState)
            {
                case PlayerState.Jumping:
                    // Solo puede saltar si está corriendo o deslizándose
                    return CurrentState == PlayerState.Running || CurrentState == PlayerState.Sliding;
                
                case PlayerState.Sliding:
                    // Solo puede deslizarse si está en el suelo
                    return CurrentState == PlayerState.Running;
                
                case PlayerState.Running:
                    // Puede volver a correr desde cualquier estado excepto muerto
                    return true;
                
                case PlayerState.Dead:
                    // Puede morir desde cualquier estado
                    return true;
                
                default:
                    return true;
            }
        }

        private void OnStateEnter(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.Dead:
                    OnDeathEnter();
                    break;
            }
        }

        private void OnDeathEnter()
        {
            // Notificar a otros componentes
            var playerDeath = GetComponent<PlayerDeath>();
            if (playerDeath != null)
            {
                playerDeath.OnStateMachineDeath();
            }
        }

        private void UpdateAnimator()
        {
            if (animator == null) return;

            // Resetear todos los estados
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isSliding", false);
            animator.SetBool("isDead", false);

            // Activar el estado actual
            switch (CurrentState)
            {
                case PlayerState.Running:
                    animator.SetBool("isRunning", true);
                    break;
                
                case PlayerState.Jumping:
                    animator.SetBool("isJumping", true);
                    animator.SetTrigger("Jump");
                    break;
                
                case PlayerState.Sliding:
                    animator.SetBool("isSliding", true);
                    animator.SetTrigger("Slide");
                    break;
                
                case PlayerState.Dead:
                    animator.SetBool("isDead", true);
                    break;
            }
        }

        /// <summary>
        /// Verificar si está en el suelo (para permitir saltos/slides)
        /// </summary>
        public bool IsGrounded()
        {
            return CurrentState == PlayerState.Running || CurrentState == PlayerState.Sliding;
        }

        /// <summary>
        /// Verificar si está muerto
        /// </summary>
        public bool IsDead()
        {
            return CurrentState == PlayerState.Dead;
        }
    }
}
