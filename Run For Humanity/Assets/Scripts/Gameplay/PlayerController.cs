using UnityEngine;
using DG.Tweening;
using RunForHumanity.Core.Events;
using RunForHumanity.Core.Input;

namespace RunForHumanity.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float forwardSpeed = 10f;
        public float speedIncreaseRate = 0.1f; // Aumento de velocidad por segundo
        public float maxSpeed = 30f;
        public float laneChangeSpeed = 20f; // Velocidad del cambio de carril suave (aumentada para mayor respuesta)
        public float jumpForce = 2f; // Fuerza de salto (1.5-2 recomendado)
        public float gravity = -9.81f; // Gravedad realista
        public float groundCheckDistance = 0.3f;
        public LayerMask groundLayer;

        [Header("Slide Settings")]
        public float slideDuration = 1f;
        private bool isSliding = false;
        private float slideTimer = 0f;
        private float normalHeight = 2f;
        private float slideHeight = 1f;

        [Header("Lane Change Settings")]
        public float laneChangeDuration = 0.15f; // Duración de la animación de cambio de carril (reducida para mayor respuesta)
        private bool isChangingLane = false;

        [Header("Particle Effects")]
        [SerializeField] private ParticleSystem runParticles;
        [SerializeField] private ParticleSystem jumpParticles;
        [SerializeField] private ParticleSystem slideParticles;
        [SerializeField] private GameObject deathParticlePrefab;

        [Header("Animation")]
        [SerializeField] private Animator animator;

        [Header("Lane Settings")]
        private int currentLane = 0; // -1, 0, 1
        
        // Power-up states
        private bool hasShield = false;
        private float shieldTimer = 0f;
        private bool hasMagnet = false;
        private float magnetTimer = 0f;
        private float magnetRange = 10f;
        private float speedBoostAmount = 0f;
        private float speedBoostTimer = 0f;
        
        // Public properties for external access
        public float CurrentSpeed => forwardSpeed;
        public bool HasShield => hasShield;
        public bool HasMagnet => hasMagnet;
        public float MagnetRange => magnetRange;

        // State
        private CharacterController controller;
        private Vector3 verticalVelocity;
        private bool isGrounded;
        private bool isDead = false;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            currentLane = LaneSystem.MIDDLE_LANE;
            
            normalHeight = controller.height;
            slideHeight = normalHeight * 0.5f;
            
            // Posicionar al player a la altura correcta
            if (transform.position.y < 1f)
            {
                transform.position = new Vector3(
                    LaneSystem.GetXPosition(currentLane),
                    1f, // Altura inicial sobre el suelo
                    transform.position.z
                );
            }

            // Subscribe to Events
            EventManager.TriggerGameStart(); // Auto start for prototype
            EventManager.OnPlayerHit += Die;
            
            // Iniciar partículas de correr
            if (runParticles != null)
            {
                runParticles.Play();
            }
            
            Debug.Log($"Player spawned at position: {transform.position}, CharacterController height: {controller.height}");
        }

        void OnDestroy()
        {
            EventManager.OnPlayerHit -= Die;
        }

        void Update()
        {
            if (isDead) return;

            HandleSlideTimer();
            HandlePowerUpTimers();
            HandleSpeedIncrease();
            HandleInput();
            MovePlayer();
            UpdateAnimations();
        }

        void UpdateAnimations()
        {
            if (animator == null) return;
            
            // Actualizar parámetros del Animator
            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("isSliding", isSliding);
        }

        void HandleSpeedIncrease()
        {
            // Aumentar velocidad automáticamente como Subway Surfers
            if (forwardSpeed < maxSpeed)
            {
                forwardSpeed += speedIncreaseRate * Time.deltaTime;
                forwardSpeed = Mathf.Min(forwardSpeed, maxSpeed);
            }
        }

        void HandleSlideTimer()
        {
            if (isSliding)
            {
                slideTimer -= Time.deltaTime;
                if (slideTimer <= 0f)
                {
                    EndSlide();
                }
            }
        }

        void HandlePowerUpTimers()
        {
            // Shield timer
            if (hasShield)
            {
                shieldTimer -= Time.deltaTime;
                if (shieldTimer <= 0f)
                {
                    hasShield = false;
                    Debug.Log("[Player] Shield deactivated");
                }
            }
            
            // Magnet timer
            if (hasMagnet)
            {
                magnetTimer -= Time.deltaTime;
                if (magnetTimer <= 0f)
                {
                    hasMagnet = false;
                    Debug.Log("[Player] Magnet deactivated");
                }
            }
            
            // Speed boost timer
            if (speedBoostTimer > 0f)
            {
                speedBoostTimer -= Time.deltaTime;
                if (speedBoostTimer <= 0f)
                {
                    forwardSpeed -= speedBoostAmount;
                    speedBoostAmount = 0f;
                    Debug.Log($"[Player] Speed boost ended - Speed: {forwardSpeed}");
                }
            }
        }

        void HandleInput()
        {
            // Keyboard input directo para testing
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
            }
            
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Slide();
            }
        }

        void MovePlayer()
        {
            // Forward speed constante (Subway Surfers style)
            Vector3 forwardMove = transform.forward * forwardSpeed * Time.deltaTime;

            // Ground check mejorado
            isGrounded = CheckGrounded();
            
            if (isGrounded && verticalVelocity.y < 0)
            {
                verticalVelocity.y = -2f; // Pequeña fuerza para mantenerlo pegado al suelo
            }
            
            // Aplicar gravedad
            if (!isGrounded)
            {
                verticalVelocity.y += gravity * Time.deltaTime;
            }

            // Movimiento lateral: cambio suave de carril con interpolación
            float targetX = LaneSystem.GetXPosition(currentLane);
            Vector3 targetPos = new Vector3(targetX, transform.position.y, transform.position.z);
            
            Vector3 diff = targetPos - transform.position;
            diff.y = 0;
            diff.z = 0;

            Vector3 lateralMove = Vector3.zero;
            if (diff.sqrMagnitude > 0.01f)
            {
                lateralMove = diff.normalized * laneChangeSpeed * Time.deltaTime;
                if (lateralMove.sqrMagnitude > diff.sqrMagnitude)
                {
                    lateralMove = diff;
                }
            }

            // COMBINE: forward + lateral + vertical
            controller.Move(forwardMove + lateralMove + verticalVelocity * Time.deltaTime);
        }

        bool CheckGrounded()
        {
            // Multiple ground checks para mayor fiabilidad
            if (controller.isGrounded) return true;

            // Raycast desde el centro
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            if (Physics.Raycast(rayOrigin, Vector3.down, groundCheckDistance, groundLayer))
            {
                return true;
            }

            // Raycast desde los lados del CharacterController
            float radius = controller.radius;
            Vector3[] offsets = new Vector3[]
            {
                new Vector3(radius, 0.1f, 0),
                new Vector3(-radius, 0.1f, 0),
                new Vector3(0, 0.1f, radius),
                new Vector3(0, 0.1f, -radius)
            };

            foreach (Vector3 offset in offsets)
            {
                if (Physics.Raycast(transform.position + offset, Vector3.down, groundCheckDistance, groundLayer))
                {
                    return true;
                }
            }

            return false;
        }

        void ChangeLane(int direction)
        {
            int newLane = currentLane + direction;
            
            // Clamp to valid lanes
            newLane = Mathf.Clamp(newLane, LaneSystem.LEFT_LANE, LaneSystem.RIGHT_LANE);
            
            if (newLane != currentLane)
            {
                currentLane = newLane;
                
                // Cancelar animación anterior si existe para respuesta instantánea
                transform.DOKill();
                isChangingLane = true;
                
                // Animación de inclinación al cambiar de carril (Subway Surfers style)
                // Duración más corta para respuesta más rápida
                float tiltAngle = direction * 15f; // Inclinación hacia el lado del movimiento
                transform.DORotate(new Vector3(0, 0, tiltAngle), laneChangeDuration * 0.3f)
                    .OnComplete(() => {
                        transform.DORotate(Vector3.zero, laneChangeDuration * 0.3f)
                            .OnComplete(() => isChangingLane = false);
                    });
                
                EventManager.TriggerTrackSpeedChanged(forwardSpeed);
            }
        }

        public void Jump()
        {
            // Permitir saltar siempre - cancela slide inmediatamente si está activo
            if (isSliding)
            {
                EndSlide();
            }
            
            // Saltos normales: solo si está en el suelo
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                EventManager.TriggerPlayerJump();
                
                // Activar animación de salto
                if (animator != null)
                {
                    animator.SetTrigger("Jump");
                }
                
                // Animación de salto estilo Subway Surfers (squash and stretch)
                transform.DOScaleY(1.2f, 0.1f).OnComplete(() => {
                    transform.DOScaleY(0.8f, 0.2f).OnComplete(() => {
                        transform.DOScaleY(1f, 0.1f);
                    });
                });
                
                // Partículas de salto
                if (jumpParticles != null)
                {
                    jumpParticles.Play();
                }
                
                Debug.Log($"[Player] Jump! Velocity: {verticalVelocity.y}");
            }
        }

        public void Die()
        {
            if (isDead) return; // Ya está muerto
            
            // Si tiene escudo, no muere
            if (hasShield)
            {
                Debug.Log("[Player] ¡Salvado por el escudo!");
                hasShield = false; // Consumir escudo
                shieldTimer = 0f;
                return;
            }
            
            isDead = true;
            Debug.Log("[Player] ¡Jugador MUERE!");
            
            // Activar animación de muerte
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }
            
            // Detener partículas de movimiento
            if (runParticles != null) runParticles.Stop();
            if (slideParticles != null) slideParticles.Stop();
            
            // Crear partículas de muerte
            if (deathParticlePrefab != null)
            {
                GameObject particles = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
                Destroy(particles, 3f);
            }
            
            EventManager.TriggerGameOver();
            
            // Animación de muerte
            transform.DOShakeScale(1f, 1f);
        }
        
        // Public methods for InputManager
        public void MoveLeft()
        {
            if (currentLane > LaneSystem.LEFT_LANE)
                ChangeLane(-1);
        }
        
        public void MoveRight()
        {
            if (currentLane < LaneSystem.RIGHT_LANE)
                ChangeLane(1);
        }
        
        public void Slide()
        {
            // Permitir slide en cualquier momento - cancela salto si está en el aire
            if (!isSliding)
            {
                // Si está saltando, forzar caída rápida al suelo
                if (!isGrounded)
                {
                    verticalVelocity.y = -20f; // Caída rápida para transición inmediata
                }
                StartSlide();
            }
        }

        void StartSlide()
        {
            isSliding = true;
            slideTimer = slideDuration;
            
            // Reducir altura del CharacterController (sin cambiar center)
            controller.height = slideHeight;
            controller.center = new Vector3(0, 0, 0);
            
            // Animación visual estilo Subway Surfers (aplastarse hacia abajo)
            transform.DOScaleY(0.4f, 0.15f);
            transform.DOScaleZ(1.3f, 0.15f); // Alargar en Z para efecto de velocidad
            
            // Partículas de deslizamiento
            if (slideParticles != null)
            {
                slideParticles.Play();
            }
            
            Debug.Log("[Player] Slide started");
        }

        void EndSlide()
        {
            isSliding = false;
            
            // Restaurar altura del CharacterController (sin cambiar center)
            controller.height = normalHeight;
            controller.center = new Vector3(0, 0, 0);
            
            // Restaurar escala visual
            transform.DOScaleY(1f, 0.15f);
            transform.DOScaleZ(1f, 0.15f);
            
            // Detener partículas de deslizamiento
            if (slideParticles != null)
            {
                slideParticles.Stop();
            }
            
            Debug.Log("[Player] Slide ended");
        }
        
        public void Dash()
        {
            // TODO: Implement dash mechanic
            Debug.Log("Dash triggered");
        }
        
        public void IncreaseSpeed(float amount)
        {
            forwardSpeed += amount;
        }
        
        // ===== POWER-UP METHODS =====
        
        public void ApplySpeedBoost(float boostAmount, float duration)
        {
            Debug.Log($"[Player] Speed boost applied: +{boostAmount} for {duration}s");
            
            // Si ya hay boost activo, quitarlo primero
            if (speedBoostTimer > 0f)
            {
                forwardSpeed -= speedBoostAmount;
            }
            
            // Aplicar nuevo boost
            speedBoostAmount = boostAmount;
            speedBoostTimer = duration;
            forwardSpeed += boostAmount;
            
            Debug.Log($"[Player] New speed: {forwardSpeed}");
        }
        
        public void ApplyShield(float duration)
        {
            Debug.Log($"[Player] Shield applied for {duration}s");
            hasShield = true;
            shieldTimer = duration;
            
            // TODO: Activar efecto visual del escudo (material, partículas, etc.)
        }
        
        public void ApplyMagnet(float duration, float range)
        {
            Debug.Log($"[Player] Magnet applied for {duration}s (range: {range})");
            hasMagnet = true;
            magnetTimer = duration;
            magnetRange = range;
            
            // TODO: Activar efecto visual del imán
        }
    }
}
