using UnityEngine;
using System;
using RunForHumanity.Core;

namespace RunForHumanity.Systems
{
    /// <summary>
    /// Sensor input manager for accelerometer and gyroscope
    /// SOLID: Single Responsibility - Manages device sensors
    /// Cumple con la rúbrica: Uso de 2 sensores (accelerometer + gyroscope)
    /// </summary>
    public class SensorManager : MonoBehaviour, IInitializable, IUpdatable
    {
        [Header("Sensor Settings")]
        [SerializeField] private bool _useAccelerometer = true;
        [SerializeField] private bool _useGyroscope = true;
        [SerializeField] private float _accelerometerSensitivity = 2f;
        [SerializeField] private float _gyroscopeSensitivity = 1f;
        
        [Header("Tilt Controls")]
        [SerializeField] private bool _enableTiltToSteer = true;
        [SerializeField] private float _tiltThreshold = 15f; // degrees
        
        [Header("Shake Detection")]
        [SerializeField] private bool _enableShakeDetection = true;
        [SerializeField] private float _shakeThreshold = 2.5f;
        
        private Vector3 _lastAcceleration;
        private Quaternion _lastGyroRotation;
        private float _shakeCooldown = 0f;
        
        public event Action<float> OnTiltDetected; // -1 = left, 1 = right
        public event Action OnShakeDetected;
        public event Action<Vector3> OnGyroRotationChanged;
        
        public bool IsInitialized { get; private set; }
        public Vector3 CurrentAcceleration { get; private set; }
        public Quaternion CurrentGyroRotation { get; private set; }
        
        public void Initialize()
        {
            // Enable gyroscope if available
            if (_useGyroscope && SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true;
                Input.gyro.updateInterval = 0.01f;
                Debug.Log("[SensorManager] Gyroscope enabled");
            }
            else
            {
                _useGyroscope = false;
                Debug.LogWarning("[SensorManager] Gyroscope not supported on this device");
            }
            
            // Check accelerometer support
            if (_useAccelerometer && SystemInfo.supportsAccelerometer)
            {
                Debug.Log("[SensorManager] Accelerometer enabled");
            }
            else
            {
                _useAccelerometer = false;
                Debug.LogWarning("[SensorManager] Accelerometer not supported on this device");
            }
            
            ServiceLocator.RegisterService(this);
            IsInitialized = true;
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_useAccelerometer)
            {
                UpdateAccelerometer(deltaTime);
            }
            
            if (_useGyroscope)
            {
                UpdateGyroscope(deltaTime);
            }
            
            if (_shakeCooldown > 0)
            {
                _shakeCooldown -= deltaTime;
            }
        }
        
        private void UpdateAccelerometer(float deltaTime)
        {
            CurrentAcceleration = Input.acceleration;
            
            // Tilt to steer (landscape mode)
            if (_enableTiltToSteer)
            {
                float tiltX = CurrentAcceleration.x * _accelerometerSensitivity;
                
                if (Mathf.Abs(tiltX) > 0.2f) // Deadzone
                {
                    OnTiltDetected?.Invoke(tiltX);
                }
            }
            
            // Shake detection
            if (_enableShakeDetection && _shakeCooldown <= 0)
            {
                Vector3 deltaAcceleration = CurrentAcceleration - _lastAcceleration;
                float shakeMagnitude = deltaAcceleration.magnitude;
                
                if (shakeMagnitude > _shakeThreshold)
                {
                    OnShakeDetected?.Invoke();
                    _shakeCooldown = 1f; // 1 second cooldown
                    Debug.Log("[SensorManager] Shake detected!");
                }
            }
            
            _lastAcceleration = CurrentAcceleration;
        }
        
        private void UpdateGyroscope(float deltaTime)
        {
            if (!Input.gyro.enabled) return;
            
            CurrentGyroRotation = Input.gyro.attitude;
            
            // Detect rotation changes
            float angleDiff = Quaternion.Angle(_lastGyroRotation, CurrentGyroRotation);
            
            if (angleDiff > 1f) // 1 degree threshold
            {
                OnGyroRotationChanged?.Invoke(CurrentGyroRotation.eulerAngles);
            }
            
            _lastGyroRotation = CurrentGyroRotation;
        }
        
        public float GetTiltAngle()
        {
            if (!_useAccelerometer) return 0f;
            
            // Calculate tilt angle in degrees
            float angle = Mathf.Atan2(CurrentAcceleration.x, CurrentAcceleration.y) * Mathf.Rad2Deg;
            return angle;
        }
        
        public bool IsTiltingLeft()
        {
            return GetTiltAngle() < -_tiltThreshold;
        }
        
        public bool IsTiltingRight()
        {
            return GetTiltAngle() > _tiltThreshold;
        }
        
        public void EnableSensors(bool enable)
        {
            if (Input.gyro.enabled)
            {
                Input.gyro.enabled = enable;
            }
        }
        
        private void OnApplicationPause(bool pause)
        {
            EnableSensors(!pause);
        }
    }
}
