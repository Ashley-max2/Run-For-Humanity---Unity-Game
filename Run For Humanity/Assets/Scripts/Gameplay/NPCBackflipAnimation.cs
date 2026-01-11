using UnityEngine;
using DG.Tweening;

namespace RunForHumanity.Gameplay
{
    public class NPCBackflipAnimation : MonoBehaviour
    {
        [Header("Animación")]
        [SerializeField] private float crouchDuration = 0.3f;      // agacharse
        [SerializeField] private float jumpDuration = 0.8f;        // salto
        [SerializeField] private float delayBetweenFlips = 0.5f;   // Pausa
        
        [Header("Valores")]
        [SerializeField] private float crouchScale = 0.6f;         // Qué tanto se encoge (0.6 = 60%)
        [SerializeField] private float jumpHeight = 3f;            // Altura del salto
        [SerializeField] private float backflipRotation = -360f;   // Rotación (negativo = hacia atrás)
        
        [Header("Efectos")]
        [SerializeField] private Ease crouchEase = Ease.OutQuad;
        [SerializeField] private Ease jumpEase = Ease.OutQuad;
        
        private Vector3 originalScale;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Sequence backflipSequence;

        void Start()
        {
            // Guardar valores originales
            originalScale = transform.localScale;
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            
            // Iniciar la animación en loop
            StartBackflipLoop();
        }

        void StartBackflipLoop()
        {
            // Matar cualquier animación anterior
            backflipSequence?.Kill();
            
            // Crear secuencia de animación
            backflipSequence = DOTween.Sequence();
            
            // 1. AGACHARSE (encogerse)
            backflipSequence.Append(
                transform.DOScale(originalScale * crouchScale, crouchDuration)
                    .SetEase(crouchEase)
            );
            
            // 2. IMPULSO + BACKFLIP (todo junto)
            backflipSequence.Append(
                transform.DOJump(originalPosition, jumpHeight, 1, jumpDuration)
                    .SetEase(jumpEase)
            );
            
            // Rotación del backflip (al mismo tiempo que el salto)
            backflipSequence.Join(
                transform.DORotate(
                    new Vector3(backflipRotation, originalRotation.eulerAngles.y, originalRotation.eulerAngles.z),
                    jumpDuration,
                    RotateMode.FastBeyond360
                ).SetEase(Ease.InOutQuad)
            );
            
            // Volver a escala normal mientras cae
            backflipSequence.Join(
                transform.DOScale(originalScale, jumpDuration * 0.5f)
                    .SetDelay(jumpDuration * 0.5f)
            );
            
            // 3. PAUSA antes de repetir
            backflipSequence.AppendInterval(delayBetweenFlips);
            
            // 4. RESETEAR ROTACIÓN para el próximo loop
            backflipSequence.AppendCallback(() => {
                transform.rotation = originalRotation;
            });
            
            // LOOP INFINITO
            backflipSequence.SetLoops(-1);
            
            Debug.Log("[NPCBackflip] Animación de backflip iniciada en loop");
        }

        void OnDestroy()
        {
            // Limpiar animaciones al destruir
            backflipSequence?.Kill();
        }

        // Para testing en el Inspector
        [ContextMenu("Restart Animation")]
        void RestartAnimation()
        {
            transform.localScale = originalScale;
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            StartBackflipLoop();
        }
    }
}
