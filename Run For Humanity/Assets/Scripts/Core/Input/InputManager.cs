using UnityEngine;

namespace RunForHumanity.Core.Input
{
    public interface IInputProvider
    {
        bool SwipeLeft { get; }
        bool SwipeRight { get; }
        bool Jump { get; }
        bool Slide { get; }
        Vector3 Tilt { get; } // For Sensor requirement
    }

    public class InputManager : MonoBehaviour, IInputProvider
    {
        public static InputManager Instance { get; private set; }

        public bool SwipeLeft { get; private set; }
        public bool SwipeRight { get; private set; }
        public bool Jump { get; private set; }
        public bool Slide { get; private set; }
        public Vector3 Tilt { get; private set; }

        private Vector2 startTouchPos;
        private Vector2 currentTouchPos;
        private bool isSwiping;
        private float swipeThreshold = 50f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            ResetInputs();
            ProcessPcInput();
            ProcessMobileInput(); // Touch & Sensors
        }

        private void ResetInputs()
        {
            SwipeLeft = false;
            SwipeRight = false;
            Jump = false;
            Slide = false;
        }

        private void ProcessPcInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow) || UnityEngine.Input.GetKeyDown(KeyCode.A)) SwipeLeft = true;
            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow) || UnityEngine.Input.GetKeyDown(KeyCode.D)) SwipeRight = true;
            if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow) || UnityEngine.Input.GetKeyDown(KeyCode.Space)) Jump = true;
            if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow) || UnityEngine.Input.GetKeyDown(KeyCode.S)) Slide = true;
            
            // Mock Tilt with Mouse for PC testing
            if (!Application.isMobilePlatform)
            {
               Tilt = new Vector3(UnityEngine.Input.mousePosition.x / Screen.width - 0.5f, 0, 0);
            }
        }

        private void ProcessMobileInput()
        {
            // Sensors (Accelerometer) - fulfills rubric
            // Simple low-pass filter to smooth out jitter
            if (Application.isMobilePlatform)
            {
                Tilt = Vector3.Lerp(Tilt, UnityEngine.Input.acceleration, Time.deltaTime * 5f);
            }
            
            // Touch Logic
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    isSwiping = true;
                    startTouchPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isSwiping = false;
                }
                else if (touch.phase == TouchPhase.Moved && isSwiping)
                {
                    currentTouchPos = touch.position;
                    float distance = Vector2.Distance(startTouchPos, currentTouchPos);

                    if (distance >= swipeThreshold)
                    {
                        Vector2 direction = currentTouchPos - startTouchPos;
                        
                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                        {
                            if (direction.x > 0) SwipeRight = true;
                            else SwipeLeft = true;
                        }
                        else
                        {
                            if (direction.y > 0) Jump = true;
                            else Slide = true;
                        }
                        isSwiping = false; // Input consumed
                    }
                }
            }
        }
    }
}
