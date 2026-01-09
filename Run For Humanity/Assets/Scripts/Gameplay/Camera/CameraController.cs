using UnityEngine;
using RunForHumanity.Core.Input;

namespace RunForHumanity.Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0, 5, -10);
        public float smoothSpeed = 0.125f;
        public float parallaxIntensity = 2f; 

        void LateUpdate()
        {
            if (target == null) return;

            // Follow Target
            Vector3 desiredPos = target.position + offset;
            
            // Lock X slightly or follow smoothly? For runner, usually Camera X follows partially or is clamped.
            // Let's stick it mostly to center lane but follow slightly.
            desiredPos.x = target.position.x * 0.5f; 
            
            // Add Sensor Parallax (Rubric Requirement: "Uso visible de sensors")
            if (InputManager.Instance != null)
            {
                // Tilt X moves camera X slightly opposite or same direction
                desiredPos.x += InputManager.Instance.Tilt.x * parallaxIntensity;
            }

            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;

            transform.LookAt(target.position + Vector3.up * 1f);
        }
    }
}
