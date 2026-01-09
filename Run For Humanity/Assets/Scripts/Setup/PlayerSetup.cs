using UnityEngine;
using RunForHumanity.Gameplay;

namespace RunForHumanity.Setup
{
    /// <summary>
    /// Script para configurar el player automáticamente en la escena Gameplay
    /// Agregar este componente al GameObject Player
    /// </summary>
    public class PlayerSetup : MonoBehaviour
    {
        [Header("Player Configuration")]
        [SerializeField] private float startHeight = 2f;
        [SerializeField] private float forwardSpeed = 10f;
        [SerializeField] private float laneChangeSpeed = 10f;
        [SerializeField] private float jumpForce = 8f;
        
        private void Start()
        {
            ConfigurePlayer();
        }
        
        [ContextMenu("Configure Player")]
        public void ConfigurePlayer()
        {
            // Asegurar CharacterController
            CharacterController controller = GetComponent<CharacterController>();
            if (controller == null)
            {
                controller = gameObject.AddComponent<CharacterController>();
            }
            
            // Configurar CharacterController correctamente
            controller.center = new Vector3(0, 0, 0);
            controller.radius = 0.5f;
            controller.height = 2f;
            controller.skinWidth = 0.08f;
            controller.minMoveDistance = 0.001f;
            controller.slopeLimit = 45f;
            controller.stepOffset = 0.3f;
            
            Debug.Log("✓ CharacterController configurado correctamente");
            
            // Asegurar que hay un suelo
            EnsureGroundExists();
            
            // Asegurar PlayerController
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController == null)
            {
                playerController = gameObject.AddComponent<PlayerController>();
                Debug.Log("✓ PlayerController agregado");
            }
            
            // Configurar valores
            playerController.forwardSpeed = forwardSpeed;
            playerController.jumpForce = jumpForce;
            
            // Posicionar correctamente
            Vector3 startPos = new Vector3(
                LaneSystem.GetXPosition(LaneSystem.MIDDLE_LANE),
                startHeight,
                transform.position.z
            );
            transform.position = startPos;
            
            // Asegurar que esté mirando hacia adelante
            transform.rotation = Quaternion.identity;
            
            Debug.Log($"✓ Player configurado en posición: {startPos}");
        }
        
        private void EnsureGroundExists()
        {
            // Buscar si ya existe un suelo
            GameObject ground = GameObject.Find("Ground");
            if (ground == null)
            {
                // Crear un suelo simple
                ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
                ground.name = "Ground";
                ground.transform.position = Vector3.zero;
                ground.transform.localScale = new Vector3(10, 1, 100); // Ancho y largo
                ground.tag = "Track";
                
                // Asegurar que tiene collider
                MeshCollider collider = ground.GetComponent<MeshCollider>();
                if (collider != null)
                {
                    collider.convex = false;
                }
                
                Debug.Log("✓ Suelo creado automáticamente con tag Track");
            }
        }
        
        [ContextMenu("Test Jump")]
        public void TestJump()
        {
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Jump();
                Debug.Log("Jump ejecutado!");
            }
        }
        
        [ContextMenu("Test Move Left")]
        public void TestMoveLeft()
        {
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.MoveLeft();
                Debug.Log("Move Left ejecutado!");
            }
        }
        
        [ContextMenu("Test Move Right")]
        public void TestMoveRight()
        {
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.MoveRight();
                Debug.Log("Move Right ejecutado!");
            }
        }
    }
}
