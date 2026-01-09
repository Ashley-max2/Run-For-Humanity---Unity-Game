using UnityEngine;
using RunForHumanity.Core;

namespace RunForHumanity.Gameplay
{
    /// <summary>
    /// Interface for movement strategies
    /// SOLID: Open/Closed Principle - New movement types can be added without modifying existing code
    /// </summary>
    public interface IMovementStrategy
    {
        void Move(Transform transform, float speed, float deltaTime);
    }
    
    /// <summary>
    /// Forward automatic movement
    /// </summary>
    public class ForwardMovement : IMovementStrategy
    {
        public void Move(Transform transform, float speed, float deltaTime)
        {
            transform.Translate(Vector3.forward * speed * deltaTime, Space.World);
        }
    }
    
    /// <summary>
    /// Lane-based movement with smooth transitions
    /// </summary>
    public class LaneMovement : IMovementStrategy
    {
        private int _currentLane = 1; // 0, 1, 2 (left, center, right)
        private int _targetLane = 1;
        private float _laneChangeSpeed = 10f;
        
        public int CurrentLane => _currentLane;
        public int TargetLane => _targetLane;
        
        public void Move(Transform transform, float speed, float deltaTime)
        {
            // Smooth lane transition
            if (_currentLane != _targetLane)
            {
                float targetX = (_targetLane - 1) * GameConstants.LANE_WIDTH;
                Vector3 currentPos = transform.position;
                float newX = Mathf.Lerp(currentPos.x, targetX, _laneChangeSpeed * deltaTime);
                transform.position = new Vector3(newX, currentPos.y, currentPos.z);
                
                // Check if reached target
                if (Mathf.Abs(currentPos.x - targetX) < 0.01f)
                {
                    _currentLane = _targetLane;
                }
            }
        }
        
        public void ChangeLane(int direction)
        {
            _targetLane = Mathf.Clamp(_targetLane + direction, 0, GameConstants.LANE_COUNT - 1);
        }
        
        public void SetLane(int lane)
        {
            _targetLane = Mathf.Clamp(lane, 0, GameConstants.LANE_COUNT - 1);
        }
    }
}
