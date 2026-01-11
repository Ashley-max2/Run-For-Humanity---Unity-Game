using UnityEngine;
using DG.Tweening;
using RunForHumanity.Core.Events;
using RunForHumanity.Core.Input;
using RunForHumanity.Core;
using RFH.Input;
using UnityEngine.InputSystem;

namespace RunForHumanity.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float forwardSpeed = 10f;
        public float speedIncreaseRate = 0.1f;
        public float maxSpeed = 30f;
        public float laneChangeSpeed = 20f;
        public float jumpForce = 2f;
        public float gravity = -9.81f;
        public float groundCheckDistance = 0.3f;
        public LayerMask groundLayer;

        [Header("Slide Settings")]
        public float slideDuration = 1f;
        private bool isSliding = false;
        private float slideTimer = 0f;
        private float normalHeight = 2f;
        private float slideHeight = 1f;

        [Header("Lane Change Settings")]
        public float laneChangeDuration = 0.15f;
        #pragma warning disable 0414
        private bool isChangingLane = false;
        #pragma warning restore 0414

        [Header("Particle Effects")]
        [SerializeField] private ParticleSystem runParticles;
        [SerializeField] private ParticleSystem jumpParticles;
        [SerializeField] private ParticleSystem slideParticles;
        [SerializeField] private GameObject deathParticlePrefab;
        
        [Header("Audio")]
        [SerializeField] private AudioClip deathSound;

        [Header("Animation")]
        [SerializeField] private Animator animator;

        [Header("Lane Settings")]
        private int currentLane = 0;
        
        private bool hasShield = false;
        private float shieldTimer = 0f;
        private bool hasMagnet = false;
        private float magnetTimer = 0f;
        private float magnetRange = 10f;
        private float speedBoostAmount = 0f;
        private float speedBoostTimer = 0f;
        
        private float totalMeters = 0f;
        private Vector3 lastPosition;
        
        private bool has500MetersVibrated = false;
        private Coroutine vibrationCoroutine;
        
        public float CurrentSpeed => forwardSpeed;
        public bool HasShield => hasShield;
        public bool HasMagnet => hasMagnet;
        public float MagnetRange => magnetRange;

        private CharacterController controller;
        private Vector3 verticalVelocity;
        private bool isGrounded;
        private bool isDead = false;
        
        private PlayerInputActions playerInputActions;
        private InputAction movementAction;
        private InputAction jumpAction;
        private InputAction slideAction;
        
        private SwipeDetector swipeDetector;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            currentLane = LaneSystem.MIDDLE_LANE;
            
            SetupSwipeDetector();
            
            normalHeight = controller.height;
            slideHeight = normalHeight * 0.5f;
            
            if (transform.position.y < 1f)
            {
                transform.position = new Vector3(
                    LaneSystem.GetXPosition(currentLane),
                    1f,
                    transform.position.z
                );
            }
            
            lastPosition = transform.position;

            EventManager.TriggerGameStart();
            EventManager.OnPlayerHit += Die;
            
            if (runParticles != null)
            {
                runParticles.Play();
            }
            
            Debug.Log($"Player spawned at position: {transform.position}, CharacterController height: {controller.height}");
        }
        
        void SetupSwipeDetector()
        {
            swipeDetector = GetComponent<SwipeDetector>();
            if (swipeDetector == null)
            {
                swipeDetector = gameObject.AddComponent<SwipeDetector>();
            }
            
            swipeDetector.OnSwipeLeft += MoveLeft;
            swipeDetector.OnSwipeRight += MoveRight;
            swipeDetector.OnSwipeUp += Jump;
            swipeDetector.OnSwipeDown += Slide;
            
            Debug.Log("[PlayerController] Swipe detector configurado");
        }

        void OnDestroy()
        {
            EventManager.OnPlayerHit -= Die;
            
            if (swipeDetector != null)
            {
                swipeDetector.OnSwipeLeft -= MoveLeft;
                swipeDetector.OnSwipeRight -= MoveRight;
                swipeDetector.OnSwipeUp -= Jump;
                swipeDetector.OnSwipeDown -= Slide;
            }
        }

        void Update()
        {
            if (isDead) return;

            HandleSlideTimer();
            HandlePowerUpTimers();
            HandleSpeedIncrease();
            
            #if UNITY_EDITOR || UNITY_STANDALONE
            HandleInput();
            #endif
            
            MovePlayer();
            UpdateAnimations();
            TrackMeters();
        }
        
        void TrackMeters()
        {
            float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), 
                                             new Vector3(lastPosition.x, 0, lastPosition.z));
            totalMeters += distance;
            lastPosition = transform.position;
            
            if (totalMeters >= 500f && !has500MetersVibrated)
            {
                has500MetersVibrated = true;
                if (vibrationCoroutine != null) StopCoroutine(vibrationCoroutine);
                vibrationCoroutine = StartCoroutine(VibrateFor500Meters());
            }
            
            if (UI.GameplayUIController.Instance != null)
            {
                UI.GameplayUIController.Instance.SetMeters(totalMeters);
            }
        }
        
        // Vibra 3s al llegar a 500m
        private System.Collections.IEnumerator VibrateFor500Meters()
        {
            Debug.Log("[PlayerController] ¡500 metros alcanzados! Vibrando durante 3 segundos");
            
            float elapsedTime = 0f;
            while (elapsedTime < 3f)
            {
                #if UNITY_ANDROID || UNITY_IOS
                Handheld.Vibrate();
                #endif
                
                yield return new WaitForSeconds(0.5f);
                elapsedTime += 0.5f;
            }
            
            Debug.Log("[PlayerController] Vibración de 500 metros completada");
        }

        void UpdateAnimations()
        {
            if (animator == null) return;
            
            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("isSliding", isSliding);
            animator.SetBool("isJumping", !isGrounded);
            animator.SetBool("isRunning", !isDead && isGrounded && !isSliding);
        }

        void HandleSpeedIncrease()
        {
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
            if (hasShield)
            {
                shieldTimer -= Time.deltaTime;
                if (shieldTimer <= 0f)
                {
                    hasShield = false;
                    Debug.Log("[Player] Shield deactivated");
                }
            }
            
            if (hasMagnet)
            {
                magnetTimer -= Time.deltaTime;
                if (magnetTimer <= 0f)
                {
                    hasMagnet = false;
                    Debug.Log("[Player] Magnet deactivated");
                }
            }
            
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
            Vector3 forwardMove = transform.forward * forwardSpeed * Time.deltaTime;

            isGrounded = CheckGrounded();
            
            if (isGrounded && verticalVelocity.y < 0)
            {
                verticalVelocity.y = -2f;
            }
            
            if (!isGrounded)
            {
                verticalVelocity.y += gravity * Time.deltaTime;
            }

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

            controller.Move(forwardMove + lateralMove + verticalVelocity * Time.deltaTime);
        }

        bool CheckGrounded()
        {
            if (controller.isGrounded) return true;

            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            if (Physics.Raycast(rayOrigin, Vector3.down, groundCheckDistance, groundLayer))
            {
                return true;
            }

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
            
            newLane = Mathf.Clamp(newLane, LaneSystem.LEFT_LANE, LaneSystem.RIGHT_LANE);
            
            if (newLane != currentLane)
            {
                currentLane = newLane;
                
                transform.DOKill();
                isChangingLane = true;
                
                float tiltAngle = direction * 15f;
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
            if (isSliding)
            {
                EndSlide();
            }
            
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                EventManager.TriggerPlayerJump();
                
                if (animator != null)
                {
                    animator.SetTrigger("Jump");
                }
                
                transform.DOScaleY(1.2f, 0.1f).OnComplete(() => {
                    transform.DOScaleY(0.8f, 0.2f).OnComplete(() => {
                        transform.DOScaleY(1f, 0.1f);
                    });
                });
                
                if (jumpParticles != null)
                {
                    jumpParticles.Play();
                }
                
                Debug.Log($"[Player] Jump! Velocity: {verticalVelocity.y}");
            }
        }

        public void Die()
        {
            if (isDead) return;
            
            if (hasShield)
            {
                Debug.Log("[Player] ¡Salvado por el escudo!");
                hasShield = false;
                shieldTimer = 0f;
                return;
            }
            
            isDead = true;
            Debug.Log("[Player] ¡Jugador MUERE!");
            
            StartCoroutine(FlashOnDeath());
            
            if (animator != null)
            {
                animator.SetBool("isDead", true);
            }
            
            if (runParticles != null) runParticles.Stop();
            if (slideParticles != null) slideParticles.Stop();
            
            if (deathParticlePrefab != null)
            {
                GameObject particles = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
                Destroy(particles, 3f);
            }
            
            if (deathSound != null)
            {
                float sfxVolume = GameSettingsManager.Instance.GetNormalizedSFXVolume();
                AudioSource.PlayClipAtPoint(deathSound, transform.position, sfxVolume);
            }
            
            EventManager.TriggerGameOver();
            
            transform.DOShakeScale(1f, 1f);
        }
        
        // SENSOR: Flash de cámara 1s al morir
        private System.Collections.IEnumerator FlashOnDeath()
        {
            Debug.Log("[PlayerController] Activando flash de cámara por muerte");
            
            #if UNITY_ANDROID
            AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
            AndroidJavaObject camera = null;
            AndroidJavaObject parameters = null;
            bool flashActivated = false;
            
            try
            {
                camera = cameraClass.CallStatic<AndroidJavaObject>("open");
                parameters = camera.Call<AndroidJavaObject>("getParameters");
                
                parameters.Call("setFlashMode", "torch");
                camera.Call("setParameters", parameters);
                camera.Call("startPreview");
                
                flashActivated = true;
                Debug.Log("[PlayerController] Flash activado");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[PlayerController] No se pudo activar el flash: {e.Message}");
            }
            
            yield return new WaitForSeconds(1f);
            
            if (flashActivated && camera != null)
            {
                try
                {
                    parameters.Call("setFlashMode", "off");
                    camera.Call("setParameters", parameters);
                    camera.Call("stopPreview");
                    camera.Call("release");
                    Debug.Log("[PlayerController] Flash desactivado");
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[PlayerController] Error al apagar flash: {e.Message}");
                    try { camera.Call("release"); } catch { }
                }
            }
            #elif UNITY_IOS
            Debug.Log("[PlayerController] iOS: Simulando flash con pantalla blanca");
            yield return new WaitForSeconds(1f);
            #else
            Debug.Log("[PlayerController] Flash no disponible en esta plataforma");
            yield return new WaitForSeconds(1f);
            #endif
        }
        
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
            if (!isSliding)
            {
                if (!isGrounded)
                {
                    verticalVelocity.y = -20f;
                }
                StartSlide();
            }
        }

        void StartSlide()
        {
            isSliding = true;
            slideTimer = slideDuration;
            
            controller.height = slideHeight;
            controller.center = new Vector3(0, 0, 0);
            
            transform.DOScaleY(0.4f, 0.15f);
            transform.DOScaleZ(1.3f, 0.15f);
            
            if (animator != null)
            {
                animator.SetTrigger("Slide");
            }
            
            if (slideParticles != null)
            {
                slideParticles.Play();
            }
            
            Debug.Log("[Player] Slide started");
        }

        void EndSlide()
        {
            isSliding = false;
            
            controller.height = normalHeight;
            controller.center = new Vector3(0, 0, 0);
            
            transform.DOScaleY(1f, 0.15f);
            transform.DOScaleZ(1f, 0.15f);
            
            if (slideParticles != null)
            {
                slideParticles.Stop();
            }
            
            Debug.Log("[Player] Slide ended");
        }
        
        public void Dash()
        {
            Debug.Log("Dash triggered");
        }
        
        public void IncreaseSpeed(float amount)
        {
            forwardSpeed += amount;
        }
        
        public void ApplySpeedBoost(float boostAmount, float duration)
        {
            Debug.Log($"[Player] Speed boost applied: +{boostAmount} for {duration}s");
            
            if (speedBoostTimer > 0f)
            {
                forwardSpeed -= speedBoostAmount;
            }
            
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
        }
        
        public void ApplyMagnet(float duration, float range)
        {
            Debug.Log($"[Player] Magnet applied for {duration}s (range: {range})");
            hasMagnet = true;
            magnetTimer = duration;
            magnetRange = range;
        }
    }
}
