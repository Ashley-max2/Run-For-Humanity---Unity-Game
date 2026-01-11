using UnityEngine;
using System;
using RunForHumanity.Core;
using RunForHumanity.Gameplay;

namespace RunForHumanity.Systems
{
    public class InputManager : MonoBehaviour, IInitializable
    {
        [Header("Input Settings")]
        [SerializeField] private bool _enableTouchInput = true;
        [SerializeField] private bool _enableKeyboardInput = true;
        [SerializeField] private bool _enableSensorInput = true;
        
        [Header("Swipe Detection")]
        [SerializeField] private float _swipeThreshold = 50f;
        [SerializeField] private float _tapTimeThreshold = 0.3f;
        
        private Vector2 _touchStartPos;
        private float _touchStartTime;
        private bool _isSwiping;
        
        private PlayerController _playerController;
        private SensorManager _sensorManager;
        
        public event Action OnSwipeLeft;
        public event Action OnSwipeRight;
        public event Action OnSwipeUp;
        public event Action OnSwipeDown;
        public event Action OnTap;
        
        public bool IsInitialized { get; private set; }
        
        public void Initialize()
        {
            if (_enableSensorInput)
            {
                _sensorManager = ServiceLocator.GetService<SensorManager>();
                
                if (_sensorManager != null)
                {
                    _sensorManager.OnTiltDetected += HandleTilt;
                    _sensorManager.OnShakeDetected += HandleShake;
                }
            }
            
            ServiceLocator.RegisterService(this);
            IsInitialized = true;
            Debug.Log("[InputManager] Initialized");
        }
        
        private void Update()
        {
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (sceneName != "Gameplay") return;
            
            if (GameManager.Instance?.CurrentState != GameState.Playing)
                return;
            
            if (_playerController == null)
            {
                _playerController = FindObjectOfType<PlayerController>();
                if (_playerController == null) return;
            }
            
            HandleTouchInput();
            HandleKeyboardInput();
        }
        
        private void HandleTouchInput()
        {
            if (!_enableTouchInput) return;
            
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _touchStartPos = touch.position;
                        _touchStartTime = Time.time;
                        _isSwiping = false;
                        break;
                    
                    case TouchPhase.Moved:
                        if (!_isSwiping)
                        {
                            Vector2 swipeDelta = touch.position - _touchStartPos;
                            
                            if (swipeDelta.magnitude > _swipeThreshold)
                            {
                                _isSwiping = true;
                                DetectSwipeDirection(swipeDelta);
                            }
                        }
                        break;
                    
                    case TouchPhase.Ended:
                        float touchDuration = Time.time - _touchStartTime;
                        
                        if (!_isSwiping && touchDuration < _tapTimeThreshold)
                        {
                            OnTap?.Invoke();
                            _playerController?.Jump();
                        }
                        break;
                }
            }
        }
        
        private void HandleKeyboardInput()
        {
            if (!_enableKeyboardInput) return;
            
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnSwipeLeft?.Invoke();
                _playerController?.MoveLeft();
            }
            
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnSwipeRight?.Invoke();
                _playerController?.MoveRight();
            }
            
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                OnSwipeUp?.Invoke();
                _playerController?.Jump();
            }
            
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnSwipeDown?.Invoke();
                _playerController?.Slide();
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                _playerController?.Dash();
            }
        }
        
        private void DetectSwipeDirection(Vector2 swipeDelta)
        {
            float absX = Mathf.Abs(swipeDelta.x);
            float absY = Mathf.Abs(swipeDelta.y);
            
            if (absX > absY)
            {
                if (swipeDelta.x > 0)
                {
                    OnSwipeRight?.Invoke();
                    _playerController?.MoveRight();
                }
                else
                {
                    OnSwipeLeft?.Invoke();
                    _playerController?.MoveLeft();
                }
            }
            else
            {
                if (swipeDelta.y > 0)
                {
                    OnSwipeUp?.Invoke();
                    _playerController?.Jump();
                }
                else
                {
                    OnSwipeDown?.Invoke();
                    _playerController?.Slide();
                }
            }
        }
        
        private void HandleTilt(float tiltValue)
        {
            if (tiltValue < -0.3f)
            {
                _playerController?.MoveLeft();
            }
            else if (tiltValue > 0.3f)
            {
                _playerController?.MoveRight();
            }
        }
        
        private void HandleShake()
        {
            _playerController?.Dash();
        }
        
        private void OnDestroy()
        {
            if (_sensorManager != null)
            {
                _sensorManager.OnTiltDetected -= HandleTilt;
                _sensorManager.OnShakeDetected -= HandleShake;
            }
        }
    }
}
